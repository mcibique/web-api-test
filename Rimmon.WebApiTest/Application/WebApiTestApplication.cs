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
    using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
    using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
    using Newtonsoft.Json.Serialization;
    using NLog;
    using Rimmon.WebApiTest.Data;
    using Rimmon.WebApiTest.Security;
    using StructureMap;

    public class WebApiTestApplication : HttpApplication
    {
        #region Fields

        protected static readonly Logger Logger = LogManager.GetLogger("web");

        #endregion

        #region Protected Methods

        protected void Application_Start()
        {
            Logger.Debug("Application pre-starting.");

            GlobalConfiguration.Configure(config =>
            {
                this.ConfigureDependencies(config);
                this.ConfigureFormatters(config);
                this.ConfigureRoutes(config);
                this.ConfigureModelValidators(config);
                this.ConfigureFilters(config);
                this.ConfigureAuthentication(config);
                this.ConfigureRequestLogging(config);
                this.ConfigureCompression(config);

                Logger.Debug("Application configured successfully.");
            });
        }

        #endregion

        #region Private Methods

        private void ConfigureAuthentication(HttpConfiguration config)
        {
            config.MessageHandlers.Insert(0, new JwtAuthorizationDelegatingHandler());
        }

        private void ConfigureCompression(HttpConfiguration config)
        {
            // https://github.com/azzlack/Microsoft.AspNet.WebApi.MessageHandlers.Compression
            config.MessageHandlers.Insert(0, new ServerCompressionHandler(256, new GZipCompressor(), new DeflateCompressor()));
        }

        private void ConfigureDependencies(HttpConfiguration config)
        {
            IContainer container = new Container();
            container.Configure(registry =>
            {
                registry.For<IProfileManagement>().Use<ProfileManagement>();
                registry.For<ISecurityManagement>().Use<SecurityManagement>();
                registry.For<IAuthorizationTokenProvider>().Use<JwtAuthorizationTokenProvider>();
            });
            config.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        private void ConfigureFilters(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
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

        private void ConfigureRequestLogging(HttpConfiguration config)
        {
            config.MessageHandlers.Insert(0, new RequestLoggerHandler());
        }

        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }

        #endregion
    }
}