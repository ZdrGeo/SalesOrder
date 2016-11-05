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
    public class SessionCollectionActor : ReceiveActor
    {
        private readonly ILoggingAdapter logger = Context.GetLogger();

        public SessionCollectionActor()
        {
            Receive<CreateSession>(message => CreateSession(message));
            Receive<DestroySession>(message => DestroySession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            logger.Info("Create session (ID: {0}, UserID: {1})", createSession.ID, createSession.UserID);

            IActorRef sessionActor = Context.ActorOf(Context.DI().Props<SessionActor>(), $"Session-{ createSession.ID }");

            sessionActor.Forward(createSession);

            SessionCreated sessionCreated = new SessionCreated(createSession.ID);

            Sender.Tell(sessionCreated);
        }

        private void DestroySession(DestroySession destroySession)
        {
            logger.Info("Destroy session (ID: {0})", destroySession.ID);

            SessionDestroyed sessionDestroyed = new SessionDestroyed(destroySession.ID);

            Sender.Tell(sessionDestroyed);
        }
    }
}
