// -----------------------------------------------------------------------
//  <copyright file="CreditsController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Controllers
{
    using System.Web.Http;

    [AllowAnonymous]
    [RoutePrefix("api/credits")]
    public class CreditsController : WebApiTestController
    {
        #region Public Methods

        [HttpGet]
        public IHttpActionResult Detail()
        {
            return
                this.Ok(
                    new
                    {
                        version = "1.0",
                        author = "mcibique",
                        sourceCode = "https://github.com/mcibique/web-api-test",
                        gui = "https://github.com/mcibique/angular-test"
                    });
        }

        #endregion
    }
}