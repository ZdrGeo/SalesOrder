using Akka.Actor;

namespace SalesOrder.Messages.Commands
{
    public class CreateSalesOrder : SessionCommand
    {
        public CreateSalesOrder(string sessionId, IActorRef sessionActor, string id, string number) : base (sessionId, sessionActor)
        {
            Id = id;
            Number = number;
        }

        public string Id { get; }
        public string Number { get; }
    }

    public class DestroySalesOrder : SessionCommand
    {
        public DestroySalesOrder(string sessionId, IActorRef sessionActor, string id) : base (sessionId, sessionActor)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class AddSalesOrderLine : SessionCommand
    {
        public AddSalesOrderLine(string sessionId, IActorRef sessionActor, string number) : base (sessionId, sessionActor)
        {
            Number = number;
        }

        public string Number { get; }
    }

    public class RemoveSalesOrderLine : SessionCommand
    {
        public RemoveSalesOrderLine(string sessionId, IActorRef sessionActor, string id) : base (sessionId, sessionActor)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
