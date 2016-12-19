using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Routing;
using Akka.Persistence;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Models;
using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class RetailSaleProcessDistributorActor : AtLeastOnceDeliveryReceiveActor
    {
        public RetailSaleProcessDistributorActor()
        {
            // actor = Context.ActorOf<>();

            Command<DistributeRetailSaleProcess>(command => DistributeRetailSaleProcess(command));

            Command<AtLeastOnceDelivered>(command => AtLeastOnceDelivered(command));
            Command<SaveSnapshotSuccess>(command => SaveSnapshotSuccess(command));
            Command<SaveSnapshotFailure>(command => SaveSnapshotFailure(command));

            Recover<SnapshotOffer>(snapshotOffer => snapshotOffer.Snapshot is AtLeastOnceDeliverySnapshot, snapshotOffer => SnapshotOffered(snapshotOffer));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private ICancelable cancelable;
        private IActorRef actor;

        public override string PersistenceId
        {
            get
            {
                return $"retail-sale-process-distributor";
            }
        }

        private void DistributeRetailSaleProcess(DistributeRetailSaleProcess distributeRetailSaleProcess)
        {
            Deliver(actor.Path, deliveryId => new DeliverAtLeastOnce<DistributeRetailSaleProcess>(deliveryId, distributeRetailSaleProcess));

            SaveSnapshot(GetDeliverySnapshot());

            var retailSaleProcessDistributed = new RetailSaleProcessDistributed(distributeRetailSaleProcess.RetailSaleId);

            Sender.Tell(retailSaleProcessDistributed);
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
