// -----------------------------------------------------------------------
//  <copyright file="IUserManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Rimmon.WebApiTest.Models;

    public interface IUserManagement
    {
        #region Abstract Members

        Task<User> BlockUser(long id);
        Task<User> CreateUser(User user);
        Task<bool> DeleteUser(long id);
        Task<User> GetUser(long id);
        Task<IEnumerable<User>> GetUsers();
        Task<User> UnblockUser(long id);
        Task<User> UpdateUser(User user);

        #endregion
    }
}