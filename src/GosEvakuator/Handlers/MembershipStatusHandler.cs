using GosEvakuator.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GosEvakuator.Handlers
{
    public class MembershipStatusHandler : AuthorizationHandler<MembershipStatusRequirement>
    {
        private IMembershipsService membershipsService;

        public MembershipStatusHandler(IMembershipsService memberships)
        {
            membershipsService = memberships;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MembershipStatusRequirement requirement)
        {
            var membership = membershipsService.GetCurrentMembership(context.User).Result;

            if ((membership != null) && (membership.Status == requirement.Status))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}