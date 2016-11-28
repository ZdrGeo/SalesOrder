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
        private readonly ManualResetEvent stop = new ManualResetEvent(false);
        // private readonly ManualResetEvent stopped = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is running...");

            stop.WaitOne();
            // stopped.Set();
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is starting...");

            SalesOrderActorSystem.Start();

            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Server is stopping...");

            stop.Set();
            // stopped.WaitOne();

            SalesOrderActorSystem.Stop();

            base.OnStop();
        }
    }
}
