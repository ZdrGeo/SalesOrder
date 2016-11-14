using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class LockManager
    {
        private Dictionary<IActorRef, Dictionary<IActorRef, bool>> lockedActorSessionActors = new Dictionary<IActorRef, Dictionary<IActorRef, bool>>();
        private Dictionary<IActorRef, HashSet<IActorRef>> sessionActorLockedActors = new Dictionary<IActorRef, HashSet<IActorRef>>();

        public bool TryLock(IActorRef lockedActor, IActorRef sessionActor, bool exclusive = false)
        {
            bool locked = false;

            /*
            if (exclusive)
            {
                Dictionary<IActorRef, bool> sessionActors = lockedActorSessionActors[lockedActor];

                if (!lockedActorSessionActors.TryGetValue(lockedActor, sessionActors))
                {
                    sessionActors = new Dictionary<IActorRef, bool>();
                }

                if (lockedActorSessionActors.ContainsKey(lockedActor))
                {
                    Dictionary<IActorRef, bool> sessionActors = lockedActorSessionActors[lockedActor];

                    if (sessionActors.Count == 0)
                    {
                        sessionActors.Add(sessionActor, true);

                        locked = true;
                    }
                }
                else
                {
                    Dictionary<IActorRef, bool> sessionActors = new Dictionary<IActorRef, bool>();

                    sessionActors.Add(sessionActor, true);

                    lockedActorSessionActors.Add(lockedActor, sessionActors);

                    locked = true;
                }
            }
            else
            {
                if (lockedActorSessionActors.ContainsKey(lockedActor))
                {
                    Dictionary<IActorRef, bool> sessionActors = lockedActorSessionActors[lockedActor];

                    if (sessionActors.ContainsKey(sessionActor))
                    {
                        if (!sessionActors[sessionActor])
                        {
                            sessionActors.Add(sessionActor, false);

                            locked = true;
                        }
                    }
                }
            }
            */

            return locked;
        }

        public void Unlock(IActorRef lockedActor, IActorRef sessionActor)
        {
            if (lockedActorSessionActors.ContainsKey(lockedActor))
            {
                Dictionary<IActorRef, bool> sessionActors = lockedActorSessionActors[lockedActor];

                if (sessionActors.ContainsKey(sessionActor))
                {
                    bool exclusive = sessionActors[sessionActor];
                }
            }
       }

        public void UnlockAll(IActorRef sessionActor)
        {
            // lockedActorToSessionActor.Add(lockedActor, sessionActor);
            // sessionActorToLockedActor
        }
    }

    public class SalesOrderCollectionActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();
        private readonly LockManager lockManager = new LockManager();
        private ICancelable cancelable;

        public SalesOrderCollectionActor()
        {
            Receive<CreateSalesOrder>(message => CreateSalesOrder(message));
            Receive<DestroySalesOrder>(message => DestroySalesOrder(message));
            Receive<ReleaseLock>(message => ReleaseLock(message));
            Receive<ReleaseLocks>(message => ReleaseLocks(message));
        }

        private void CreateSalesOrder(CreateSalesOrder createSalesOrder)
        {
            logger.Info("Create purchase order (Number: {0})", createSalesOrder.Number);

            string id = string.Empty;

            IActorRef SalesOrderActor = Context.ActorOf(Context.DI().Props<SalesOrderActor>(), $"SalesOrder-{ id }");

            // locks.Add(SalesOrderActor, new Lock(createSalesOrder.SessionActor, DateTime.Now));

            SalesOrderActor.Forward(createSalesOrder);

            SalesOrderCreated SalesOrderCreated = new SalesOrderCreated(id, createSalesOrder.Number);

            Sender.Tell(SalesOrderCreated);
        }

        private void DestroySalesOrder(DestroySalesOrder destroySalesOrder)
        {
            logger.Info("Destroy purchase order (ID: {0})", destroySalesOrder.Id);

            IActorRef SalesOrderActor = Context.Child($"SalesOrder-{ destroySalesOrder.Id }");

            if (SalesOrderActor.IsNobody())
            {

            }

            SalesOrderActor.Forward(destroySalesOrder);

            // locks.Remove(SalesOrderActor);

            SalesOrderDestroyed SalesOrderDestroyed = new SalesOrderDestroyed(destroySalesOrder.Id);

            Sender.Tell(SalesOrderDestroyed);
        }

        private void ReleaseLock(ReleaseLock releaseLock)
        {
            logger.Info("Release lock");

        }

        private void ReleaseLocks(ReleaseLocks releaseLocks)
        {
            logger.Info("Release locks");

        }

        protected override void PreStart()
        {
            ReleaseLocks releaseLocks = new ReleaseLocks(string.Empty, ActorRefs.Nobody);

            cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(20), Self, releaseLocks, Self);

            base.PreStart();
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
