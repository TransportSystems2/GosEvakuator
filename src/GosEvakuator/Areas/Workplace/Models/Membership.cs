namespace GosEvakuator.Models
{
    public enum MembershipStatus
    {
        New,

        Accepted,

        Suspended,

        Canceled
    }

    public class Membership
    {
        public int ID { get; set; }

        public string ApplicationUserId { get; set; }

        public int? DriverID { get; set; }

        public int? DispatcherID { get; set; }

        public int? CustomerID { get; set; }

        public MembershipStatus Status { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Dispatcher Dispatcher { get; set; }

        public virtual Customer Customer { get; set; }
    }
}