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
        public ClientProcessorService(IUnitOfWorkFactory unitOfWorkFactory, IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory, IClientRepository clientRepository)
        {
            if (unitOfWorkFactory == null) { throw new ArgumentNullException("unitOfWorkFactory"); }
            if (unitOfWorkIsolatedFactory == null) { throw new ArgumentNullException("unitOfWorkIsolatedFactory"); }
            if (clientRepository == null) { throw new ArgumentNullException("clientRepository"); }

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.unitOfWorkIsolatedFactory = unitOfWorkIsolatedFactory;
            this.clientRepository = clientRepository;
        }

        private IUnitOfWorkFactory unitOfWorkFactory;
        private IUnitOfWorkIsolatedFactory unitOfWorkIsolatedFactory;
        private IClientRepository clientRepository;

        public string Create(Client client)
        {
            string newId = string.Empty;

            unitOfWorkIsolatedFactory.With(
                unitOfWork =>
                {
                    clientRepository.Enlist(unitOfWork);

                    newId = clientRepository.GetNewId();
                    client.Id = newId;
                    clientRepository.Add(client);

                    unitOfWork.Complete();

                    clientRepository.Delist();
                }
            );

            using (var unitOfWork = unitOfWorkFactory.Create())
            {
                clientRepository.Enlist(unitOfWork);

                newId = clientRepository.GetNewId();
                client.Id = newId;
                clientRepository.Add(client);

                unitOfWork.Complete();

                clientRepository.Delist();
            }

            return string.Empty;
        }
    }
}
