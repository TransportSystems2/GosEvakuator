using System.Collections.Generic;

namespace GosEvakuator.Models
{
    public class Shedule
    {
        public Shedule()
        {
            WorkWeek = new HashSet<SheduleDay>();
        }

        public int ID { get; set; }

        public virtual ICollection<SheduleDay> WorkWeek { get; set; }
    }
}