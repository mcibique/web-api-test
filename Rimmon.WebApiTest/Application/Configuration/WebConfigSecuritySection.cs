// -----------------------------------------------------------------------
//  <copyright file="WebConfigSecuritySection.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;

    public class WebConfigSecuritySection : WebConfigSection
    {
        #region Constructors

        public WebConfigSecuritySection()
            : base("webApiTest/security")
        {
        }

        #endregion

        #region Public Properties

        public string AppliesToAddress
        {
            get
            {
                return this.GetValue("AppliesToAddress", String.Empty);
            }
        }

        public string DigestAlgorithm
        {
            get
            {
                return this.GetValue("DigestAlgorithm", String.Empty);
            }
        }

        public bool MatchAllRoles
        {
            get
            {
                return this.GetValue("MatchAllRoles", false);
            }
        }

        public string SignatureAlgorithm
        {
            get
            {
                return this.GetValue("SignatureAlgorithm", String.Empty);
            }
        }

        public string SymetricKey
        {
            get
            {
                return this.GetValue("SymetricKey", String.Empty);
            }
        }

        public string ValidIssuer
        {
            get
            {
                return this.GetValue("ValidIssuer", String.Empty);
            }
        }

        #endregion
    }
}