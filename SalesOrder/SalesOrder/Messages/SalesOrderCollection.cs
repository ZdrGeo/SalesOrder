using Akka.Actor;

namespace SalesOrder.Messages
{
    public class ReleaseLock : SessionMessage
    {
        public ReleaseLock(IActorRef lockedActor, string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor)
        {
            LockedActor = lockedActor;
        }

        public IActorRef LockedActor { get; }
    }

    public class ReleaseLocks : SessionMessage
    {
        public ReleaseLocks(string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor) { }
    }
}
