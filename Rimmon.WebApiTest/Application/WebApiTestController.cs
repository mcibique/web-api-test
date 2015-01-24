// -----------------------------------------------------------------------
//  <copyright file="WebApiTestController.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;

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

        #region Protected Methods

        /// <summary>
        /// The same method as this.BadRequest(this.ModelState) except following:
        /// 
        /// - result doesn't use property modelState but errors
        /// - result never includes exception messages
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        protected IHttpActionResult Errors(ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, ModelState> keyValuePair in modelState)
            {
                string key = keyValuePair.Key;
                ModelErrorCollection collection = keyValuePair.Value.Errors;
                if (collection == null || collection.Count <= 0)
                {
                    continue;
                }

                var current = collection.Select(error => error.ErrorMessage).Where(message => !String.IsNullOrWhiteSpace(message)).ToList();
                if (current.Count > 0)
                {
                    errors.Add(key, current);
                }
            }

            return this.Content(HttpStatusCode.BadRequest, new { message = "The request is invalid.", errors });
        }

        #endregion
    }
}