using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;
using Akka.Persistence;

using SalesOrder.Services;
using SalesOrder.Models;
using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class AvailabilityQuoteSchedulerActor : AtLeastOnceDeliveryReceiveActor
    {
        public AvailabilityQuoteSchedulerActor()
        {
            Command<ScheduleAvailabilityQuote>(command => ScheduleAvailabilityQuote(command));

            Command<AtLeastOnceDelivered>(command => AtLeastOnceDelivered(command));
            Command<SaveSnapshotSuccess>(command => SaveSnapshotSuccess(command));
            Command<SaveSnapshotFailure>(command => SaveSnapshotFailure(command));

            Recover<SnapshotOffer>(snapshotOffer => snapshotOffer.Snapshot is AtLeastOnceDeliverySnapshot, snapshotOffer => SnapshotOffered(snapshotOffer));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private ICancelable cancelable;

        public override string PersistenceId
        {
            get
            {
                return $"availability-quote-scheduler";
            }
        }

        private void ScheduleAvailabilityQuote(ScheduleAvailabilityQuote scheduleAvailabilityQuote)
        {
            logger.Info("Schedule availability quote (ProductId: {0}, Quantity {1})", scheduleAvailabilityQuote.ProductId, scheduleAvailabilityQuote.Quantity);

            foreach (string partnerId in scheduleAvailabilityQuote.PartnerIds)
            {
                string name = $"availability-quote-agent-{ partnerId }";

                IActorRef availabilityQuoteAgentActor = Context.Child(name);

                if (availabilityQuoteAgentActor.IsNobody())
                {
                    availabilityQuoteAgentActor = Context.ActorOf(Props.Create<AvailabilityQuoteAgentActor>(partnerId), name);
                }

                var requestAvailabilityQuote = new RequestAvailabilityQuote(scheduleAvailabilityQuote.ProductId, scheduleAvailabilityQuote.Quantity);

                Deliver(availabilityQuoteAgentActor.Path, deliveryId => new DeliverAtLeastOnce<RequestAvailabilityQuote>(deliveryId, requestAvailabilityQuote));

                SaveSnapshot(GetDeliverySnapshot());
            }

            var availabilityQuoteScheduled = new AvailabilityQuoteScheduled();

            Sender.Tell(availabilityQuoteScheduled);
        }

        private void AtLeastOnceDelivered(AtLeastOnceDelivered atLeastOnceDelivered)
        {
            ConfirmDelivery(atLeastOnceDelivered.DeliveryId);
        }

        private void SaveSnapshotSuccess(SaveSnapshotSuccess saveSnapshotSuccess)
        {
            var snapshotSelectionCriteria = new SnapshotSelectionCriteria(saveSnapshotSuccess.Metadata.SequenceNr, saveSnapshotSuccess.Metadata.Timestamp.AddMilliseconds(-1));

            DeleteSnapshots(snapshotSelectionCriteria);
        }

        private void SaveSnapshotFailure(SaveSnapshotFailure saveSnapshotFailure)
        {
            logger.Error(saveSnapshotFailure.Cause, "Fail to save snapshot");
        }

        private void SnapshotOffered(SnapshotOffer snapshotOffer)
        {
            SetDeliverySnapshot((Akka.Persistence.AtLeastOnceDeliverySnapshot)snapshotOffer.Snapshot);
        }

        protected override void PreStart()
        {
            // cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(20), Self, null, ActorRefs.NoSender);

            base.PreStart();
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
