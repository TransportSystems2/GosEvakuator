using GosEvakuator.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GosEvakuator.Services
{
    public interface IMembershipsService
    {
        Task<Membership> GetCurrentMembership(ClaimsPrincipal user);

        Task<Member> GetCurrentMember(ClaimsPrincipal user);
    }
}