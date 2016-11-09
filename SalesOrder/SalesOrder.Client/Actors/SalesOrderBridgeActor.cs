using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;
using Akka.Actor;

using SalesOrder.Client.Hubs;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;
using SalesOrder.Actors;

namespace SalesOrder.Client.Actors
{
    public class SalesOrderActorRefs
    {
        public const string SessionCollection = "akka.tcp://SalesOrder-Server@localhost:8091/user/SessionCollection";
    }

    public class SalesOrderBridgeActor : ReceiveActor
    {
        public SalesOrderBridgeActor(ISalesOrderEventSource SalesOrderEventSource)
        {
            this.SalesOrderEventSource = SalesOrderEventSource;

            Receive<CreateSession>(message => CreateSession(message));
            Receive<DestroySession>(message => DestroySession(message));

            /*
            Receive<CreateSalesOrder>(message => CreateSalesOrder(message));
            Receive<DestroySalesOrder>(message => DestroySalesOrder(message));
            Receive<AddSalesOrderLine>(message => AddSalesOrderLine(message));
            Receive<RemoveSalesOrderLine>(message => RemoveSalesOrderLine(message));
            */

            Receive<SessionCreated>(message => SessionCreated(message));

            /*
            Receive<SalesOrderCreated>(message => SalesOrderCreated(message));
            Receive<SalesOrderDestroyed>(message => SalesOrderDestroyed(message));
            Receive<SalesOrderLineAdded>(message => SalesOrderLineAdded(message));
            Receive<SalesOrderLineRemoved>(message => SalesOrderLineRemoved(message));
            */
        }

        private ISalesOrderEventSource SalesOrderEventSource;

        private void CreateSession(CreateSession createSession)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(createSession);
        }

        private void DestroySession(DestroySession destroySession)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(destroySession);
        }

        /*
        private void CreateSalesOrder(CreateSalesOrder createSalesOrder)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(createSalesOrder);
        }

        private void DestroySalesOrder(DestroySalesOrder destroySalesOrder)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(destroySalesOrder);
        }

        private void AddSalesOrderLine(AddSalesOrderLine addSalesOrderLine)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(addSalesOrderLine);
        }

        private void RemoveSalesOrderLine(RemoveSalesOrderLine removeSalesOrderLine)
        {
            IActorRef sessionActor = Context.ActorSelection(SalesOrderActorRefs.SessionCollection).ResolveOne(TimeSpan.FromSeconds(10)).Result;

            sessionActor.Tell(removeSalesOrderLine);
        }
        */

        public void SessionCreated(SessionCreated sessionCreated)
        {
            SalesOrderEventSource.SessionCreated(sessionCreated);
        }

        /*
        public void SalesOrderCreated(SalesOrderCreated SalesOrderCreated)
        {
            SalesOrderEventSource.SalesOrderCreated(SalesOrderCreated);
        }

        public void SalesOrderDestroyed(SalesOrderDestroyed SalesOrderDestroyed)
        {
            SalesOrderEventSource.SalesOrderDestroyed(SalesOrderDestroyed);
        }

        public void SalesOrderLineAdded(SalesOrderLineAdded SalesOrderLineAdded)
        {
            SalesOrderEventSource.SalesOrderLineAdded(SalesOrderLineAdded);
        }

        public void SalesOrderLineRemoved(SalesOrderLineRemoved SalesOrderLineRemoved)
        {
            SalesOrderEventSource.SalesOrderLineRemoved(SalesOrderLineRemoved);
        }
        */
    }
}