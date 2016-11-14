using Akka.Actor;

namespace SalesOrder.Messages.Commands
{
    public abstract class SessionCommand : SessionMessage
    {
        public SessionCommand(string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor) { }
    }
}
