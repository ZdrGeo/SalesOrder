using Akka.Actor;

namespace SalesOrder.Messages.Commands
{
    public class CreateSalesOrder : SessionCommand
    {
        public CreateSalesOrder(IActorRef sessionActor, int id, string number) : base (sessionActor)
        {
            ID = id;
            Number = number;
        }

        public int ID { get; }
        public string Number { get; }
    }

    public class DestroySalesOrder : SessionCommand
    {
        public DestroySalesOrder(IActorRef sessionActor, int id) : base (sessionActor)
        {
            ID = id;
        }

        public int ID { get; }
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
        public RemoveSalesOrderLine(IActorRef sessionActor, int id) : base (sessionActor)
        {
            ID = id;
        }

        public int ID { get; }
    }
}
