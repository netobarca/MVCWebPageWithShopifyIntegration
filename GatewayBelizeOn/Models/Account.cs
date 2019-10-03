using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GatewayBelizeOn.Models
{
    /// <summary>
    /// Class for the Account of a user
    /// </summary>
    public class Account
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}