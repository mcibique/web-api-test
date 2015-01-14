// -----------------------------------------------------------------------
//  <copyright file="ISecurityManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System.Threading.Tasks;

    public interface ISecurityManagement
    {
        #region Abstract Members

        Task<bool> ValidateUser(string userName, string password);

        #endregion
    }
}