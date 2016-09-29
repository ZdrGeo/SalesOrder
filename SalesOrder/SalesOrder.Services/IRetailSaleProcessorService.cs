﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SalesOrder.Models;

namespace SalesOrder.Services
{
    public interface IRetailSaleProcessorService
    {
        string Create(RetailSale retailSale);
    }
}
