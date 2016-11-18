using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Owin;

// using Autofac;
using System.Reflection;
// using Autofac.Integration.WebApi;

namespace SalesOrder.Cloud.Server.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();

            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute(name: "Api", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            // httpConfiguration.Formatters.Remove(httpConfiguration.Formatters.XmlFormatter);
            // httpConfiguration.Formatters.Add(httpConfiguration.Formatters.JsonFormatter);

            // httpConfiguration.EnableSystemDiagnosticsTracing();

            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}
