using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;

namespace SalesOrder.Services
{
    public interface IRetailSaleRepository
    {
        bool ContainsWithId(string id);
        RetailSale FindWithId(string id);
        string GetNewId();
        void Add(RetailSale retailSale);
    }
}
