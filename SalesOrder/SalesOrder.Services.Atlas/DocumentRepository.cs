using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;
using SalesOrder.Models.Atlas;

namespace SalesOrder.Services.Atlas
{
    public class DocumentRepository : EnlistableRepository<Models.Atlas.Atlas>, IDocumentRepository
    {
        public bool ContainsWithId(string id)
        {
            EnsureEnlisted();

            short firmId = 1;
            short branchId = 1;
            int docId = int.Parse(id);

            return UnitOfWork.ST_CDocs.Any(st_cd => st_cd.FirmID == firmId && st_cd.BranchID == branchId && st_cd.DocID == docId);
        }

        public Document FindWithId(string id)
        {
            EnsureEnlisted();

            short firmId = 1;
            short branchId = 1;
            int docId = int.Parse(id);

            ST_CDocs ST_CDocs = UnitOfWork.ST_CDocs.First(st_cd => st_cd.FirmID == firmId && st_cd.BranchID == branchId && st_cd.DocID == docId);

            var document = new Document();

            return document;
        }

        public string GetNewId()
        {
            EnsureEnlisted();

            short firmId = 1;
            short branchId = 1;

            /* If we want to lock the table exclusively, which is the right way.
            string sql = @"
                SELECT
                    MAX(DocID) AS DocID
                FROM ST_CDocs WITH (TABLOCKX)
                WHERE FirmID = @FirmID
                    AND BranchID = @BranchID
            ";

            int docId = atlas.Database.SqlQuery<int>(sql, firmId, branchId).FirstOrDefault() + 1;
            */

            int docId = UnitOfWork.ST_CDocs.Where(st_cd => st_cd.FirmID == firmId && st_cd.BranchID == branchId).Max(st_cd => (int?)st_cd.DocID).GetValueOrDefault() + 1;

            return docId.ToString();
        }

        public void Add(Document document)
        {
            EnsureEnlisted();

            int docId = int.Parse(document.Id);

            var st_CDocs = new ST_CDocs();

            st_CDocs.FirmID = 1;
            st_CDocs.BranchID = 1;
            st_CDocs.DocID = docId;
            st_CDocs.V3_S1 = document.Number;

            UnitOfWork.ST_CDocs.Add(st_CDocs);
        }
    }
}
