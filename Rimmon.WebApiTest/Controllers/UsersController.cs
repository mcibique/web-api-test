// -----------------------------------------------------------------------
//  <copyright file="UsersController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Rimmon.WebApiTest.Data;
    using Rimmon.WebApiTest.Models;

    [AuthorizeRoles(Roles.Administrators)]
    [RoutePrefix("api/users")]
    public class UsersController : WebApiTestController
    {
        #region Fields

        private readonly IUserManagement _userManagement;

        #endregion

        #region Constructors

        public UsersController(IUserManagement userManagement)
        {
            if (userManagement == null)
            {
                throw new ArgumentNullException("userManagement");
            }
            this._userManagement = userManagement;
        }

        #endregion

        #region Public Methods

        [HttpPatch]
        [Route("{id}/block")]
        public async Task<IHttpActionResult> BlockUser(long id)
        {
            User result = await this._userManagement.BlockUser(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateUser([FromBody] User user)
        {
            if (this.ModelState.IsValid)
            {
                User result = await this._userManagement.CreateUser(user);
                return this.Created("/api/users/" + result.Id, result);
            }

            return this.Errors(this.ModelState);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteUser(long id)
        {
            bool result = await this._userManagement.DeleteUser(id);
            return result ? this.NoContent() : this.NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetUser(long id)
        {
            User user = await this._userManagement.GetUser(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(user);
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetUsers()
        {
            IEnumerable<User> users = await this._userManagement.GetUsers();
            return this.Ok(users);
        }

        [HttpPatch]
        [Route("{id}/unblock")]
        public async Task<IHttpActionResult> UnblockUser(long id)
        {
            User result = await this._userManagement.UnblockUser(id);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> UpdateUser([FromUri] long id, [FromBody] User user)
        {
            if (this.ModelState.IsValid)
            {
                user.Id = id;
                User result = await this._userManagement.UpdateUser(user);
                if (result == null)
                {
                    return this.NotFound();
                }
                return this.Ok(result);
            }

            return this.Errors(this.ModelState);
        }

        #endregion
    }
}