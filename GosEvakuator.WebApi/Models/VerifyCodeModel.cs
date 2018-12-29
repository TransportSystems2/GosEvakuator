using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.WebApi.Models
{
    public class VerifyCodeModel
    {
        public PhoneNumberModel PhoneNumberModel { get; set; }

        public string VerifyCode { get; set; }
    }
}

