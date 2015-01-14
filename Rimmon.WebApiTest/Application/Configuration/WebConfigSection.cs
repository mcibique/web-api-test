// -----------------------------------------------------------------------
//  <copyright file="WebConfigSection.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public abstract class WebConfigSection
    {
        #region Fields

        private readonly bool _mandatory;
        private readonly string _path;

        private NameValueCollection _section;

        #endregion

        #region Constructors

        protected WebConfigSection(string path, bool mandatory = true)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            this._path = path;
            this._mandatory = mandatory;
        }

        #endregion

        #region Protected Properties

        protected NameValueCollection Section
        {
            get
            {
                if (this._section == null)
                {
                    this._section = ConfigurationManager.GetSection(this._path) as NameValueCollection;
                    if (this._mandatory && this._section == null)
                    {
                        throw new ConfigurationErrorsException("Unable to get configuration section: " + this._path);
                    }
                }

                return this._section;
            }
        }

        #endregion

        #region Protected Methods

        protected T GetValue<T>(string key, T defaultValue = default(T))
        {
            var value = this.Section[key];
            if (value == null)
            {
                return defaultValue;
            }

            var converted = Convert.ChangeType(value, typeof(T));
            if (converted == null)
            {
                return defaultValue;
            }

            return (T)converted;
        }

        #endregion
    }
}