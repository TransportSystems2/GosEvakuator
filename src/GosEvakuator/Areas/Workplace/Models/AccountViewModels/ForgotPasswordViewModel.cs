using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
