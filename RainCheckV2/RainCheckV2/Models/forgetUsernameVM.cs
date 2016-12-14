using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RainCheckV2.Models
{
    public class forgetUsernameVM
    {
        [Required]
        [RegularExpression("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])+$")]
        public string email { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal policy_Num { get; set; }

    }
}