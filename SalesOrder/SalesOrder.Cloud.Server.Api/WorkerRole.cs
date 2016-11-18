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
using Microsoft.Owin.Hosting;

namespace SalesOrder.Cloud.Server.Api
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent stopped = new ManualResetEvent(false);
        private IDisposable disposable;

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(2000);
            }
        }

        public override void Run()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is running...");

            try
            {
                RunAsync(cancellationTokenSource.Token).Wait();
            }
            finally
            {
                stopped.Set();
            }
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is starting...");

            SalesOrderActorSystem.Start();

            ServicePointManager.DefaultConnectionLimit = 12;

            RoleInstanceEndpoint roleInstanceEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Api"];

            disposable = WebApp.Start($"{ roleInstanceEndpoint.Protocol }://{ roleInstanceEndpoint.IPEndpoint }");

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is stopping...");

            cancellationTokenSource.Cancel();

            stopped.WaitOne();

            disposable?.Dispose();

            SalesOrderActorSystem.Stop();

            base.OnStop();
        }
    }
}
