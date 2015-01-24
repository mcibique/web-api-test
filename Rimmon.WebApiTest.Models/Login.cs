// -----------------------------------------------------------------------
//  <copyright file="Login.cs" author="Rimmon">
//      Copyright (c) Rimmon All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Rimmon.WebApiTest.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        #region Public Properties

        [Required(ErrorMessage = "The field is required.")]
        [MaxLength(128)]
        public string Password { get; set; }

        [Required(ErrorMessage = "The field is required.")]
        [MaxLength(128)]
        public string UserName { get; set; }

        #endregion
    }
}