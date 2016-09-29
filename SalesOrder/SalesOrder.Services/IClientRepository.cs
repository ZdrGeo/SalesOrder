using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;

namespace SalesOrder.Services
{
    public interface IEnlistableRepository<TUnitOfWork>
    {
        void Enlist(TUnitOfWork unitOfWork);
    }

    /*
    public interface IUnitOfWork
    {
        IClientRepository ClientRepository { get; }
        IRetailSaleRepository RetailSaleRepository { get; }
        void Complete();
    }
    */

    public interface IUnitOfWork
    {
        void Complete();
    }

    public interface IUnitOfWorkIsolatedFactory
    {
        void With(Action<IUnitOfWork> action);
    }

    public interface IClientRepository<TUnitOfWork> : IEnlistableRepository<TUnitOfWork>
    {
        bool ContainsWithId(string id);
        Client FindWithId(string id);
        string GetNewId();
        void Add(Client client);
    }
}
