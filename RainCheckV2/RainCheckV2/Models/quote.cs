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
    
    public partial class quote
    {
        public decimal quote_id { get; set; }
        public decimal userid { get; set; }
        public System.DateTime quote_date { get; set; }
        public decimal quote_amount { get; set; }
        [Required]
        [StringLength(10)]
        public string Reference_number { get; set; }
    
        public virtual user_tbl user_tbl { get; set; }
    }
}
