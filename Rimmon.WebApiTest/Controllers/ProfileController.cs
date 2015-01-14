// -----------------------------------------------------------------------
//  <copyright file="ProfileController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Rimmon.WebApiTest.Data;

    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        #region Fields

        private readonly IProfileManagement _profileManagement;

        #endregion

        #region Constructors

        public ProfileController(IProfileManagement profileManagement)
        {
            if (profileManagement == null)
            {
                throw new ArgumentNullException("profileManagement");
            }

            this._profileManagement = profileManagement;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetProfile()
        {
            return this.Ok(await this._profileManagement.GetProfile("currentUser"));
        }

        #endregion
    }
}