// -----------------------------------------------------------------------
//  <copyright file="AuthorizeRolesAttribute.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        #region Fields

        private readonly string[] _roles;

        #endregion

        #region Constructors

        public AuthorizeRolesAttribute(params string[] roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }

            this._roles = roles;
            this.MatchAll = WebConfig.Security.MatchAllRoles;
        }

        #endregion

        #region Public Properties

        public bool MatchAll { get; set; }

        #endregion

        #region Protected Methods

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }

            if (!this._roles.Any())
            {
                return true;
            }

            IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
            var result = this.MatchAll ? this._roles.All(principal.IsInRole) : this._roles.Any(principal.IsInRole);
            if (!result)
            {
                var logger = new RequestLogger(actionContext.Request);
                if (this.MatchAll)
                {
                    logger.Debug("Access denied. Expected all [" + String.Join(", ", this._roles) + "]");
                }
                else
                {
                    logger.Debug("Access denied. Expected any of [" + String.Join(", ", this._roles) + "]");
                }
            }

            return result;
        }

        #endregion
    }
}