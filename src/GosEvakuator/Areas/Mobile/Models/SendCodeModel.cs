using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Areas.Mobile.Models
{
    public class SendCodeModel
    {
        public string CountryCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}