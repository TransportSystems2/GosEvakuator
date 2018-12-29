using System.ComponentModel.DataAnnotations;

namespace GosEvakuator.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
