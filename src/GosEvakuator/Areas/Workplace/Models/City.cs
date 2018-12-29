using System.Collections.Generic;
using System.Linq;

namespace GosEvakuator.Models
{
    public class City
    {
        public City()
        {
            Garages = new HashSet<Garage>();
        }

        public int ID { get; set; }

        public string Domen { get; set; }

        public string Name { get; set; }

        public string PrepositionalName { get; set; }

        public string Area { get; set; }

        public string PrepositionalArea { get; set; }

        public virtual ICollection<Garage> Garages { get; set; }

        public Garage GetMasterGarage()
        {
            return Garages.FirstOrDefault(g => g.IsMaster);
        }
    }
}