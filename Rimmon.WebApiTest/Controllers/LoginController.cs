// -----------------------------------------------------------------------
//  <copyright file="LoginController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Rimmon.WebApiTest.Security;
    using Rimmon.WebApiTest.Data;
    using Rimmon.WebApiTest.Models;

    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : WebApiTestController
    {
        #region Fields

        private readonly ISecurityManagement _securityManagement;
        private readonly IAuthorizationTokenProvider _authorizationTokenProvider;

        #endregion

        #region Constructors

        public LoginController(ISecurityManagement securityManagement, IAuthorizationTokenProvider authorizationTokenProvider)
        {
            if (securityManagement == null)
            {
                throw new ArgumentNullException("securityManagement");
            }
            if (authorizationTokenProvider == null)
            {
                throw new ArgumentNullException("authorizationTokenProvider");
            }

            this._securityManagement = securityManagement;
            this._authorizationTokenProvider = authorizationTokenProvider;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<IHttpActionResult> Validate([FromBody] Login login)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this._securityManagement.ValidateUser(login.UserName, login.Password);
                if (result)
                {
                    var roles = await this._securityManagement.GetRoles(login.UserName);
                    var token = _authorizationTokenProvider.CreateTokenFor(login.UserName, roles);

                    return this.Ok(new { valid = true, token });
                }

                this.ModelState.AddModelError("", "Invalid user name or password.");
            }

            return this.BadRequest(this.ModelState);
        }

        #endregion
    }
}