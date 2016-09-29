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
        private ActorSystem actorSystem;

        public void Start()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            //containerBuilder.RegisterType<SalesOrderService>().As<ISalesOrderService>();
            //containerBuilder.RegisterType<SalesOrderActor>();
            //containerBuilder.RegisterType<SalesOrderLockActor>();
            //containerBuilder.RegisterType<SalesOrderCollectionActor>();
            //containerBuilder.RegisterType<SessionActor>();
            //containerBuilder.RegisterType<SessionCollectionActor>();
            containerBuilder.RegisterType<SalesOrderProcessActor>();
            containerBuilder.RegisterType<SalesOrderProcessManagerActor>();

            IContainer container = containerBuilder.Build();

            actorSystem = ActorSystem.Create("SalesOrder-Server");

            new AutoFacDependencyResolver(container, actorSystem);

            actorSystem.ActorOf(actorSystem.DI().Props<SalesOrderCollectionActor>(), "SalesOrderCollection");
            actorSystem.ActorOf(actorSystem.DI().Props<SessionCollectionActor>(), "SessionCollection");
        }

        public void Stop()
        {
            actorSystem.Shutdown();
            actorSystem.AwaitTermination();
        }
    }
}
