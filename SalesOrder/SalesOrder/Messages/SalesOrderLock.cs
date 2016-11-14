using Akka.Actor;

namespace SalesOrder.Messages
{
    public class LockSalesOrder : SessionMessage
    {
        public LockSalesOrder(string sessionId, IActorRef sessionActor, bool edit = false) : base (sessionId, sessionActor)
        {
            Edit = edit;
        }

        public bool Edit;
    }

    public class UnlockSalesOrder : SessionMessage
    {
        public UnlockSalesOrder(string sessionId, IActorRef sessionActor) : base (sessionId, sessionActor) { }
    }
}
