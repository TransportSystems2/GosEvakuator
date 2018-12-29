using System;
using System.Collections.Generic;

namespace GosEvakuator.Models
{
    public class Order
    {
        public Order()
        {
            OrderStatuses = new HashSet<OrderStatus>();
            Customer = new Customer();
        }

        public int ID { get; set; }

        public int CityID { get; set; }

        public int? VehicleID { get; set; }

        public int CustomerID { get; set; }

        public string PlaceDeparture { get; set; }

        public string PlaceArrival { get; set; }

        public int Distance { get; set; }

        public virtual City City { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
    }
}
