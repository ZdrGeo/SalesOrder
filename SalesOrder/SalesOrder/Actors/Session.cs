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
        private IActorRef SalesOrderCollectionActor;

        public SessionActor()
        {
            SalesOrderCollectionActor = Context.ActorSelection("/SalesOrderCollection").ResolveOne(TimeSpan.FromSeconds(10)).Result;

            Receive<CreateSession>(message => CreateSession(message));
            Receive<DestroySession>(message => DestroySession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            logger.Info("Create session (ID: {0}, UserID: {1})", createSession.ID, createSession.UserID);

            SessionCreated sessionCreated = new SessionCreated(createSession.ID);

            Sender.Tell(sessionCreated);
        }

        private void DestroySession(DestroySession destroySession)
        {
            logger.Info("Destroy session (ID: {0})", destroySession.ID);

            ReleaseLock releaseLock = new ReleaseLock(Self, ActorRefs.Nobody);

            SalesOrderCollectionActor.Tell(releaseLock);

            SessionDestroyed sessionDestroyed = new SessionDestroyed(destroySession.ID);

            Sender.Tell(sessionDestroyed);
        }

        protected override void PreStart()
        {
            DestroySession destroySession = new DestroySession(string.Empty);

            cancelable = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(20), Self, destroySession, ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            cancelable?.Cancel(false);

            base.PostStop();
        }
    }
}
