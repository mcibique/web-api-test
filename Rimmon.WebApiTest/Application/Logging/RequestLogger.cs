// -----------------------------------------------------------------------
//  <copyright file="RequestLogger.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using NLog;

    public class RequestLogger
    {
        #region Constants

        private const string PropertiesKey = "__RequestLogger__";

        #endregion

        #region Fields

        private readonly string _currentType = typeof(RequestLogger).FullName;
        private readonly IDictionary<string, object> _properties;
        private IList<RequestLoggerEntry> _entries;

        #endregion

        #region Constructors

        public RequestLogger(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            this._properties = properties;
        }

        public RequestLogger(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            this._properties = request.Properties;
        }

        #endregion

        #region Public Methods

        public void Debug(string message, params object[] args)
        {
            this.Log(LogLevel.Debug, message, args);
        }

        public void Error(string message, params object[] args)
        {
            this.Log(LogLevel.Error, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            this.Log(LogLevel.Fatal, message, args);
        }

        public IList<RequestLoggerEntry> GetEntries()
        {
            if (this._entries == null)
            {
                if (this._properties.ContainsKey(PropertiesKey))
                {
                    this._entries = this._properties[PropertiesKey] as IList<RequestLoggerEntry>;
                }
                else
                {
                    this._entries = new List<RequestLoggerEntry>();
                    this._properties[PropertiesKey] = this._entries;
                }
            }

            return this._entries;
        }

        public void Info(string message, params object[] args)
        {
            this.Log(LogLevel.Info, message, args);
        }

        public void Log(LogLevel logLevel, string message, params object[] args)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames();
            string methodInfo = "";

            if (frames != null)
            {
                foreach (StackFrame frame in frames)
                {
                    var method = frame.GetMethod();
                    if (method == null || method.DeclaringType == null)
                    {
                        continue;
                    }
                    var declaringType = method.DeclaringType.FullName;
                    if (declaringType == this._currentType)
                    {
                        continue;
                    }

                    var line = frame.GetFileLineNumber();
                    methodInfo = declaringType + "." + method.Name + " at " + line;
                    break;
                }
            }

            this.GetEntries().Add(new RequestLoggerEntry { LogLevel = logLevel, Message = message, Args = args, StackTrace = methodInfo, ThreadId = threadId });
        }

        public void Trace(string message, params object[] args)
        {
            this.Log(LogLevel.Trace, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.Log(LogLevel.Warn, message, args);
        }

        #endregion
    }
}