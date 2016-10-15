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
using Topshelf.Autofac;

using SalesOrder.Actors;

namespace SalesOrder.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            // containerBuilder.RegisterType<Dependency>().As<IDependency>();

            containerBuilder.RegisterType<Service>();

            IContainer container = containerBuilder.Build();

            HostFactory.Run(hostConfigurator => {
                hostConfigurator
                .UseAutofacContainer(container)
                .Service<Service>(serviceConfigurator => {
                    serviceConfigurator
                    .ConstructUsingAutofacContainer()
                    .WhenStarted(service => service.Start())
                    .WhenStopped(service => service.Stop());
                })
                .RunAsLocalSystem();

                hostConfigurator.SetDescription("SalesOrder.Server Host");
                hostConfigurator.SetDisplayName("SalesOrder.Server");
                hostConfigurator.SetServiceName("SalesOrder.Server");
            });
        }
    }
}
