using GosEvakuator.Handlers;
using Microsoft.AspNetCore.Builder;

namespace GosEvakuator.Extension
{
    public static class SubdomainMiddlewareExtension
    {
        public static void UseSubdomain(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SubdomainMiddlewareHandler>();
        }
    }
}