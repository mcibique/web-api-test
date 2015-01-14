// -----------------------------------------------------------------------
//  <copyright file="IProfileManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System.Threading.Tasks;
    using Rimmon.WebApiTest.Models;

    public interface IProfileManagement
    {
        #region Abstract Members

        Task<Profile> GetProfile(string userName);

        #endregion
    }
}