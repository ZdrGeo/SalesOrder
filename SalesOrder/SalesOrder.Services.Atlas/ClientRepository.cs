using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

using DataUtils;
using SalesOrder.Models;
using SalesOrder.Models.Atlas;

namespace DataUtils
{
    public static class Db
    {
        public static T ToValueOrDefault<T>(this object value)
        {
            if (value == DBNull.Value) { return default (T); }
            else { return (T)value; }
        }

        public static T GetValueOrDefault<T>(object value)
        {
            return (value ?? DBNull.Value).ToValueOrDefault<T>();
        }

        public static IDbDataParameter AddParameter(this IDbCommand dbCommand, string name, object value)
        {
            IDbDataParameter dbDataParameter = dbCommand.CreateParameter();

            dbDataParameter.ParameterName = name;
            dbDataParameter.Value = value ?? DBNull.Value;

            dbCommand.Parameters.Add(dbDataParameter);

            return dbDataParameter;
        }
    }
}

namespace SalesOrder.Services.Atlas
{
    public abstract class EnlistableRepository<TUnitOfWork> : IEnlistableRepository where TUnitOfWork : class
    {
        protected TUnitOfWork UnitOfWork { get; private set; }

        protected void EnsureEnlisted()
        {
            if (UnitOfWork == null) { throw new InvalidOperationException("Repository is not enlisted in unit of work"); }
        }

        public void Enlist(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) { throw new ArgumentNullException("unitOfWork", "Repository can not be enlisted in null unit of work"); }

            UnitOfWork = (TUnitOfWork)unitOfWork.Get();
        }

        public void Delist()
        {
            UnitOfWork = null;
        }
    }

    /*
    public abstract class UnitOfWork : IUnitOfWork
    {
        ~UnitOfWork()
        {
            Dispose(false);
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }

                disposed = true;
            }
        }

        public abstract object Get();
        public abstract void Complete();
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

        public object Get()
        {
            return atlas;
        }

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

    public class AdoUnitOfWork : IUnitOfWork
    {
        public AdoUnitOfWork(IDbTransaction dbTransaction)
        {
            if (dbTransaction == null) { throw new ArgumentNullException("dbTransaction"); }

            this.dbTransaction = dbTransaction;
        }

        private IDbTransaction dbTransaction;

        public object Get()
        {
            return dbTransaction;
        }

        public void Complete()
        {
            dbTransaction.Commit();
        }
    }

    public class AdoUnitOfWorkIsolatedFactory : IUnitOfWorkIsolatedFactory
    {
        public AdoUnitOfWorkIsolatedFactory(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        private IDbConnection dbConnection;

        public void With(Action<IUnitOfWork> action)
        {
            using (IDbTransaction dbTransaction = dbConnection.BeginTransaction())
            {
                AdoUnitOfWork adoUnitOfWork = new AdoUnitOfWork(dbTransaction);

                action(adoUnitOfWork);
            }
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

    public class ClientAdoRepository : EnlistableRepository<IDbTransaction>, IClientRepository
    {
        public bool ContainsWithId(string id)
        {
            EnsureEnlisted();

            bool contains = false;

            short firmId = 1;
            int clientId = int.Parse(id);

            using (IDbCommand dbCommand = UnitOfWork.Connection.CreateCommand())
            {
                dbCommand.CommandText = @"
                    SELECT
                        0
                    FROM A_Clients
                    WHERE FirmID = @FirmID
                        AND ClientID = @ClientID
                ";

                dbCommand.AddParameter("@FirmID", firmId);
                dbCommand.AddParameter("@ClientID", clientId);

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read()) { contains = true; }
                }
            }

            return contains;
        }

        public Client FindWithId(string id)
        {
            EnsureEnlisted();

            Client client = new Client();

            short firmId = 1;
            int clientId = int.Parse(id);

            using (IDbCommand dbCommand = UnitOfWork.Connection.CreateCommand())
            {
                dbCommand.CommandText = @"
                    SELECT
                        *
                    FROM A_Clients
                    WHERE FirmID = @FirmID
                        AND ClientID = @ClientID
                ";

                dbCommand.AddParameter("@FirmID", firmId);
                dbCommand.AddParameter("@ClientID", clientId);

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        // MapClient(dataReader, client);
                    }
                }
            }

            return client;
        }

        public string GetNewId()
        {
            EnsureEnlisted();

            short firmId = 1;
            int clientId = 1;

            using (IDbCommand dbCommand = UnitOfWork.Connection.CreateCommand())
            {
                dbCommand.CommandText = @"
                    SELECT
                        MAX(ClientID) AS ClientID
                    FROM A_Clients WITH (TABLOCKX)
                    WHERE FirmID = @FirmID
                ";

                dbCommand.AddParameter("@FirmID", firmId);

                using (IDataReader dataReader = dbCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        clientId = Db.GetValueOrDefault<int>(dataReader["ClientID"]);
                    }
                }
            }

            return clientId.ToString();
        }

        public void Add(Client client)
        {
            EnsureEnlisted();

            short firmId = 1;
            int clientId = int.Parse(client.Id);

            using (IDbCommand dbCommand = UnitOfWork.Connection.CreateCommand())
            {
                dbCommand.CommandText = @"
                    INSERT INTO A_Clients (
                        FirmID, ClientID, ClientName
                    ) VALUES (
                        @FirmID, @ClientID, @ClientName
                    )
                ";

                dbCommand.AddParameter("@FirmID", firmId);
                dbCommand.AddParameter("@FirmID", clientId);
                dbCommand.AddParameter("@FirmID", client.Name);

                dbCommand.ExecuteNonQuery();
            }
        }
    }
}
