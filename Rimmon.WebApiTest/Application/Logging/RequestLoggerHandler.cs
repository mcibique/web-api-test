// -----------------------------------------------------------------------
//  <copyright file="RequestLoggerHandler.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Configuration;
    using NLog;

    public class RequestLoggerHandler : DelegatingHandler
    {
        #region Protected Methods

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestContent = request.Content != null ? await request.Content.ReadAsStringAsync() : String.Empty;
            var response = await base.SendAsync(request, cancellationToken);

            var level = LogLevel.Debug;
            var requestLogger = new RequestLogger(request.Properties);
            var entries = requestLogger.GetEntries();
            var messages = new StringBuilder();
            foreach (RequestLoggerEntry entry in entries)
            {
                if (!String.IsNullOrWhiteSpace(entry.StackTrace))
                {
                    messages.AppendFormat("\t---[({1}) {0}]---", entry.StackTrace, entry.ThreadId.ToString("00")).AppendLine();
                }

                messages.Append("\t").AppendFormat(entry.Message, entry.Args).AppendLine().AppendLine();
                if (entry.LogLevel > level)
                {
                    level = entry.LogLevel;
                }
            }

            var logger = LogManager.GetLogger("web");
            if (!logger.IsEnabled(level))
            {
                return response;
            }

            var output = new StringBuilder();
            output.Append(this.GetRequestInfo(request, requestContent));

            var messagesString = messages.ToString();
            if (!String.IsNullOrWhiteSpace(messagesString))
            {
                output.AppendLine("\tMessages:").AppendLine().Append(messagesString);
            }
            else
            {
                output.AppendLine();
            }

            var responseContent = response.Content != null ? await response.Content.ReadAsStringAsync() : String.Empty;
            output.Append(this.GetResponseInfo(response, responseContent));

            logger.Log(level, output.ToString());

            return response;
        }

        protected string CleanupContent(string content)
        {
            return Regex.Replace(content, @"(""password"":"")([^""]*)("")", "$1*****$3");
        }

        protected HttpBrowserCapabilities GetBrowser(string userAgent)
        {
            var browser = new HttpBrowserCapabilities { Capabilities = new Hashtable { { String.Empty, userAgent } } };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);
            return browser;
        }

        protected string GetRequestInfo(HttpRequestMessage request, string requestContent)
        {
            var stringBuilder = new StringBuilder();

            // Request: GET http://localhost:9004/api/login (AJAX)
            IEnumerable<string> values;
            var ajax = request.Headers.TryGetValues("X-Requested-With", out values);
            var method = request.Method;
            var url = request.RequestUri;
            stringBuilder.Append("Request:\t").Append(method).Append(" ").Append(url).AppendIf(ajax, " (AJAX)").AppendLine();

            // Browser: Chrome 40.0 beta for WinNT (Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.93 Safari/537.36)
            var userAgent = request.Headers.UserAgent.ToString();
            var browser = this.GetBrowser(userAgent);

            stringBuilder.Append("\tBrowser:\t")
                .Append(browser.Browser)
                .Append(" ")
                .Append(browser.Version)
                .AppendIf(browser.Beta, " beta")
                .AppendIf(!String.IsNullOrWhiteSpace(browser.Platform), " for " + browser.Platform)
                .AppendLine(" (" + userAgent + ")");

            // Host: localhost:9004, Via: 1.1 pc-rimmon, Referrer: http://localhost:9000/login/
            var host = request.Headers.Host;
            var via = request.Headers.Via != null ? request.Headers.Via.ToString() : String.Empty;
            var referrer = request.Headers.Referrer != null ? request.Headers.Referrer.ToString() : String.Empty;
            stringBuilder.Append("\tHost:\t\t" + host)
                .AppendIf(!String.IsNullOrWhiteSpace(via), ", Via: " + via)
                .AppendIf(!String.IsNullOrWhiteSpace(referrer), ", Referrer: " + referrer)
                .AppendLine();

            // User: admin, Roles: [Administrators, Users]
            var principal = Thread.CurrentPrincipal;
            bool isAuthenticated = false;
            string userName = String.Empty;
            IEnumerable<string> roles = null;
            if (principal != null && principal.Identity != null)
            {
                isAuthenticated = principal.Identity.IsAuthenticated;
                userName = principal.Identity.Name;
                var claimsPrincipal = principal as ClaimsPrincipal;
                if (claimsPrincipal != null)
                {
                    roles = claimsPrincipal.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                }
            }

            if (isAuthenticated)
            {
                stringBuilder.Append("\tUser: \t\t").Append(userName);
                if (roles != null)
                {
                    stringBuilder.Append(", Roles: [").Append(String.Join(", ", roles)).Append("]");
                }
                stringBuilder.AppendLine();
            }

            // Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8, Encoding: gzip, deflate, sdch, Languages: sk-SK,sk;q=0.8,cs;q=0.6,en-US;q=0.4,en;q=0.2, Charset: 
            var accept = request.Headers.Accept.ToString();
            var acceptCharset = request.Headers.AcceptCharset.ToString();
            var acceptEncoding = request.Headers.AcceptEncoding.ToString();
            var acceptLanguage = request.Headers.AcceptLanguage.ToString();

            stringBuilder.Append("\tAccept:\t\t")
                .Append(accept)
                .AppendIf(!String.IsNullOrWhiteSpace(acceptEncoding), ", Encoding: " + acceptEncoding)
                .AppendIf(!String.IsNullOrWhiteSpace(acceptLanguage), ", Language: " + acceptLanguage)
                .AppendIf(!String.IsNullOrWhiteSpace(acceptCharset), ", Charset: " + acceptCharset)
                .AppendLine();

            // Content: application/json; charset=UTF-8, Length: 37
            var contentType = request.Content.Headers.ContentType;
            if (contentType != null)
            {
                var contentEncoding = request.Content.Headers.ContentLength;
                stringBuilder.Append("\tContent:\t").Append(contentType).AppendIf(contentEncoding.HasValue, ", Length: " + contentEncoding).AppendLine();
            }

            // Data: {"userName":"admin","password":"*****"}
            if (method == HttpMethod.Post || method == HttpMethod.Delete || method == HttpMethod.Put)
            {
                stringBuilder.Append("\tData:\t\t").AppendLine(this.CleanupContent(requestContent));
            }

            return stringBuilder.ToString();
        }

        protected string GetResponseInfo(HttpResponseMessage response, string responseContent)
        {
            var stringBuilder = new StringBuilder();

            // Response: 400 Bad Request
            stringBuilder.Append("\tResponse:\t").Append((int)response.StatusCode).Append(" ").AppendLine(response.ReasonPhrase);

            // Content: {"message":"The request is invalid.","errors":{"":["Invalid user name or password."]}}
            stringBuilder.Append("\tContent:\t");
            stringBuilder.Append(this.CleanupContent(responseContent));

            return stringBuilder.ToString();
        }

        #endregion
    }
}