using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrder.Models
{
    public class AvailabilityQuote
    {
        public AvailabilityQuote(DateTime expiryDate, decimal quantity)
        {
            ExpiryDate = expiryDate;
            Quantity = quantity;
        }

        public DateTime ExpiryDate { get; }
        public decimal Quantity { get; }
    }
}
