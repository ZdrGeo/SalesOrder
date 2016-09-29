using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using SalesOrder.Models;
using SalesOrder.Models.Atlas;

namespace SalesOrder.Services.Atlas
{
    public class ClientProcessorService : IClientProcessorService
    {
        public ClientProcessorService(IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory, IClientRepository clientRepository)
        {
            if (unitOfWorkIsolatedFactory == null) { throw new ArgumentNullException("unitOfWorkIsolatedFactory"); }
            if (clientRepository == null) { throw new ArgumentNullException("clientRepository"); }

            this.unitOfWorkIsolatedFactory = unitOfWorkIsolatedFactory;
            this.clientRepository = clientRepository;
        }

        private IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory;
        private IClientRepository clientRepository;

        public string Create(Client client)
        {
            unitOfWorkIsolatedFactory.With(
                unitOfWork =>
                {
                    client.Id = unitOfWork.ClientRepository.GetNewId();
                    unitOfWork.ClientRepository.Add(client);
                    unitOfWork.Complete();
                }
            );

            unitOfWorkIsolatedFactory.With(
                unitOfWork =>
                {
                    clientRepository.Enlist(unitOfWork);
                    client.Id = clientRepository.GetNewId();
                    clientRepository.Add(client);
                    unitOfWork.Complete();
                }
            );

            return string.Empty;
        }
    }
}
