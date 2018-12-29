using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Handlers
{
    public interface ISubdomain
    {
        string Value { get; }
    }

    public class Subdomain : ISubdomain
    {
        public string Value { get; private set; }

        public bool HasValue
        {
            get
            {
                return !string.IsNullOrEmpty(Value);
            }
        }

        public Subdomain(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return;
            }

            // значение по умолчанию
            var subdomain = "moscow";
             
            var domains = host.Split('.').Reverse().ToArray();

            var subdomainIndex = 2;

            if (host.Contains("localhost"))
            {
                subdomainIndex = 1;
            }
            else
            {
                if (host.Contains("azurewebsites.net"))
                {
                    subdomainIndex = 3;
                }
            }

            if (domains.Length > subdomainIndex)
            {
                var domain = domains[subdomainIndex];
                if (!domain.Equals("www"))
                {
                    subdomain = domain;
                }
            }

            Value = subdomain;
        }
    }

    public class SubdomainMiddlewareHandler
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public SubdomainMiddlewareHandler(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger<SubdomainMiddlewareHandler>();
        }

        public async Task Invoke(HttpContext context)
        {
            using (logger.BeginScope("TenantResolverMiddleware"))
            {
                var subdomain = new Subdomain(context.Request.Host.Value);

                logger.LogInformation(string.Format("Resolved tenant. Current tenant: {0}", subdomain));
                context.Features.Set<ISubdomain>(subdomain);

                var val = context.Features.Get<ISubdomain>();

                await next(context);
            }
        }
    }
}