// -----------------------------------------------------------------------
//  <copyright file="WebApiTestApplication.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System.Web;
    using System.Web.Http;
    using StructureMap;

    public class WebApiTestApplication : HttpApplication
    {
        #region Protected Methods

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                this.ConfigureDependencies(config);
                this.ConfigureRoutes(config);
            });
        }

        #endregion

        #region Private Methods

        private void ConfigureDependencies(HttpConfiguration config)
        {
            IContainer container = new Container();
            container.Configure(registry => { });
            config.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }

        #endregion
    }
}