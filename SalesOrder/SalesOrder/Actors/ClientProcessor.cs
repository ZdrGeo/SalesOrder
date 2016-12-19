using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;

using SalesOrder.Services;
using SalesOrder.Models;
using SalesOrder.States;
using SalesOrder.Messages;
using SalesOrder.Messages.Commands;
using SalesOrder.Messages.Events;

namespace SalesOrder.Actors
{
    public class ClientProcessorActor : ReceiveActor
    {
        public ClientProcessorActor(IClientProcessorService clientProcessorService)
        {
            if (clientProcessorService == null) { throw new ArgumentNullException("clientProcessorService"); }

            this.clientProcessorService = clientProcessorService;

            Receive<CreateClient>(message => CreateClient(message));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private readonly IClientProcessorService clientProcessorService;

        private void CreateClient(CreateClient createClient)
        {
            logger.Info("Create client (Name: {0})", createClient.Name);

            string id = clientProcessorService.Create(null);

            var clientCreated = new ClientCreated(createClient.ProcessActor, id);

            Sender.Tell(clientCreated);
        }
    }
}
