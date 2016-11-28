using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrder.Server
{
    public class Service
    {
        public void Start()
        {
            SalesOrderActorSystem.Start();
        }

        public void Stop()
        {
            SalesOrderActorSystem.Stop();
        }
    }
}
