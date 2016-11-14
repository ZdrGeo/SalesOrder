using Akka.Actor;

namespace SalesOrder.Messages
{
    public class DistributeRetailSaleProcess : Message
    {
        public DistributeRetailSaleProcess(string retailSaleId)
        {
            RetailSaleId = retailSaleId;
        }

        public string RetailSaleId { get; }
    }

    public class RetailSaleProcessDistributed : Message
    {
        public RetailSaleProcessDistributed(string retailSaleId)
        {
            RetailSaleId = retailSaleId;
        }

        public string RetailSaleId { get; }
    }
}
