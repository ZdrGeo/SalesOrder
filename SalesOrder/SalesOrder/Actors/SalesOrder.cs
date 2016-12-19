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
        public Document Document { get; set; }
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
        private Document Document = new Document();
        private int count;
        private const int SNAPSHOT_COUNT = 10;

        public override string PersistenceId
        {
            get
            {
                return $"sales-order-{ Document.Id }";
            }
        }

        private void SaveSnapshot()
        {
            if (count == SNAPSHOT_COUNT)
            {
                SaveSnapshot(Document);
                count = 0;
            }
            else { count++; }
        }

        private void CreateSalesOrder(CreateSalesOrder createSalesOrder)
        {
            logger.Info("Create purchase order (Id: {0})", createSalesOrder.Id);

            var salesOrderCreated = new SalesOrderCreated(createSalesOrder.Id, createSalesOrder.Number);

            Persist(salesOrderCreated, @event => {
                this.SalesOrderCreated(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderCreated);
            });
        }

        private void DestroySalesOrder(DestroySalesOrder destroySalesOrder)
        {
            logger.Info("Destroy purchase order (Id: {0})", destroySalesOrder.Id);

            var salesOrderDestroyed = new SalesOrderDestroyed(Document.Id);

            Persist(salesOrderDestroyed, @event => {
                this.SalesOrderDestroyed(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderDestroyed);
            });
        }

        private void AddSalesOrderLine(AddSalesOrderLine addSalesOrderLine)
        {
            logger.Info("Add purchase order line (Number: {0})", addSalesOrderLine.Number);

            string id = string.Empty;

            var salesOrderLineAdded = new SalesOrderLineAdded(id, addSalesOrderLine.Number);

            Persist(salesOrderLineAdded, @event => {
                this.SalesOrderLineAdded(@event);
                SaveSnapshot();
                // Sender.Tell(SalesOrderLineAdded);
            });
        }

        private void RemoveSalesOrderLine(RemoveSalesOrderLine removeSalesOrderLine)
        {
            logger.Info("Remove purchase order line (ID: {0})", removeSalesOrderLine.Id);

            var salesOrderLineRemoved = new SalesOrderLineRemoved(removeSalesOrderLine.Id);

            Persist(salesOrderLineRemoved, @event => {
                this.SalesOrderLineRemoved(@event);
                SaveSnapshot();
                // Sender.Tell(salesOrderLineAdded);
            });
        }

        private void SalesOrderCreated(SalesOrderCreated SalesOrderCreated)
        {
            Document.Id = SalesOrderCreated.Id;
            Document.Number = SalesOrderCreated.Number;
        }

        private void SalesOrderDestroyed(SalesOrderDestroyed SalesOrderDestroyed)
        {

        }

        private void SalesOrderLineAdded(SalesOrderLineAdded SalesOrderLineAdded)
        {
            var line = new Line();

            line.Id = SalesOrderLineAdded.Id;
            line.Number = SalesOrderLineAdded.Number;

            Document.Lines.Add(line);
        }

        private void SalesOrderLineRemoved(SalesOrderLineRemoved SalesOrderLineRemoved)
        {
            Line line = Document.Lines.Single(l => l.Id == SalesOrderLineRemoved.Id);

            Document.Lines.Remove(line);
        }

        private void SnapshotOffered(SnapshotOffer snapshotOffer)
        {
            Document = (Document)snapshotOffer.Snapshot;
        }
    }
}
