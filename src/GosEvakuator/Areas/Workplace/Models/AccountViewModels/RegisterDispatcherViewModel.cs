using GosEvakuator.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models.AccountViewModels
{
    public class RegisterDispatcherViewModel : RegisterViewModel
    {
        public int CityID { get; set; }
    }
}
