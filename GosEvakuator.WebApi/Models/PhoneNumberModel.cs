using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.WebApi.Models
{
    public class PhoneNumberModel
    {
        public string CountryCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}