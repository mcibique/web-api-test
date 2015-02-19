// -----------------------------------------------------------------------
//  <copyright file="User.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Models
{
    using System;

    public class User
    {
        #region Public Properties

        public bool Blocked { get; set; }
        public DateTime Created { get; set; }
        public long Id { get; set; }
        public Profile Profile { get; set; }
        public string UserName { get; set; }

        #endregion
    }
}