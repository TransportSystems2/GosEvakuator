using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models
{
    public enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wensdey,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class SheduleDay
    {
        public SheduleDay()
        {
            HourRanges = new HashSet<SheduleHourRange>();
        }

        public int ID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public virtual ICollection<SheduleHourRange> HourRanges { get; set; }
    }
}
