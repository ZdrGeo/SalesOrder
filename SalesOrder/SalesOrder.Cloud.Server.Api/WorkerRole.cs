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
        private readonly ManualResetEvent stop = new ManualResetEvent(false);
        // private readonly ManualResetEvent stopped = new ManualResetEvent(false);
        private IDisposable disposable;

        public override void Run()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is running...");

            stop.WaitOne();
            // stopped.Set();
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is starting...");

            SalesOrderActorSystem.Start();

            ServicePointManager.DefaultConnectionLimit = 12;

            RoleInstanceEndpoint roleInstanceEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Api"];

            disposable = WebApp.Start<Startup>($"{ roleInstanceEndpoint.Protocol }://{ roleInstanceEndpoint.IPEndpoint }");

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server.Api is stopping...");

            stop.Set();
            // stopped.WaitOne();

            disposable?.Dispose();

            SalesOrderActorSystem.Stop();

            base.OnStop();
        }
    }
}
