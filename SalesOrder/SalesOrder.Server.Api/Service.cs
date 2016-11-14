using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Microsoft.Owin.Hosting;
using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.AutoFac;
using Autofac;
using Topshelf;

using SalesOrder.Actors;

namespace SalesOrder.Server.Api
{
    public class Service
    {
        IDisposable disposable;

        public void Start()
        {
            SalesOrderActorSystem.Start();

            disposable = WebApp.Start(ConfigurationManager.AppSettings["ApiUrl"]);
        }

        public void Stop()
        {
            disposable.Dispose();

            SalesOrderActorSystem.Stop();
        }
    }
}
