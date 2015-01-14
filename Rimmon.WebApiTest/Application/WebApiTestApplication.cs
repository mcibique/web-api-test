// -----------------------------------------------------------------------
//  <copyright file="WebApiTestApplication.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Validation;
    using Newtonsoft.Json.Serialization;
    using Rimmon.WebApiTest.Data;
    using StructureMap;

    public class WebApiTestApplication : HttpApplication
    {
        #region Protected Methods

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                this.ConfigureDependencies(config);
                this.ConfigureFormatters(config);
                this.ConfigureRoutes(config);
                this.ConfigureModelValidators(config);
            });
        }

        #endregion

        #region Private Methods

        private void ConfigureDependencies(HttpConfiguration config)
        {
            IContainer container = new Container();
            container.Configure(registry =>
            {
                registry.For<IProfileManagement>().Use<ProfileManagement>();
                registry.For<ISecurityManagement>().Use<SecurityManagement>();
            });
            config.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        private void ConfigureFormatters(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
        }

        private void ConfigureModelValidators(HttpConfiguration config)
        {
            var services = config.Services;
            var current = services.GetBodyModelValidator();
            services.Replace(typeof(IBodyModelValidator), new WebApiTestBodyModelValidator(current));
        }

        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }

        #endregion
    }
}