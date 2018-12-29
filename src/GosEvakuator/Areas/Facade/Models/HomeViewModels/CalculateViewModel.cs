using Microsoft.AspNetCore.Mvc;

namespace GosEvakuator.Models.HomeViewModels
{
    public class CalculateViewModel
    {
        [FromForm(Name = "customer-phone-number")]
        public string CustomerPhoneNumber { get; set; }
    }
}