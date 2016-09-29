using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using Document.Models;
using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class SalesOrderLockActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();
        private IActorRef SalesOrderActor;
        private IActorRef editSessionActor = ActorRefs.Nobody;
        private HashSet<IActorRef> sessionActors = new HashSet<IActorRef>();

        public IStash Stash { get; set; }

        public SalesOrderLockActor(IActorRef SalesOrderActor)
        {
            if (SalesOrderActor == null) { throw new ArgumentNullException("SalesOrderActor"); }

            this.SalesOrderActor = SalesOrderActor;

            Unlocked();
        }

        private void Unlocked()
        {
            Receive<LockSalesOrder>(message => LockSalesOrder(message));
            Receive<UnlockSalesOrder>(message => UnlockSalesOrder(message));

            ReceiveAny(message => SalesOrderActor.Forward(message));
        }

        private void Locked()
        {
            Receive<LockSalesOrder>(message => LockSalesOrder(message));
            Receive<UnlockSalesOrder>(message => UnlockSalesOrder(message));

            ReceiveAny(message => SalesOrderActor.Forward(message));
        }

        private void LockSalesOrder(LockSalesOrder lockSalesOrder)
        {
            if (lockSalesOrder.Edit)
            {
                EditLockSalesOrder(lockSalesOrder.SessionActor);
            }
            else
            {
                LockSalesOrder(lockSalesOrder.SessionActor);
            }
        }

        private void EditLockSalesOrder(IActorRef sessionActor)
        {
            if (!editSessionActor.IsNobody() && editSessionActor != sessionActor)
            {
                return;
            }

            if (sessionActors.Any(sA => !sA.IsNobody() && sA != sessionActor))
            {
                return;
            }

            sessionActors.Remove(sessionActor);
            editSessionActor = sessionActor;
        }

        private void LockSalesOrder(IActorRef sessionActor)
        {
            if (!editSessionActor.IsNobody())
            {
                return;
            }

            if (sessionActors.Any(sA => !sA.IsNobody() && sA != sessionActor))
            {
                return;
            }

            sessionActors.Add(sessionActor);
        }

        private void UnlockSalesOrder(UnlockSalesOrder unlockSalesOrder)
        {
            if (editSessionActor == unlockSalesOrder.SessionActor)
            {
                editSessionActor = ActorRefs.Nobody;
            }

            sessionActors.Remove(unlockSalesOrder.SessionActor);
        }
    }
}
