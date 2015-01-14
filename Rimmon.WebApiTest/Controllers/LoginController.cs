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
    using Rimmon.WebApiTest.Data;
    using Rimmon.WebApiTest.Models;

    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        #region Fields

        private readonly ISecurityManagement _securityManagement;

        #endregion

        #region Constructors

        public LoginController(ISecurityManagement securityManagement)
        {
            if (securityManagement == null)
            {
                throw new ArgumentNullException("securityManagement");
            }

            this._securityManagement = securityManagement;
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
                    return this.Ok(new { valid = true, token = "JWT token" });
                }

                this.ModelState.AddModelError("", "Invalid user name or password.");
            }

            return this.BadRequest(this.ModelState);
        }

        #endregion
    }
}