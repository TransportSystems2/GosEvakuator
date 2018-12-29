using System.ComponentModel.DataAnnotations;

namespace GosEvakuator.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Phone]
        public string Email { get; set; }
    }
}
