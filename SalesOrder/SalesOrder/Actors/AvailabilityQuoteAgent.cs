using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Services;
using SalesOrder.Models;
using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class AvailabilityQuoteAgentActor : ReceiveActor
    {
        public AvailabilityQuoteAgentActor(string partnerId)
        {
            PartnerId = partnerId;

            Receive<DeliverAtLeastOnce<RequestAvailabilityQuote>>(message => RequestAvailabilityQuote(message));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private readonly string PartnerId;

        private void RequestAvailabilityQuote(DeliverAtLeastOnce<RequestAvailabilityQuote> deliverAtLeastOnce)
        {
            logger.Info("Request availability quote (ProductId: {0}, Quantity {1})", deliverAtLeastOnce.Message.ProductId, deliverAtLeastOnce.Message.Quantity);

            var atLeastOnceDelivered = new AtLeastOnceDelivered(deliverAtLeastOnce.DeliveryId);

            Sender.Tell(atLeastOnceDelivered);

            var availabilityQuotes = new HashSet<AvailabilityQuote>() { new AvailabilityQuote(DateTime.Today, deliverAtLeastOnce.Message.Quantity) };

            var responseAvailabilityQuote = new ResponseAvailabilityQuote(deliverAtLeastOnce.Message.ProductId, PartnerId, availabilityQuotes);

            Sender.Tell(responseAvailabilityQuote);
        }
    }
}
