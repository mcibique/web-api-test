// -----------------------------------------------------------------------
//  <copyright file="SecurityManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SecurityManagement : ISecurityManagement
    {
        #region ISecurityManagement Members

        public Task<IEnumerable<string>> GetRoles(string userName)
        {
            IEnumerable<string> roles = new List<string> { Roles.Administrators, Roles.Users };
            return Task.FromResult(roles);
        }

        public Task<bool> ValidateUser(string userName, string password)
        {
            return Task.FromResult(userName.Equals("admin", StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}