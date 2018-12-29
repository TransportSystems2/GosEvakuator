using GosEvakuator.Models;
using Microsoft.AspNetCore.Authorization;

namespace GosEvakuator.Handlers
{
    public class MembershipStatusRequirement : IAuthorizationRequirement
    {
        protected internal MembershipStatus Status { get; set; }

        public MembershipStatusRequirement(MembershipStatus status)
        {
            Status = status;
        }
    }
}