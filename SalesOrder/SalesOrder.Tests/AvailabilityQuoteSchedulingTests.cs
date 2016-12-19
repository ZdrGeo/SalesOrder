using Akka.TestKit.Xunit2;
using Xunit;
using Akka.Actor;
using SalesOrder.Messages;
using SalesOrder.Actors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesOrder.Tests
{
    public class AvailabilityQuoteSchedulingTests : TestKit
    {
        public AvailabilityQuoteSchedulingTests() : base (@"
          akka.persistence {
            journal {
              plugin = ""akka.persistence.journal.sql-server""
              sql-server {
                class = ""Akka.Persistence.SqlServer.Journal.SqlServerJournal, Akka.Persistence.SqlServer""
                schema-name = dbo
                auto-initialize = on
                connection-string = ""Data Source=BERILLIUM;Initial Catalog=Atlas;Integrated Security=True""
              }
            }

            snapshot-store {
              plugin = ""akka.persistence.snapshot-store.sql-server""
              sql-server {
                class = ""Akka.Persistence.SqlServer.Snapshot.SqlServerSnapshotStore, Akka.Persistence.SqlServer""
                schema-name = dbo
                auto-initialize = on
                connection-string = ""Data Source=BERILLIUM;Initial Catalog=Atlas;Integrated Security=True""
              }
            }

            at-least-once-delivery {
              redeliver-interval = 5
            }
          }
        ") { }

        [Fact]
        public void AvailabilityQuoteShouldBeReceived()
        {
            long deliveryId = 1;
            string partnerId = "1";
            string productId = "1";
            decimal quantity = 1;

            IActorRef availabilityQuoteAgentActor = Sys.ActorOf(Props.Create<AvailabilityQuoteAgentActor>(partnerId), $"availability-quote-agent-{ partnerId }");

            var deliverAtLeastOnce = new DeliverAtLeastOnce<RequestAvailabilityQuote>(deliveryId, new RequestAvailabilityQuote(productId, quantity));

            availabilityQuoteAgentActor.Tell(deliverAtLeastOnce);

            AtLeastOnceDelivered atLeastOnceDelivered = ExpectMsg<AtLeastOnceDelivered>(message => message.DeliveryId == deliveryId);
            ResponseAvailabilityQuote responseAvailabilityQuote = ExpectMsg<ResponseAvailabilityQuote>(
                message =>
                message.PartnerId == partnerId &&
                message.ProductId == productId &&
                message.AvailabilityQuotes.Count(
                    availabilityQuote =>
                    availabilityQuote.ExpiryDate == DateTime.Today &&
                    availabilityQuote.Quantity == quantity
                ) == 1
            );
        }

        [Fact]
        public void AvailabilityQuoteShouldBeScheduled()
        {
            IActorRef availabilityQuoteSchedulerActor = Sys.ActorOf<AvailabilityQuoteSchedulerActor>("availability-quote-scheduler");

            var scheduleAvailabilityQuote = new ScheduleAvailabilityQuote(new HashSet<string>() { "1", "2", "3", "4", "5" }, "1", 1);

            availabilityQuoteSchedulerActor.Tell(scheduleAvailabilityQuote);

            AvailabilityQuoteScheduled availabilityQuoteScheduled = ExpectMsg<AvailabilityQuoteScheduled>(TimeSpan.FromMinutes(10));
        }
    }
}
