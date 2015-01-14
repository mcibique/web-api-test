// -----------------------------------------------------------------------
//  <copyright file="WebConfig.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    public class WebConfig
    {
        #region Fields

        private static readonly WebConfigSecuritySection _webConfigSecuritySection = new WebConfigSecuritySection();

        #endregion

        #region Public Properties

        public static WebConfigSecuritySection Security
        {
            get
            {
                return _webConfigSecuritySection;
            }
        }

        #endregion
    }
}