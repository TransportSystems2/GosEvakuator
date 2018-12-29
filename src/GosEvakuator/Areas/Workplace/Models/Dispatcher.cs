using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models
{
    public class Dispatcher : Member
    {
        public virtual ICollection<Garage> Garages { get; set; }
    }
}