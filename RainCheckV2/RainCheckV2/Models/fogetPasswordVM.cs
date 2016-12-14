using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace RainCheckV2.Models
{
    public class fogetPasswordVM
    {
        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal policy_Num { get; set; }
    }
}