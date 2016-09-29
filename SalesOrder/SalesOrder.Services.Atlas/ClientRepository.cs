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
    /*
    public class AtlasUnitOfWork : IUnitOfWork
    {
        public AtlasUnitOfWork(Models.Atlas.Atlas atlas)
        {
            if (atlas == null) { throw new ArgumentNullException("atlas"); }

            ClientRepository = new ClientRepository(atlas);
            RetailSaleRepository = new RetailSaleRepository(atlas);

            this.atlas = atlas;
        }

        private Models.Atlas.Atlas atlas;

        public IClientRepository ClientRepository { get; }
        public IRetailSaleRepository RetailSaleRepository { get; }

        public void Complete()
        {
            atlas.SaveChanges();
            atlas.Database.CurrentTransaction.Commit();
        }
    }
    */

    public class AtlasUnitOfWork : IUnitOfWork
    {
        public AtlasUnitOfWork(Models.Atlas.Atlas atlas)
        {
            if (atlas == null) { throw new ArgumentNullException("atlas"); }

            this.atlas = atlas;
        }

        private Models.Atlas.Atlas atlas;

        public void Complete()
        {
            atlas.SaveChanges();
            atlas.Database.CurrentTransaction.Commit();
        }
    }

    public class AtlasUnitOfWorkIsolatedFactory : IUnitOfWorkIsolatedFactory
    {
        public void With(Action<IUnitOfWork> action)
        {
            using (Models.Atlas.Atlas atlas = new Models.Atlas.Atlas())
            {
                using (DbContextTransaction dbContextTransaction = atlas.Database.BeginTransaction())
                {
                    AtlasUnitOfWork atlasUnitOfWork = new AtlasUnitOfWork(atlas);

                    action(atlasUnitOfWork);
                }
            }
        }
    }

    public interface IEnlistableRepository<TUnitOfWork>
    {
        void Enlist(TUnitOfWork unitOfWork);
        void Delist();
    }

    public abstract class EnlistableRepository<TUnitOfWork> : IEnlistableRepository<TUnitOfWork> where TUnitOfWork : class
    {
        protected TUnitOfWork UnitOfWork { get; private set; }

        protected void EnsureEnlisted()
        {
            if (UnitOfWork == null)
            {
                throw new InvalidOperationException("Repository is not enlisted in unit of work");
            }
        }

        public void Enlist(TUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork", "Repository can not be enlisted in null unit of work");
            }

            UnitOfWork = unitOfWork;
        }

        public void Delist()
        {
            UnitOfWork = null;
        }
    }

    public class ClientRepository : EnlistableRepository<Models.Atlas.Atlas>, IClientRepository
    {
        public bool ContainsWithId(string id)
        {
            EnsureEnlisted();

            short firmId = 1;
            int clientId = int.Parse(id);

            return UnitOfWork.A_Clients.Any(a_c => a_c.FirmID == firmId && a_c.ClientID == clientId);
        }

        public Client FindWithId(string id)
        {
            EnsureEnlisted();

            short firmId = 1;
            int clientId = int.Parse(id);

            A_Clients a_clients = UnitOfWork.A_Clients.First(a_c => a_c.FirmID == firmId && a_c.ClientID == clientId);

            Client client = new Client();

            return client;
        }

        public string GetNewId()
        {
            EnsureEnlisted();

            short firmId = 1;

            /* If we want to lock the table exclusively, which is the right way.
            string sql = @"
                SELECT
                    MAX(ClientID) AS ClientID
                FROM A_Clients WITH (TABLOCKX)
                WHERE FirmID = @FirmID
            ";

            int clientId = UnitOfWork.Database.SqlQuery<int>(sql, firmId).FirstOrDefault() + 1;
            */

            int clientId = UnitOfWork.A_Clients.Where(a_c => a_c.FirmID == firmId).Max(a_c => (int?)a_c.ClientID).GetValueOrDefault() + 1;

            return clientId.ToString();
        }

        public void Add(Client client)
        {
            EnsureEnlisted();

            int clientId = int.Parse(client.Id);

            A_Clients a_clients = new A_Clients();

            a_clients.FirmID = 1;
            a_clients.ClientID = clientId;
            a_clients.ClientName = client.Name;

            UnitOfWork.A_Clients.Add(a_clients);
        }
    }
}
