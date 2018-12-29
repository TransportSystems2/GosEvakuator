using GosEvakuator.Consts;
using GosEvakuator.Controllers;
using GosEvakuator.Data;
using GosEvakuator.Handlers;
using GosEvakuator.Models;
using GosEvakuator.Services;
using GosEvakuator.Extension;

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
using GosEvakuator.Options;
using GosEvakuator.Managers;

namespace GosEvakuator
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
            services.AddTransient<IEmailSender, MessageServices>();
            services.AddTransient<ISmsSender, MessageServices>();
            services.AddTransient<IMembershipsService, MembershipsController>();
            services.AddTransient<IAuthorizationHandler, MembershipStatusHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AcceptedMembership", policy => policy.Requirements.Add(new MembershipStatusRequirement(MembershipStatus.Accepted)));
            });
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
            app.UseAuthentication();

            app.UseSubdomain();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists=Facade}/{controller=Home}/{action=Index}/{id?}");
            });

            DatabaseInitialize(app.ApplicationServices).Wait();
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