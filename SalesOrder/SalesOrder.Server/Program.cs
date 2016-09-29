using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.AutoFac;
using Autofac;
using Topshelf;

using SalesOrder.Actors;

namespace SalesOrder.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator => {
                hostConfigurator.Service<Service>(serviceConfigurator => {
                    serviceConfigurator.ConstructUsing(name => new Service());
                    serviceConfigurator.WhenStarted(service => service.Start());
                    serviceConfigurator.WhenStopped(service => service.Stop());
                });

                hostConfigurator.RunAsLocalSystem();

                hostConfigurator.SetDescription("SalesOrder.Server Host");
                hostConfigurator.SetDisplayName("SalesOrder.Server");
                hostConfigurator.SetServiceName("SalesOrder.Server");
            });
        }
    }
}
