// -----------------------------------------------------------------------
//  <copyright file="StringBuilderExtensions.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest
{
    using System.Text;

    public static class StringBuilderExtensions
    {
        #region Public Methods

        public static StringBuilder AppendFormatIf(this StringBuilder sb, bool condition, string value, params object[] args)
        {
            return condition ? sb.AppendFormat(value, args) : sb;
        }

        public static StringBuilder AppendIf(this StringBuilder sb, bool condition, string value)
        {
            return condition ? sb.Append(value) : sb;
        }

        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition, string value)
        {
            return condition ? sb.AppendLine(value) : sb;
        }

        public static StringBuilder AppendLineIf(this StringBuilder sb, bool condition)
        {
            return condition ? sb.AppendLine() : sb;
        }

        #endregion
    }
}