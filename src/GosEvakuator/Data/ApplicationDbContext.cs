using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using GosEvakuator.Models;

namespace GosEvakuator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<City> City { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<Dispatcher> Dispatchers { get; set; }

        public DbSet<Driver> Driver { get; set; }

        public DbSet<Garage> Garages { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DbSet<Shedule> Shedule { get; set; }

        public DbSet<SheduleDay> SheduleDay { get; set; }

        public DbSet<SheduleHourRange> SheduleHourRange { get; set; }

        public DbSet<Vehicle> Vehicle { get; set; }

        public DbSet<Membership> Membership { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<GosEvakuator.Models.Pricelist> Pricelist { get; set; }

        public DbSet<GosEvakuator.Models.PricelistItem> PricelistItem { get; set; }
    }
}
