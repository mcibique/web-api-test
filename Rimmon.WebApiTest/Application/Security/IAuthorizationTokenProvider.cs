// -----------------------------------------------------------------------
//  <copyright file="IAuthorizationTokenProvider.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Security
{
    using System.Collections.Generic;

    public interface IAuthorizationTokenProvider
    {
        #region Abstract Members

        string CreateTokenFor(string userName, IEnumerable<string> roles);

        #endregion
    }
}