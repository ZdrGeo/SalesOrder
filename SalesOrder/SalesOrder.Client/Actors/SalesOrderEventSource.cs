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

namespace SalesOrder.Client.Actors
{
    public interface ISalesOrderEventSource
    {
        void SessionCreated(SessionCreated sessionCreated);
        /*
        void SalesOrderCreated(SalesOrderCreated SalesOrderCreated);
        void SalesOrderDestroyed(SalesOrderDestroyed SalesOrderDestroyed);
        void SalesOrderLineAdded(SalesOrderLineAdded SalesOrderLineAdded);
        void SalesOrderLineRemoved(SalesOrderLineRemoved SalesOrderLineRemoved);
        */
    }

    public class SalesOrderEventSource : ISalesOrderEventSource
    {
        static SalesOrderEventSource()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<SalesOrderHub>();
        }

        private static readonly IHubContext hubContext;

        public void SessionCreated(SessionCreated sessionCreated)
        {
            hubContext.Clients.Client(sessionCreated.SessionId).sessionCreated();
        }

        /*
        public void SalesOrderCreated(SalesOrderCreated SalesOrderCreated)
        {
            hubContext.Clients.Client(SalesOrderCreated.SessionID).SalesOrderCreated(SalesOrderCreated);
        }

        public void SalesOrderDestroyed(SalesOrderDestroyed SalesOrderDestroyed)
        {
            hubContext.Clients.Client(SalesOrderDestroyed.SessionID).SalesOrderDestroyed(SalesOrderDestroyed);
        }

        public void SalesOrderLineAdded(SalesOrderLineAdded SalesOrderLineAdded)
        {
            hubContext.Clients.Client(SalesOrderLineAdded.SessionID).SalesOrderLineAdded(SalesOrderLineAdded);
        }

        public void SalesOrderLineRemoved(SalesOrderLineRemoved SalesOrderLineRemoved)
        {
            hubContext.Clients.Client(sessionID).SalesOrderLineRemoved(SalesOrderLineRemoved);
        }
        */
    }
}