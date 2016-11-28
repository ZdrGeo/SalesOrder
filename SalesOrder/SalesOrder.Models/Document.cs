using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrder.Models
{
    public class Line
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string ProductCode { get; set; }
        public decimal Quantity { get; set; }
    }

    public class Document
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public IList<Line> Lines { get; } = new List<Line>();
    }
}
