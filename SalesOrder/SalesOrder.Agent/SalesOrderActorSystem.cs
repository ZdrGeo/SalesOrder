using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Routing;

using SalesOrder.Actors;
using System.Threading;
using Akka.Cluster;

namespace SalesOrder.Agent
{
    public static class SalesOrderActorSystem
    {
        private static readonly ManualResetEvent memberRemoved = new ManualResetEvent(false);
        public static ActorSystem ActorSystem { get; private set; }
        public static IActorRef SessionRouterActor { get; private set; }

        public static void Start()
        {
            memberRemoved.Reset();

            ActorSystem = ActorSystem.Create("sales-order");

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

            Cluster cluster = Cluster.Get(ActorSystem);

            cluster.RegisterOnMemberRemoved(
                async () =>
                {
                    await ActorSystem.Terminate();

                    memberRemoved.Set();
                }
            );

            cluster.Leave(cluster.SelfAddress);

            memberRemoved.WaitOne();
        }
    }
}
