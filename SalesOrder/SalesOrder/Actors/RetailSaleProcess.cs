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
    internal class RetailSaleProcessState
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string RetailSaleId { get; set; }
    }
}

namespace SalesOrder.Actors
{
    public class RetailSaleProcessActor : ReceivePersistentActor
    {
        public RetailSaleProcessActor()
        {
            Command<CreateRetailSaleProcess>(command => CreateRetailSaleProcess(command));
            Command<StoreClient>(command => StoreClient(command));
            Command<StoreRetailSale>(command => StoreRetailSale(command));

            Recover<RetailSaleProcessCreated>(@event => RetailSaleProcessCreated(@event));
            Recover<ClientStored>(@event => ClientStored(@event));
            Recover<RetailSaleStored>(@event => RetailSaleStored(@event));
            Recover<SnapshotOffer>(snapshotOffer => SnapshotOffered(snapshotOffer));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private RetailSaleProcessState retailSaleProcessState = new RetailSaleProcessState();
        private int count;
        private const int SNAPSHOT_COUNT = 10;

        public override string PersistenceId
        {
            get
            {
                return $"retail-sale-process-{ retailSaleProcessState.Id }";
            }
        }

        private void SaveSnapshot()
        {
            if (count == SNAPSHOT_COUNT)
            {
                SaveSnapshot(retailSaleProcessState);
                count = 0;
            }
            else { count++; }
        }

        private void CreateRetailSaleProcess(CreateRetailSaleProcess createRetailSaleProcess)
        {
            var retailSaleProcessCreated = new RetailSaleProcessCreated(Self);

            Persist(retailSaleProcessCreated, @event => {
                RetailSaleProcessCreated(@event);
                SaveSnapshot();
                Sender.Tell(retailSaleProcessCreated);
            });
        }

        private void StoreClient(StoreClient storeClient)
        {
            var clientStored = new ClientStored(storeClient.Id);

            Persist(clientStored, @event => {
                ClientStored(@event);
                SaveSnapshot();
                Sender.Tell(clientStored);
            });
        }

        private void StoreRetailSale(StoreRetailSale storeRetailSale)
        {
            var retailSaleStored = new RetailSaleStored(storeRetailSale.Id);

            Persist(retailSaleStored, @event => {
                RetailSaleStored(@event);
                SaveSnapshot();
                Sender.Tell(retailSaleStored);
            });
        }

        private void RetailSaleProcessCreated(RetailSaleProcessCreated retailSaleProcessCreated)
        {
        }

        private void ClientStored(ClientStored clientStored)
        {
            retailSaleProcessState.ClientId = clientStored.Id;
        }

        private void RetailSaleStored(RetailSaleStored retailSaleStored)
        {
            retailSaleProcessState.RetailSaleId = retailSaleStored.Id;
        }

        private void SnapshotOffered(SnapshotOffer snapshotOffer)
        {
            retailSaleProcessState = (RetailSaleProcessState)snapshotOffer.Snapshot;
        }
    }
}
