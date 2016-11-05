using Akka.Actor;

namespace SalesOrder.Messages.Commands
{
    public class CreateSalesOrder : SessionCommand
    {
        public CreateSalesOrder(IActorRef sessionActor, string id, string number) : base (sessionActor)
        {
            Id = id;
            Number = number;
        }

        public string Id { get; }
        public string Number { get; }
    }

    public class DestroySalesOrder : SessionCommand
    {
        public DestroySalesOrder(IActorRef sessionActor, string id) : base (sessionActor)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class AddSalesOrderLine : SessionCommand
    {
        public AddSalesOrderLine(IActorRef sessionActor, string number) : base (sessionActor)
        {
            Number = number;
        }

        public string Number { get; }
    }

    public class RemoveSalesOrderLine : SessionCommand
    {
        public RemoveSalesOrderLine(IActorRef sessionActor, string id) : base (sessionActor)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
