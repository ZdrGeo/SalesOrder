using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Akka.Actor;
using Akka.Routing;
using Akka.DI.Core;
using Akka.DI.AutoFac;

using SalesOrder.Actors;

namespace SalesOrder.Server.Api
{
    public static class SalesOrderActorSystem
    {
        public static ActorSystem ActorSystem { get; private set; }
        // public static IActorRef SalesOrderDistributorActor { get; private set; }
        public static IActorRef SessionRouterActor { get; private set; }

        public static void Start()
        {
            var containerBuilder = new ContainerBuilder();

            // containerBuilder.RegisterType<SessionCollectionActor>();

            IContainer container = containerBuilder.Build();

            ActorSystem = ActorSystem.Create("sales-order");

            new AutoFacDependencyResolver(container, ActorSystem);

            // SessionRouterActor = ActorSystem.ActorOf(ActorSystem.DI().Props<SessionCollectionActor>().WithRouter(FromConfig.Instance), "session-router");
            // SessionRouterActor = ActorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "session-router");
            SessionRouterActor = ActorSystem.ActorOf(Props.Create<SessionCollectionActor>().WithRouter(FromConfig.Instance), "session-router");
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
