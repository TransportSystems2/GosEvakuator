using System.ComponentModel.DataAnnotations;

namespace GosEvakuator.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            Shedule = new Shedule();
        }

        public int ID { get; set; }

        public int GarageID { get; set; }

        public int SheduleID { get; set; }

        public int MembershipID { get; set; }

        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        public bool IsAccept { get; set; }

        public virtual Shedule Shedule { get; set; }

        public virtual Garage Garage { get; set; }

        public virtual Membership Memebership { get; set; }
        
        [Display(Name = "Гараж")]
        public string GarageName
        {
            get
            {
                return GetGarageName();
            }
        }

        [Display(Name = "Город")]
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