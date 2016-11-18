using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Akka.Actor;
using Akka.Cluster;

namespace SalesOrder.Cloud.Server
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent memberRemoved = new ManualResetEvent(false);
        private readonly ManualResetEvent stopped = new ManualResetEvent(false);

        private ActorSystem actorSystem;

        public override void Run()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is running");

            try
            {
                RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                stopped.Set();
            }
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is starting");

            actorSystem = ActorSystem.Create("sales-order");

            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is stopping");

            cancellationTokenSource.Cancel();

            Cluster cluster = Cluster.Get(actorSystem);

            cluster.RegisterOnMemberRemoved(
                async () =>
                {
                    await actorSystem.Terminate();

                    memberRemoved.Set();
                }
            );

            cluster.Leave(cluster.SelfAddress);

            memberRemoved.WaitOne();

            stopped.WaitOne();

            base.OnStop();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
