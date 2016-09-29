using Akka.Actor;

namespace SalesOrder.Messages
{
    public class LockSalesOrder : SessionMessage
    {
        public LockSalesOrder(IActorRef sessionActor, bool edit = false) : base (sessionActor)
        {
            Edit = edit;
        }

        public bool Edit;
    }

    public class UnlockSalesOrder : SessionMessage
    {
        public UnlockSalesOrder(IActorRef sessionActor) : base (sessionActor) { }
    }
}
