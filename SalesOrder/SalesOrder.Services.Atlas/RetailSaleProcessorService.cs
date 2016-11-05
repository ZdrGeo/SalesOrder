using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;
using SalesOrder.Models.Atlas;

namespace SalesOrder.Services.Atlas
{
    public class RetailSaleProcessorService : IRetailSaleProcessorService
    {
        public RetailSaleProcessorService(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory, IDocumentRepository retailSaleRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (unitOfWorkIsolatedFactory == null) { throw new ArgumentNullException("unitOfWorkIsolatedFactory"); }
            if (retailSaleRepository == null) { throw new ArgumentNullException("retailSaleRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.unitOfWorkIsolatedFactory = unitOfWorkIsolatedFactory;
            this.retailSaleRepository = retailSaleRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory;
        private IDocumentRepository retailSaleRepository;

        public string Create(Document document)
        {
            unitOfWorkIsolatedFactory.With(
                unitOfWork =>
                {
                    retailSaleRepository.Enlist(unitOfWork);

                    document.Id = retailSaleRepository.GetNewId();
                    retailSaleRepository.Add(document);

                    unitOfWork.Complete();

                    retailSaleRepository.Delist();
                }
            );

            return string.Empty;
        }
    }
}
