using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class SessionActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();
        private ICancelable cancelable;
        // private IActorRef SalesOrderCollectionActor;

        public SessionActor()
        {
            // SalesOrderCollectionActor = Context.ActorSelection("/sales-order-collection").ResolveOne(TimeSpan.FromSeconds(10)).Result;

            Receive<CreateSession>(message => CreateSession(message));
            Receive<DestroySession>(message => DestroySession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            logger.Info("Create session (ID: {0}, UserID: {1})", createSession.Id, createSession.UserId);

            SessionCreated sessionCreated = new SessionCreated(createSession.Id, Self);

            Sender.Tell(sessionCreated);
        }

        private void DestroySession(DestroySession destroySession)
        {
            logger.Info("Destroy session (ID: {0})", destroySession.Id);

            // ReleaseLock releaseLock = new ReleaseLock(Self, ActorRefs.Nobody);

            // SalesOrderCollectionActor.Tell(releaseLock);

            Context.Stop(Self);
        }

        protected override void PreStart()
        {
            DestroySession destroySession = new DestroySession(string.Empty);

            cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20), Self, destroySession, ActorRefs.NoSender);

            base.PreStart();
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
