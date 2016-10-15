using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;

namespace SalesOrder.Services
{
    public interface IUnitOfWork
    {
        object Get();
        void Complete();
    }

    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }

    public interface IUnitOfWorkIsolatedFactory
    {
        void With(Action<IUnitOfWork> action);
    }

    public interface IEnlistableRepository
    {
        void Enlist(IUnitOfWork unitOfWork);
        void Delist();
    }

    public interface IClientRepository : IEnlistableRepository
    {
        bool ContainsWithId(string id);
        Client FindWithId(string id);
        string GetNewId();
        void Add(Client client);
    }
}
