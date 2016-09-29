using Akka.Actor;

namespace SalesOrder.Messages.Commands
{
    public abstract class SessionCommand : SessionMessage
    {
        public SessionCommand(IActorRef sessionActor) : base (sessionActor) { }
    }
}
