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
using System.Threading.Tasks;
using SalesOrder.Messages;

namespace SalesOrder.Cloud.Agent
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string queueName = "salesorder";

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent stopped = new ManualResetEvent(false);

        private QueueClient queueClient;

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            queueClient.OnMessageAsync(
                async message =>
                {
                    try
                    {
                        Trace.WriteLine($"Processing: { message.SequenceNumber }, Label: { message.Label }...");

                        string sessionId = $"{ message.Properties["SessionId"] }"; // message.SessionId
                        string userId = $"{ message.Properties["UserId"] }";

                        SessionFound sessionFound = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionFound>(new FindSession(sessionId), TimeSpan.FromSeconds(20));

                        if (sessionFound.SessionActor.IsNobody())
                        {
                            SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
                        }

                        // SessionCreated sessionCreated = await SalesOrderActorSystem.SessionRouterActor.Ask<SessionCreated>(new CreateSession(sessionId, userId), TimeSpan.FromSeconds(20));
                    }
                    catch (Exception exception)
                    {
                        Trace.WriteLine(exception);
                    }
                }
            );

            cancellationToken.WaitHandle.WaitOne();
        }

        public override void Run()
        {
            Trace.WriteLine("SalesOrder.Cloud.Server is running...");

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
            Trace.TraceInformation("SalesOrder.Cloud.Agent is stopping...");

            SalesOrderActorSystem.Start();

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
            Trace.TraceInformation("SalesOrder.Cloud.Agent is stopping...");

            cancellationTokenSource.Cancel();

            stopped.WaitOne();

            queueClient.Close();

            SalesOrderActorSystem.Stop();

            base.OnStop();
        }
    }
}
