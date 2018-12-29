using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Models
{
    public class Member
    {
        public int ID { get; set; }

        public virtual Membership Membership { get; set; }

        public ApplicationUser ApplicationUser
        {
            get
            {
                return Membership.ApplicationUser;
            }
        }

        public virtual string FirstName
        {
            get
            {
                return ApplicationUser.FirstName;
            }
        }

        public virtual string LastName
        {
            get
            {
                return ApplicationUser.LastName;
            }
        }

        public virtual string FullName
        {
            get
            {
                return ApplicationUser.FullName;
            }
        }

        public virtual string PhoneNumber
        {
            get
            {
                return ApplicationUser.PhoneNumber;
            }
        }
    }
}