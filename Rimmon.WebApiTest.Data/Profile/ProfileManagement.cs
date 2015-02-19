// -----------------------------------------------------------------------
//  <copyright file="ProfileManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System.Threading.Tasks;
    using Rimmon.WebApiTest.Models;

    public class ProfileManagement : IProfileManagement
    {
        #region IProfileManagement Members

        public Task<Profile> GetProfile(string userName)
        {
            return Task.FromResult(new Profile { FirstName = "First Name", LastName = "Last Name", Email = userName + "@email.com" });
        }

        #endregion
    }
}