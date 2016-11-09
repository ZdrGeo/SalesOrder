using Akka.Actor;

namespace SalesOrder.Messages
{
    public class DistributeRetailSale : Message
    {
        public DistributeRetailSale(string retailSaleId)
        {
            RetailSaleId = retailSaleId;
        }

        public string RetailSaleId { get; }
    }

    public class RetailSaleDistributed : Message
    {
        public RetailSaleDistributed(string retailSaleId)
        {
            RetailSaleId = retailSaleId;
        }

        public string RetailSaleId { get; }
    }
}
