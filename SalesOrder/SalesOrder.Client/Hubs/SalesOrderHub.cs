using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using Akka.Actor;

using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Client.Hubs
{
    // [Authorize]
    public class SalesOrderHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void CreateSession()
        {
            CreateSession createSession = new CreateSession(Context.ConnectionId, Context.User.Identity.Name);

            SalesOrderActorSystem.SalesOrderBridgeActor.Tell(createSession);
        }

        private void DestroySession()
        {
            DestroySession destroySession = new DestroySession(Context.ConnectionId);

            SalesOrderActorSystem.SalesOrderBridgeActor.Tell(destroySession);
        }

        /*
        public void CreateSalesOrder(CreateSalesOrder createSalesOrder)
        {
            SalesOrderActorSystem.SalesOrderHubActor.Tell(createSalesOrder);
        }

        private void DestroySalesOrder(DestroySalesOrder destroySalesOrder)
        {
            SalesOrderActorSystem.SalesOrderHubActor.Tell(destroySalesOrder);
        }

        private void AddSalesOrderLine(AddSalesOrderLine addSalesOrderLine)
        {
            SalesOrderActorSystem.SalesOrderHubActor.Tell(addSalesOrderLine);
        }

        private void RemoveSalesOrderLine(RemoveSalesOrderLine removeSalesOrderLine)
        {
            SalesOrderActorSystem.SalesOrderHubActor.Tell(removeSalesOrderLine);
        }
        */
    }
}