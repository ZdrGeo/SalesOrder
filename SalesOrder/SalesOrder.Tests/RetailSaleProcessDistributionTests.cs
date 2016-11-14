using Akka.TestKit.Xunit2;
using Xunit;
using Akka.Actor;
using SalesOrder.Messages;
using SalesOrder.Actors;

namespace SalesOrder.Tests
{
    public class RetailSaleProcessDistributionTests : TestKit
    {
        [Fact]
        public void RetailSaleProcessShouldBeDistributed()
        {
            IActorRef actor = ActorOf<RetailSaleProcessDistributorActor>("retail-sale-process-distributor");

            var distributeRetailSaleProcess = new DistributeRetailSaleProcess(string.Empty);

            actor.Tell(distributeRetailSaleProcess);

            RetailSaleProcessDistributed retailSaleProcessDistributed = ExpectMsg<RetailSaleProcessDistributed>(message => message.RetailSaleId == distributeRetailSaleProcess.RetailSaleId);
        }
    }
}
