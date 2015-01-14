// -----------------------------------------------------------------------
//  <copyright file="JwtAuthorizationTokenProvider.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class JwtAuthorizationTokenProvider : IAuthorizationTokenProvider
    {
        #region Fields

        private readonly byte[] _symmetricKey = Encoding.UTF8.GetBytes(WebConfig.Security.SymetricKey);

        #endregion

        #region IAuthorizationTokenProvider Members

        public string CreateTokenFor(string userName, IEnumerable<string> roles)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }

            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, userName));

            var signingCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(this._symmetricKey), WebConfig.Security.SignatureAlgorithm,
                WebConfig.Security.DigestAlgorithm);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                AppliesToAddress = WebConfig.Security.AppliesToAddress,
                TokenIssuerName = WebConfig.Security.ValidIssuer,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        #endregion
    }
}