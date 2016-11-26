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
    public class RetailSaleProcessManagerActor : ProcessManagerActor
    {
        public RetailSaleProcessManagerActor()
        {
            clientProcessorActor = Context.ActorOf(Context.DI().Props<ClientProcessorActor>(), $"client-processor-{ 0 }");
            retailSaleProcessorActor = Context.ActorOf(Context.DI().Props<RetailSaleProcessorActor>(), $"retail-sale-processor-{ 0 }");

            Receive<DeliverAtLeastOnce<ProcessRetailSale>>(message => ProcessRetailSale(message));
            Receive<RetailSaleProcessCreated>(message => RetailSaleProcessCreated(message));
            Receive<ClientCreated>(message => ClientCreated(message));
            Receive<ClientStored>(message => ClientStored(message));
            Receive<RetailSaleCreated>(message => RetailSaleCreated(message));
            Receive<RetailSaleStored>(message => RetailSaleStored(message));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private IActorRef clientProcessorActor;
        private IActorRef retailSaleProcessorActor;

        private void ProcessRetailSale(DeliverAtLeastOnce<ProcessRetailSale> deliverAtLeastOnce)
        {
            logger.Info("Process retail sale (Id: {0})", deliverAtLeastOnce.Message.Id);

            Sender.Tell(new AtLeastOnceDelivered(deliverAtLeastOnce.DeliveryId));

            string processId = string.Empty;

            IActorRef processActor = Context.ActorOf(Context.DI().Props<RetailSaleProcessActor>(), $"retail-sale-process-{ processId }");

            CreateProcess(processId, processActor);
        }

        private void RetailSaleProcessCreated(RetailSaleProcessCreated retailSaleProcessCreated)
        {
            string name = string.Empty;

            CreateClient createClient = new CreateClient(retailSaleProcessCreated.ProcessActor, name);

            clientProcessorActor.Tell(createClient);
        }

        private void ClientCreated(ClientCreated clientCreated)
        {
            StoreClient storeClient = new StoreClient(clientCreated.Id);

            clientCreated.ProcessActor.Tell(storeClient);
        }

        private void ClientStored(ClientStored clientStored)
        {
            // CreateRetailSale createRetailSale = new CreateRetailSale(clientStored.ProcessActor);

            // retailSaleProcessorActor.Tell(createRetailSale);
        }

        private void RetailSaleCreated(RetailSaleCreated retailSaleCreated)
        {
            StoreRetailSale storeRetailSale = new StoreRetailSale(retailSaleCreated.Id);

            retailSaleCreated.ProcessActor.Tell(storeRetailSale);
        }

        private void RetailSaleStored(RetailSaleStored retailSaleStored)
        {
            // CreateProcess(retailSaleStored.Id, retailSaleStored.ProcessActor);
        }

        protected override void OnCreateProcess(IActorRef actor)
        {
            string retailSaleId = string.Empty;

            CreateRetailSaleProcess createRetailSaleProcess = new CreateRetailSaleProcess(retailSaleId);

            actor.Tell(createRetailSaleProcess);
        }

        protected override void OnDestroyProcess(IActorRef actor)
        {
            Context.Stop(actor);
        }
    }
}
