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
            Receive<FindSession>(message => FindSession(message));
        }

        private void CreateSession(CreateSession createSession)
        {
            // IActorRef sessionActor = Context.ActorOf(Context.DI().Props<SessionActor>(), $"session-{ createSession.SessionId }");
            IActorRef sessionActor = Context.ActorOf(Props.Create<SessionActor>(), $"session-{ createSession.SessionId }");

            sessionActor.Forward(createSession);
        }

        private void FindSession(FindSession findSession)
        {
            logger.Info("Find session (Id: {0})", findSession.SessionId);

            IActorRef sessionActor = Context.Child($"session-{ findSession.SessionId }");

            SessionFound sessionFound = new SessionFound(findSession.SessionId, sessionActor);

            Sender.Tell(sessionFound);
        }
    }
}
