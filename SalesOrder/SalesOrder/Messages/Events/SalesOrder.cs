namespace SalesOrder.Messages.Events
{
    public class SalesOrderCreated : Event
    {
        public SalesOrderCreated(int id, string number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public string Number { get; }
    }

    public class SalesOrderDestroyed : Event
    {
        public SalesOrderDestroyed(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class SalesOrderLineAdded : Event
    {
        public SalesOrderLineAdded(int id, string number)
        {
            Id = id;
            Number = number;
        }

        public int Id { get; }
        public string Number { get; }
    }

    public class SalesOrderLineRemoved : Event
    {
        public SalesOrderLineRemoved(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
