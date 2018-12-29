using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using GosEvakuator.WebApi.Data;
using GosEvakuator.WebApi.Managers;
using GosEvakuator.WebApi.Services;
using GosEvakuator.WebApi.Services.SmsService;
using GosEvakuator.WebApi.Options;
using GosEvakuator.WebApi.Consts;
using GosEvakuator.WebApi.Models;

namespace GosEvakuator.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                // builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<ApplicationUserManager<ApplicationUser>>();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, SmsService>();
            // services.AddTransient<IMembershipsService, MembershipsController>();
            // services.AddTransient<IAuthorizationHandler, MembershipStatusHandler>();

            //  services.AddAuthorization(options =>
            // {
            //      options.AddPolicy("AcceptedMembership", policy => policy.Requirements.Add(new MembershipStatusRequirement(MembershipStatus.Accepted)));
            //  });

            // Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Facade/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();


            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AuthenticationScheme = "Bearer",
                AutomaticAuthenticate = false,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = AuthOptions.ISSUER,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = AuthOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,

                    //ClockSkew = TimeSpan.Zero
                }
            });

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "api/{area:exists=Facade}/{controller=Home}/{action=Index}/{id?}");
            });

            DatabaseInitialize(app.ApplicationServices).Wait();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
        }

        public async Task DatabaseInitialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<ApplicationUserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string phoneNumber = "89159882658";
            string password = "123456";

            if (await roleManager.FindByNameAsync(Roles.Administrator) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            }

            if (await roleManager.FindByNameAsync(Roles.Dispatcher) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Dispatcher));
            }

            if (await roleManager.FindByNameAsync(Roles.Driver) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Driver));
            }

            if (await roleManager.FindByIdAsync(Roles.Customer) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            }

            if (await userManager.FindByNameAsync(phoneNumber) == null)
            {
                var admin = new ApplicationUser { PhoneNumber = phoneNumber, UserName = phoneNumber };
                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Administrator);
                }
            }
        }
    }
}