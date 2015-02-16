// -----------------------------------------------------------------------
//  <copyright file="FailController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Controllers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;

    [DebugOnly]
    [RoutePrefix("api/debug")]
    [AllowAnonymous]
    public class FailController : WebApiTestController
    {
        #region Public Methods

        [HttpGet]
        [Route("echo/{statusCode}")]
        public IHttpActionResult Echo([FromUri] int statusCode, [FromUri] string message = "")
        {
            HttpStatusCode parsedCode;
            if (!Enum.TryParse(statusCode.ToString(CultureInfo.InvariantCulture), out parsedCode))
            {
                parsedCode = HttpStatusCode.OK;
            }

            if ((int)parsedCode >= 400)
            {
                return this.ResponseMessage(this.Request.CreateErrorResponse(parsedCode, message));
            }

            return this.ResponseMessage(this.Request.CreateResponse(parsedCode, message));
        }

        [HttpGet]
        [Route("throw")]
        public IHttpActionResult Throw([FromUri] string message = "")
        {
            throw new HttpException(message);
        }

        #endregion
    }
}