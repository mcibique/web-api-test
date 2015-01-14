// -----------------------------------------------------------------------
//  <copyright file="JwtAuthorizationDelegatingHandler.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.ServiceModel.Security.Tokens;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public class JwtAuthorizationDelegatingHandler : DelegatingHandler
    {
        #region Fields

        private readonly byte[] _symmetricKey = Encoding.UTF8.GetBytes(WebConfig.Security.SymetricKey);

        #endregion

        #region Protected Methods

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string header;
            if (!this.TryGetAuthorizationHeader(request, out header))
            {
                return base.SendAsync(request, cancellationToken);
            }

            string token = Regex.Replace(header, @"^Bearer\ ", "", RegexOptions.IgnoreCase);
            if (String.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = WebConfig.Security.AppliesToAddress,
                ValidIssuer = WebConfig.Security.ValidIssuer,
                IssuerSigningToken = new BinarySecretSecurityToken(_symmetricKey)
            };

            try
            {
                SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                HttpContext.Current.User = Thread.CurrentPrincipal = principal;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            catch (Exception)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }

        #endregion

        #region Private Methods

        private bool TryGetAuthorizationHeader(HttpRequestMessage request, out string value)
        {
            value = String.Empty;
            IEnumerable<string> headers;
            if (!request.Headers.TryGetValues("Authorization", out headers) || headers.Count() > 1)
            {
                return false;
            }

            value = headers.ElementAt(0);
            return true;
        }

        #endregion
    }
}