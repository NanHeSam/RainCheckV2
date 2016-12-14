//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace RainCheckV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class login
    {
        public decimal login_id { get; set; }
        public decimal customer_id { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z]{1,1}[A-Za-z0-9]{0,10}$")]
        [StringLength(20, MinimumLength = 3)]
        public string user_name { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string password { get; set; }

        public virtual customer_tbl customer_tbl { get; set; }
    }
}
