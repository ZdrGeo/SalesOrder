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

namespace SalesOrder.States
{
    internal class SalesOrderState
    {
        public string Id { get; set; }
        public SalesOrderDocument SalesOrderDocument { get; set; }
    }
}

namespace SalesOrder.Actors
{
    public class SalesOrderActor : ReceivePersistentActor
    {
        public SalesOrderActor()
        {
            Command<CreateSalesOrder>(command => CreateSalesOrder(command));
            Command<DestroySalesOrder>(command => DestroySalesOrder(command));
            Command<AddSalesOrderLine>(command => AddSalesOrderLine(command));
            Command<RemoveSalesOrderLine>(command => RemoveSalesOrderLine(command));

            Recover<SalesOrderCreated>(@event => SalesOrderCreated(@event));
            Recover<SalesOrderDestroyed>(@event => SalesOrderDestroyed(@event));
            Recover<SalesOrderLineAdded>(@event => SalesOrderLineAdded(@event));
            Recover<SalesOrderLineRemoved>(@event => SalesOrderLineRemoved(@event));
            Recover<SnapshotOffer>(snapshotOffer => SnapshotOffered(snapshotOffer));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private SalesOrderDocument SalesOrderDocument = new SalesOrderDocument();
        private int count;
        private const int SNAPSHOT_COUNT = 10;

        public override string PersistenceId
        {
            get
            {
                return $"SalesOrder-{ SalesOrderDocument.Id }";
            }
        }

        private void SaveSnapshot()
        {
            if (count == SNAPSHOT_COUNT)
            {
                SaveSnapshot(SalesOrderDocument);
                count = 0;
            }
            else { count++; }
        }

        private void CreateSalesOrder(CreateSalesOrder createSalesOrder)
        {
            logger.Info("Create purchase order (ID: {0})", createSalesOrder.ID);

            SalesOrderCreated SalesOrderCreated = new SalesOrderCreated(createSalesOrder.ID, createSalesOrder.Number);

            Persist(SalesOrderCreated, @event => {
                SalesOrderCreated(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderCreated);
            });
        }

        private void DestroySalesOrder(DestroySalesOrder destroySalesOrder)
        {
            logger.Info("Destroy purchase order (ID: {0})", destroySalesOrder.ID);

            SalesOrderDestroyed SalesOrderDestroyed = new SalesOrderDestroyed(SalesOrderDocument.ID);

            Persist(SalesOrderDestroyed, @event => {
                SalesOrderDestroyed(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderDestroyed);
            });
        }

        private void AddSalesOrderLine(AddSalesOrderLine addSalesOrderLine)
        {
            logger.Info("Add purchase order line (Number: {0})", addSalesOrderLine.Number);

            int id = 0;

            SalesOrderLineAdded SalesOrderLineAdded = new SalesOrderLineAdded(id, addSalesOrderLine.Number);

            Persist(SalesOrderLineAdded, @event => {
                SalesOrderLineAdded(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderLineAdded);
            });
        }

        private void RemoveSalesOrderLine(RemoveSalesOrderLine removeSalesOrderLine)
        {
            logger.Info("Remove purchase order line (ID: {0})", removeSalesOrderLine.ID);

            SalesOrderLineRemoved SalesOrderLineRemoved = new SalesOrderLineRemoved(removeSalesOrderLine.ID);

            Persist(SalesOrderLineRemoved, @event => {
                SalesOrderLineRemoved(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderLineAdded);
            });
        }

        private void SalesOrderCreated(SalesOrderCreated SalesOrderCreated)
        {
            SalesOrderDocument.Id = SalesOrderCreated.Id;
            SalesOrderDocument.Number = SalesOrderCreated.Number;
        }

        private void SalesOrderDestroyed(SalesOrderDestroyed SalesOrderDestroyed)
        {

        }

        private void SalesOrderLineAdded(SalesOrderLineAdded SalesOrderLineAdded)
        {
            SalesOrderLine SalesOrderLine = new SalesOrderLine();

            SalesOrderLine.Id = SalesOrderLineAdded.Id;
            SalesOrderLine.Number = SalesOrderLineAdded.Number;

            SalesOrderDocument.Lines.Add(SalesOrderLine);
        }

        private void SalesOrderLineRemoved(SalesOrderLineRemoved SalesOrderLineRemoved)
        {
            SalesOrderLine SalesOrderLine = (SalesOrderLine)SalesOrderDocument.Lines.Single(line => line.Id == SalesOrderLineRemoved.Id);

            SalesOrderDocument.Lines.Remove(SalesOrderLine);
        }

        private void SnapshotOffered(SnapshotOffer snapshotOffer)
        {
            SalesOrderDocument = (SalesOrderDocument)snapshotOffer.Snapshot;
        }
    }
}
