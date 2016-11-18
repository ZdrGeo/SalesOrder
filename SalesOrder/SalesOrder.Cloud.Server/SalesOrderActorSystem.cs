using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Cluster;

using SalesOrder.Actors;
using System.Threading;

namespace SalesOrder.Cloud.Server
{
    public static class SalesOrderActorSystem
    {
        private static readonly ManualResetEvent memberRemoved = new ManualResetEvent(false);
        public static ActorSystem ActorSystem { get; private set; }
        // public static IActorRef SessionCollectionActor { get; private set; }

        public static void Start()
        {
            memberRemoved.Reset();

            ActorSystem = ActorSystem.Create("sales-order");

            // SessionCollectionActor = ActorSystem.ActorOf(Props.Create<SessionCollectionActor>(), "session-collection");
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
