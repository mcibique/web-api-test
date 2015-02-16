// -----------------------------------------------------------------------
//  <copyright file="WebConfig.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System.Configuration;
    using System.Web.Configuration;

    public class WebConfig
    {
        #region Fields

        private static readonly WebConfigSecuritySection _webConfigSecuritySection = new WebConfigSecuritySection();
        private static CompilationSection _compilationSection;

        #endregion

        #region Public Properties

        public static CompilationSection Compilation
        {
            get
            {
                return _compilationSection ?? (_compilationSection = ConfigurationManager.GetSection("system.web/compilation") as CompilationSection);
            }
        }

        public static bool IsDebug
        {
            get
            {
                return Compilation.Debug;
            }
        }

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