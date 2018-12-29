using System.Collections.Generic;
using System.Linq;

namespace GosEvakuator.Models
{
    public class Pricelist
    {
        public Pricelist()
        {
            Items = new HashSet<PricelistItem>();
        }

        public int ID { get; set; }

        public ICollection<PricelistItem> Items { get; set; }

        public PricelistItem GetDefaultedPricelistItem()
        {
            return Items.FirstOrDefault(i => i.Alias.Equals("car"));
        }
    }
}