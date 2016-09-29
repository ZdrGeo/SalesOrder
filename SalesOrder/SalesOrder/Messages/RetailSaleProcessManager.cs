using Akka.Actor;

namespace SalesOrder.Messages
{
    public class ProcessRetailSale : Message
    {
        public ProcessRetailSale(string id) 
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class RetailSaleProcessed : Message
    {
        public RetailSaleProcessed(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
