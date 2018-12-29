using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GosEvakuator.Models
{
    public class Garage
    {
        public Garage()
        {
            SetDefaultValues();
        }

        public int ID { get; set; }

        public string Name { get; set; }
        
        public bool IsMaster { get; set; }

        public double PhoneNumber { get; set; }

        public string PhoneNumberMask { get; set; }

        public int CityID { get; set; }

        public int? PricelistID { get; set; }

        public int? DispatcherID { get; set; }

        public virtual City City { get; set; }

        public virtual Dispatcher Dispatcher { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public virtual Pricelist Pricelist { get; set; }

        public string FullName
        {
            get
            {
                return GetFullName();
            }
        }

        [Display(Name = "Dispatcher")]
        public string DispatcherFullName
        {
            get
            {
                return GetDispatcherFullName();
            }
        }

        [Display(Name = "City")]
        public string CityName
        {
            get
            {
                return GetCityName();
            }
        }

        private string GetFullName()
        {
            var name = IsMaster ? "Master" : Name;

            return string.Format("{0}-{1}", CityName, name);
        }

        private string GetCityName()
        {
            var result = "CityNotSet";

            if (City != null)
            {
                result = City.Name;
            }

            return result;
        }

        private string GetDispatcherFullName()
        {
            var result = "DispatcherNotSet";

            if (Dispatcher != null)
            {
                result = Dispatcher.FullName;
            }

            return result;
        }

        private void SetDefaultValues()
        {
            PhoneNumber = 77777777777;
            PhoneNumberMask = "{0:+#(###)###-####}";
        }
    }
}
