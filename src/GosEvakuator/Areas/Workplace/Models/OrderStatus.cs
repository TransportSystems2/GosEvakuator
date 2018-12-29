using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models
{
    public enum StatusType
    {
        New,

        Accepted,

        Failed,

        Processing,

        Completed,

        Canceled
    }

    public class OrderStatus
    {
        public OrderStatus()
        {
            Type = StatusType.New;
        }

        public int ID { get; set; }

        public DateTime Time { get; set; }

        public StatusType Type { get; set; }
    }
}
