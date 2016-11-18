using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Akka.Actor;
using Akka.Cluster;
using Microsoft.Azure;

namespace SalesOrder.Cloud.Agent
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string queueName = "ProcessingQueue";

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent memberRemoved = new ManualResetEvent(false);
        private readonly ManualResetEvent stopped = new ManualResetEvent(false);

        private ActorSystem actorSystem;
        private QueueClient queueClient;

        public override void Run()
        {
            Trace.WriteLine("SalesOrder.Cloud.Server is running");

            queueClient.OnMessage(message =>
            {
                try
                {
                    Trace.WriteLine($"Processing Service Bus message: { message.SequenceNumber }");
                }
                catch
                {

                }
            });

            stopped.WaitOne();
        }

        public override bool OnStart()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Agent is stopping");

            actorSystem = ActorSystem.Create("sales-order");

            ServicePointManager.DefaultConnectionLimit = 12;

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("SalesOrder.Cloud.Agent is stopping");

            cancellationTokenSource.Cancel();

            queueClient.Close();

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

            stopped.Set();

            base.OnStop();
        }
    }
}
