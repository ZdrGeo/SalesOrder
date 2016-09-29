using Akka.Actor;

namespace SalesOrder.Messages
{
    public class CreateRetailSale : ProcessMessage
    {
        public CreateRetailSale(IActorRef processActor) : base (processActor)
        {
            
        }
    }

    public class RetailSaleCreated : ProcessMessage
    {
        public RetailSaleCreated(IActorRef processActor, string id) : base (processActor)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
