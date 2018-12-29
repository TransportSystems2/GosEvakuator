using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GosEvakuator.Models
{
    public class Driver : Member
    {
        public int? GarageID { get; set; }

        public virtual Garage Garage { get; set; }

        public bool IsAccept { get; set; }

        public string GarageName
        {
            get
            {
                return GetGarageName();
            }
        }

        public string CityName
        {
            get
            {
                return GetCityName();
            }
        }

        private string GetCityName()
        {
            var result = "NoSet";

            if ((Garage != null) && (Garage.City != null))
            {
                result = Garage.City.Name;
            }

            return result;
        }

        private string GetGarageName()
        {
            var result = "NoSet";
            
            if (Garage != null)
            {
                result = Garage.Name;
            }
            
            return result;
        }
    }
}