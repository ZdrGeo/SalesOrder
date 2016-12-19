using Akka.Actor;
using SalesOrder.Models;
using System;
using System.Collections.Generic;

namespace SalesOrder.Messages
{
    public class ScheduleAvailabilityQuote
    {
        public ScheduleAvailabilityQuote(IEnumerable<string> partnerIds, string productId, decimal quantity)
        {
            if (partnerIds == null) { throw new ArgumentNullException("partnerIds"); }

            PartnerIds = partnerIds;
            ProductId = productId;
            Quantity = quantity;
        }

        public IEnumerable<string> PartnerIds { get; }
        public string ProductId { get; }
        public decimal Quantity { get; }
    }

    public class AvailabilityQuoteScheduled { }

    public class RequestAvailabilityQuote
    {
        public RequestAvailabilityQuote(string productId, decimal quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public string ProductId { get; }
        public decimal Quantity { get; }
    }

    public class ResponseAvailabilityQuote
    {
        public ResponseAvailabilityQuote(string productId, string partnerId, IEnumerable<AvailabilityQuote> availabilityQuotes)
        {
            ProductId = productId;
            PartnerId = partnerId;
            AvailabilityQuotes = availabilityQuotes;
        }

        public string ProductId { get; }
        public string PartnerId { get; }
        public IEnumerable<AvailabilityQuote> AvailabilityQuotes { get; }
    }
}
