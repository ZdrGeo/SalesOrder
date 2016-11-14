using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Owin;

using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;

namespace SalesOrder.Server.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // containerBuilder.RegisterType<SalesOrderService>().As<ISalesOrderService>();

            IContainer container = containerBuilder.Build();

            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(name: "Api", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            // httpConfiguration.Formatters.Remove(httpConfiguration.Formatters.XmlFormatter);
            // httpConfiguration.Formatters.Add(httpConfiguration.Formatters.JsonFormatter);

            // httpConfiguration.EnableSystemDiagnosticsTracing();

            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseAutofacMiddleware(container);

            appBuilder.UseAutofacWebApi(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}
