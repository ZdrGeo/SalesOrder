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

using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class RetailSaleProcessDriver
    {
        public RetailSaleProcessDriver(ActorSystem actorSystem)
        {
            //...
            processManagerActor = actorSystem.ActorOf(actorSystem.DI().Props<RetailSaleProcessManagerActor>(), "RetailSaleProcessManager");

            ProcessRetailSale processRetailSale = new ProcessRetailSale(string.Empty);

            processManagerActor.Tell(processRetailSale);
        }

        //...
        private IActorRef processManagerActor;
    }
}
