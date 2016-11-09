using Akka.TestKit.Xunit2;
using Xunit;
using Akka.Actor;
using SalesOrder.Messages;
using SalesOrder.Actors;

namespace SalesOrder.Tests
{
    public class SalesOrderDistributionTests : TestKit
    {
        [Fact]
        public void SalesOrderShouldBeDistributed()
        {
            IActorRef actor = ActorOf<RetailSaleDistributorActor>("RetailSaleDistributorActor");

            DistributeRetailSale distributeRetailSale = new DistributeRetailSale(string.Empty);

            actor.Tell(distributeRetailSale);

            RetailSaleDistributed retailSaleDistributed = ExpectMsg<RetailSaleDistributed>(message => message.RetailSaleId == distributeRetailSale.RetailSaleId);
        }
    }
}
