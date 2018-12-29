namespace GosEvakuator.Models
{
    public class SheduleHourRange
    {
        public SheduleHourRange()
        {
            Beginning = 8;
            End = 20;
        }

        public int ID { get; set; }

        public int Beginning { get; set; }

        public int End { get; set; }

        public int? DriverID { get; set; }

        public virtual Driver Driver { get; set; }
    }
}