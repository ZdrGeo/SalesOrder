using Akka.Actor;

namespace SalesOrder.Messages
{
    public class ReleaseLock : SessionMessage
    {
        public ReleaseLock(IActorRef lockedActor, IActorRef sessionActor) : base (sessionActor)
        {
            LockedActor = lockedActor;
        }

        public IActorRef LockedActor { get; }
    }

    public class ReleaseLocks : SessionMessage
    {
        public ReleaseLocks(IActorRef sessionActor) : base (sessionActor) { }
    }
}
