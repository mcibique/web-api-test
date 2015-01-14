// -----------------------------------------------------------------------
//  <copyright file="WebApiTestBodyModelValidator.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.Metadata;
    using System.Web.Http.Validation;

    public class WebApiTestBodyModelValidator : IBodyModelValidator
    {
        #region Fields

        private readonly IBodyModelValidator _innerValidator;

        #endregion

        #region Constructors

        public WebApiTestBodyModelValidator(IBodyModelValidator innerValidator)
        {
            if (innerValidator == null)
            {
                throw new ArgumentNullException("innerValidator");
            }

            this._innerValidator = innerValidator;
        }

        #endregion

        #region IBodyModelValidator Members

        public bool Validate(object model, Type type, ModelMetadataProvider metadataProvider, HttpActionContext actionContext, string keyPrefix)
        {
            return this._innerValidator.Validate(model, type, metadataProvider, actionContext, String.Empty);
        }

        #endregion
    }
}