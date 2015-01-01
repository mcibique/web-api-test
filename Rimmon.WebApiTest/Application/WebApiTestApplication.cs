// -----------------------------------------------------------------------
//  <copyright file="WebApiTestApplication.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System.Web;
    using System.Web.Http;

    public class WebApiTestApplication : HttpApplication
    {
        #region Protected Methods

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            });
        }

        #endregion
    }
}