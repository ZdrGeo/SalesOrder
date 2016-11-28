using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Autofac;

namespace SalesOrder.Agent
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

                hostConfigurator.SetDescription("SalesOrder.Agent Host");
                hostConfigurator.SetDisplayName("SalesOrder.Agent");
                hostConfigurator.SetServiceName("SalesOrder.Agent");
            });
        }
    }
}
