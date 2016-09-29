using Akka.Actor;

namespace SalesOrder.Messages
{
    public class CreateRetailSaleProcess : Message
    {
        public CreateRetailSaleProcess(string retailSaleId)
        {
            RetailSaleId = retailSaleId;
        }

        public string RetailSaleId { get; }
    }

    public class RetailSaleProcessCreated : Message
    {
        public RetailSaleProcessCreated(IActorRef processActor)
        {
            ProcessActor = processActor;
        }

        public IActorRef ProcessActor { get; }
    }

    public class StoreClient : Message
    {
        public StoreClient(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class ClientStored : Message
    {
        public ClientStored(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class StoreRetailSale : Message
    {
        public StoreRetailSale(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public class RetailSaleStored : Message
    {
        public RetailSaleStored(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
