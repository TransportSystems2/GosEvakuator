using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GosEvakuator.Models
{
    public class PricelistItem
    {
        [JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore]
        public int PricelistID { get; set; }

        public string Alias { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        public int Discount { get; set; }

        public int LockedWheel { get; set; }

        public int LockedSteeringWheel { get; set; }

        public int LoadingVehicle { get; set; }

        public decimal PerKilometer { get; set; }

        [NotMapped]
        public int LoadingVehicleFromOther
        {
            get
            {
                return GetLoadingVehicleFromOther();
            }
        }

        private int GetLoadingVehicleFromOther()
        {
            var result = LoadingVehicle * ((Discount + 100) / 100d);
            result = Math.Round(result / 1000, 1) * 1000;

            return (int)result;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}