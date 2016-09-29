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
        public RetailSaleProcessorService(IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory)
        {
            if (unitOfWorkIsolatedFactory == null) { throw new ArgumentNullException("unitOfWorkIsolatedFactory"); }

            this.unitOfWorkIsolatedFactory = unitOfWorkIsolatedFactory;
        }

        private IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory;

        public string Create(RetailSale retailSale)
        {
            unitOfWorkIsolatedFactory.With(
                unitOfWork =>
                {
                    retailSale.Id = unitOfWork.RetailSaleRepository.GetNewId();
                    unitOfWork.RetailSaleRepository.Add(retailSale);
                    unitOfWork.Complete();
                }
            );

            return string.Empty;
        }
    }
}
