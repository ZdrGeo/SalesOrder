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

using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public abstract class ProcessManagerActor : ReceiveActor
    {
        private Dictionary<string, IActorRef> processActors = new Dictionary<string, IActorRef>();

        protected void CreateProcess(string id, IActorRef actor)
        {
            if (!processActors.ContainsKey(id))
            {
                processActors.Add(id, actor);

                OnCreateProcess(actor);
            }
        }

        protected void DestroyProcess(string id)
        {
            if (processActors.ContainsKey(id))
            {
                OnDestroyProcess(processActors[id]);

                processActors.Remove(id);
            }
        }

        protected abstract void OnCreateProcess(IActorRef actor);
        protected abstract void OnDestroyProcess(IActorRef actor);
    }
}
