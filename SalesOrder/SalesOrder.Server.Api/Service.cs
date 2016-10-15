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
        private ActorSystem actorSystem;

        public void Start()
        {
            var containerBuilder = new ContainerBuilder();

            // containerBuilder.RegisterType<SalesOrderProcessDriverActor>();

            IContainer container = containerBuilder.Build();

            actorSystem = ActorSystem.Create("SalesOrder-Server-Api");

            new AutoFacDependencyResolver(container, actorSystem);

            // actorSystem.ActorOf(actorSystem.DI().Props<SalesOrderProcessDriverActor>(), "SalesOrderProcessDriver");

            disposable = WebApp.Start(ConfigurationManager.AppSettings["ApiUrl"]);
        }

        public void Stop()
        {
            disposable.Dispose();
            actorSystem.Shutdown();
            actorSystem.AwaitTermination();
        }
    }
}
