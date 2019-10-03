using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GatewayBelizeOn.Models
{
    /// <summary>
    /// Class for the transaction Object
    /// </summary>
    public class Transaction
    {
        [Required]
        //VEr si se puede poner datatypes
        public string kind { get; set; }
        [Required]
        public string gateway { get; set; }
        [Required]
        public string amount { get; set; }
        [Required]
        public string parent_id { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public string currency { get; set; }
        [Required]
        public string order_id { get; set; }
    }
}