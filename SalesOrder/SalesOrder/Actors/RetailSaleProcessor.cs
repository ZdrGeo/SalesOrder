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
    public class RetailSaleProcessorActor : ReceiveActor
    {
        public RetailSaleProcessorActor(IRetailSaleProcessorService retailSaleProcessorService)
        {
            if (retailSaleProcessorService == null) { throw new ArgumentNullException("retailSaleProcessorService"); }

            this.retailSaleProcessorService = retailSaleProcessorService;

            Receive<CreateRetailSale>(message => CreateRetailSale(message));
        }

        private readonly ILoggingAdapter logger = Context.GetLogger();
        private readonly IRetailSaleProcessorService retailSaleProcessorService;

        private void CreateRetailSale(CreateRetailSale createRetailSale)
        {
            logger.Info("Create retail sale");

            string id = retailSaleProcessorService.Create(null);

            RetailSaleCreated retailSaleCreated = new RetailSaleCreated(createRetailSale.ProcessActor, id);

            Sender.Tell(retailSaleCreated);
        }
    }
}
