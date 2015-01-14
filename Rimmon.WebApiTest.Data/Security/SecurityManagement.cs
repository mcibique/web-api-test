// -----------------------------------------------------------------------
//  <copyright file="SecurityManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System.Threading.Tasks;

    public class SecurityManagement : ISecurityManagement
    {
        #region ISecurityManagement Members

        public Task<bool> ValidateUser(string userName, string password)
        {
            return Task.FromResult(true);
        }

        #endregion
    }
}