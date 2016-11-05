namespace SalesOrder.Messages.Events
{
    public class SalesOrderCreated : Event
    {
        public SalesOrderCreated(string id, string number)
        {
            Id = id;
            Number = number;
        }

        public string Id { get; }
        public string Number { get; }
    }

    public class SalesOrderDestroyed : Event
    {
        public SalesOrderDestroyed(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class SalesOrderLineAdded : Event
    {
        public SalesOrderLineAdded(string id, string number)
        {
            Id = id;
            Number = number;
        }

        public string Id { get; }
        public string Number { get; }
    }

    public class SalesOrderLineRemoved : Event
    {
        public SalesOrderLineRemoved(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
