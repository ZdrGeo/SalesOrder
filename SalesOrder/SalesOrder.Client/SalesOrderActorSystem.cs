using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.AutoFac;
using Autofac;

using SalesOrder.Client.Actors;

namespace SalesOrder.Client
{
    public static class SalesOrderActorSystem
    {
        public static ActorSystem ActorSystem { get; private set; }
        public static IActorRef SalesOrderBridgeActor { get; private set; }

        public static void Start()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<SalesOrderEventSource>().As<ISalesOrderEventSource>();
            containerBuilder.RegisterType<SalesOrderBridgeActor>();

            IContainer container = containerBuilder.Build();

            ActorSystem = ActorSystem.Create("sales-order");

            new AutoFacDependencyResolver(container, ActorSystem);

            SalesOrderBridgeActor = ActorSystem.ActorOf(ActorSystem.DI().Props<SalesOrderBridgeActor>(), "sales-order-bridge");
        }

        public static void Stop()
        {
            if (ActorSystem == null)
            {
                throw new InvalidOperationException("Actor system is not started.");
            }

            ActorSystem.Shutdown();
            ActorSystem.AwaitTermination();
        }
    }
}