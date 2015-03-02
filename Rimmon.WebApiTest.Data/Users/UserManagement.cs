// -----------------------------------------------------------------------
//  <copyright file="UserManagement.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Rimmon.WebApiTest.Models;

    public class UserManagement : IUserManagement
    {
        #region Fields

        private static readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                UserName = "admin",
                Created = new DateTime(2015, 1, 1),
                Blocked = false,
                Profile = new Profile { FirstName = "admin", LastName = "admin", Email = "admin@email.com" }
            },
            new User
            {
                Id = 2,
                UserName = "commonuser",
                Created = new DateTime(2015, 7, 2),
                Blocked = false,
                Profile = new Profile { FirstName = "common", LastName = "user", Email = "commonuser@email.com" }
            }
        };

        private static long _lastId = _users.Count;

        #endregion

        #region IUserManagement Members

        public Task<User> BlockUser(long id)
        {
            var entity = _users.FirstOrDefault(u => u.Id == id);
            if (entity == null)
            {
                return Task.FromResult<User>(null);
            }

            entity.Blocked = true;
            return Task.FromResult(entity);
        }

        public Task<User> CreateUser(User user)
        {
            user.Id = ++_lastId;
            user.Created = DateTime.Now;
            user.Blocked = false;

            _users.Add(user);

            return Task.FromResult(user);
        }

        public Task<bool> DeleteUser(long id)
        {
            var entity = _users.FirstOrDefault(u => u.Id == id);
            if (entity == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_users.Remove(entity));
        }

        public Task<User> GetUser(long id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult<IEnumerable<User>>(_users);
        }

        public Task<User> UnblockUser(long id)
        {
            var entity = _users.FirstOrDefault(u => u.Id == id);
            if (entity == null)
            {
                return Task.FromResult<User>(null);
            }

            entity.Blocked = false;
            return Task.FromResult(entity);
        }

        public Task<User> UpdateUser(User user)
        {
            var entity = _users.FirstOrDefault(u => u.Id == user.Id);
            if (entity == null)
            {
                return Task.FromResult<User>(null);
            }

            entity.Profile.FirstName = user.Profile.FirstName;
            entity.Profile.LastName = user.Profile.LastName;
            entity.Profile.Email = user.Profile.Email;
            return Task.FromResult(entity);
        }

        #endregion
    }
}