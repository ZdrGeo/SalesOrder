using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Microsoft.Owin.Hosting;

namespace SalesOrder.Server.Api
{
    public class Service
    {
        IDisposable disposable;

        public void Start()
        {
            SalesOrderActorSystem.Start();

            disposable = WebApp.Start(ConfigurationManager.AppSettings["ApiUrl"]);
        }

        public void Stop()
        {
            disposable.Dispose();

            SalesOrderActorSystem.Stop();
        }
    }
}
