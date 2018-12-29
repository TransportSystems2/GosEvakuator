using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Areas.Mobile.Models
{
    public class LocationModel
    {
        public string ID { get; set; }

        public int DriverID { get; set; }

        public float Altitude { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public float Accuracy { get; set; }
    }
}
