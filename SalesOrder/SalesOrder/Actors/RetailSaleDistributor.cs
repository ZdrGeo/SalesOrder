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
    public class RetailSaleDistributorActor : AtLeastOnceDeliveryReceiveActor
    {
        public RetailSaleDistributorActor()
        {
            Command<DistributeRetailSale>(command => DistributeRetailSale(command));
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
                return "RetailSaleDistributor";
            }
        }

        private void DistributeRetailSale(DistributeRetailSale distributeRetailSale)
        {
            Deliver(actor.Path, deliveryId => new DeliverAtLeastOnce<DistributeRetailSale>(deliveryId, distributeRetailSale));

            SaveSnapshot(GetDeliverySnapshot());

            RetailSaleDistributed retailSaleDistributed = new RetailSaleDistributed(distributeRetailSale.RetailSaleId);

            Sender.Tell(retailSaleDistributed);
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
            DestroySession destroySession = new DestroySession(string.Empty);

            cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(20), Self, destroySession, ActorRefs.NoSender);

            base.PreStart();
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
