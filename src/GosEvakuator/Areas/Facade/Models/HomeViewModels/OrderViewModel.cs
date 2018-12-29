using Microsoft.AspNetCore.Mvc;

namespace GosEvakuator.Models.HomeViewModels
{
    public class OrderViewModel
    {
        public Pricelist Pricelist { get; set; }

        [FromForm(Name = "customer-phone-number")]
        public string CustomerPhoneNumber { get; set; }
    }
}