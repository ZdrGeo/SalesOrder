using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.AutoFac;
using Autofac;
using Topshelf;

using SalesOrder.Actors;

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
