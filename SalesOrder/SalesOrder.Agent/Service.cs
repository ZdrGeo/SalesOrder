using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SalesOrder.Messages;
using Akka.Actor;
using System.Net;
using Microsoft.ServiceBus;
using System.Configuration;

namespace SalesOrder.Agent
{
    public class Service
    {
        private const string queueName = "salesorder";

        private QueueClient queueClient;

        public void Start()
        {
            SalesOrderActorSystem.Start();

            ServicePointManager.DefaultConnectionLimit = 12;

            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);

            queueClient.OnMessageAsync(
                async message =>
                {
                    try
                    {
                        Console.WriteLine($"Processing: { message.SequenceNumber }, Label: { message.Label }...");

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
                        Console.WriteLine(exception);
                    }
                }
            );
        }

        public void Stop()
        {
            queueClient.Close();

            SalesOrderActorSystem.Stop();
        }
    }
}
