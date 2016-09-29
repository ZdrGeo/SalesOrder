using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Owin;

using Autofac;

namespace SalesOrder.Server.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            /*
            ContainerBuilder containerBuilder = new ContainerBuilder();

            // containerBuilder.RegisterType<SalesOrderService>().As<ISalesOrderService>();

            IContainer container = containerBuilder.Build();

            appBuilder.UseAutofacMiddleware(container);
            */

            HttpConfiguration httpConfiguration = new HttpConfiguration();

            httpConfiguration.Routes.MapHttpRoute(name: "Api", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}
