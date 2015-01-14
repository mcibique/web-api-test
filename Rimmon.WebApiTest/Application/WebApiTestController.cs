// -----------------------------------------------------------------------
//  <copyright file="WebApiTestController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Web.Http;

    public abstract class WebApiTestController : ApiController
    {
        #region Protected Properties

        protected string UserName
        {
            get
            {
                if (this.User.Identity != null && this.User.Identity.IsAuthenticated)
                {
                    return this.User.Identity.Name ?? String.Empty;
                }

                return String.Empty;
            }
        }

        #endregion
    }
}