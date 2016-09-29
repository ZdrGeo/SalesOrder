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
    }

    public class Document
    {
        private List<Line> lines = new List<Line>();
        public string Id { get; set; }
        public string Number { get; set; }
        public IList<Line> Lines => lines;
    }
}
