﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.AutoFac;

using SalesOrder.Actors;

namespace SalesOrder.Server
{
    public static class SalesOrderActorSystem
    {
        public static ActorSystem ActorSystem { get; private set; }
        // public static IActorRef SessionCollectionActor { get; private set; }

        public static void Start()
        {
            var containerBuilder = new ContainerBuilder();

            // containerBuilder.RegisterType<SalesOrderService>().As<ISalesOrderService>();
            // containerBuilder.RegisterType<SalesOrderActor>();
            // containerBuilder.RegisterType<SalesOrderLockActor>();
            // containerBuilder.RegisterType<SalesOrderCollectionActor>();
            // containerBuilder.RegisterType<SessionActor>();
            // containerBuilder.RegisterType<SessionCollectionActor>();
            // containerBuilder.RegisterType<RetailSaleProcessActor>();
            // containerBuilder.RegisterType<RetailSaleProcessManagerActor>();
            // containerBuilder.RegisterType<RetailSaleProcessDistributorActor>();

            IContainer container = containerBuilder.Build();

            ActorSystem = ActorSystem.Create("sales-order");

            new AutoFacDependencyResolver(container, ActorSystem);

            // SessionCollectionActor = ActorSystem.ActorOf(ActorSystem.DI().Props<SessionCollectionActor>(), "session-collection");
            // SessionCollectionActor = ActorSystem.ActorOf(Props.Create<SessionCollectionActor>(), "session-collection");
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
