using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;

namespace SalesOrder.Services
{
    public interface IDocumentRepository : IEnlistableRepository
    {
        bool ContainsWithId(string id);
        Document FindWithId(string id);
        string GetNewId();
        void Add(Document document);
    }
}
