// -----------------------------------------------------------------------
//  <copyright file="RequestLoggerEntry.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using NLog;

    public class RequestLoggerEntry
    {
        #region Public Properties

        public object[] Args { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int ThreadId { get; set; }

        #endregion
    }
}