using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GosEvakuator.Data;
using GosEvakuator.Models;

namespace GosEvakuator.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170625075131_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GosEvakuator.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<bool>("VerifiedCode");

                    b.Property<string>("VerifyCode");

                    b.Property<DateTime>("VerifyCodeTime");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GosEvakuator.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Area");

                    b.Property<string>("Domen");

                    b.Property<string>("Name");

                    b.Property<string>("PrepositionalArea");

                    b.Property<string>("PrepositionalName");

                    b.HasKey("ID");

                    b.ToTable("City");
                });

            modelBuilder.Entity("GosEvakuator.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("GosEvakuator.Models.Dispatcher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Dispatchers");
                });

            modelBuilder.Entity("GosEvakuator.Models.Driver", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GarageID");

                    b.Property<bool>("IsAccept");

                    b.HasKey("ID");

                    b.HasIndex("GarageID");

                    b.ToTable("Driver");
                });

            modelBuilder.Entity("GosEvakuator.Models.Garage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CityID");

                    b.Property<int?>("DispatcherID");

                    b.Property<bool>("IsMaster");

                    b.Property<string>("Name");

                    b.Property<double>("PhoneNumber");

                    b.Property<string>("PhoneNumberMask");

                    b.Property<int?>("PricelistID");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.HasIndex("DispatcherID");

                    b.HasIndex("PricelistID");

                    b.ToTable("Garages");
                });

            modelBuilder.Entity("GosEvakuator.Models.Membership", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int?>("CustomerID");

                    b.Property<int?>("DispatcherID");

                    b.Property<int?>("DriverID");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("CustomerID")
                        .IsUnique();

                    b.HasIndex("DispatcherID")
                        .IsUnique();

                    b.HasIndex("DriverID")
                        .IsUnique();

                    b.ToTable("Membership");
                });

            modelBuilder.Entity("GosEvakuator.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CityID");

                    b.Property<int>("CustomerID");

                    b.Property<int>("Distance");

                    b.Property<string>("PlaceArrival");

                    b.Property<string>("PlaceDeparture");

                    b.Property<int?>("VehicleID");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("VehicleID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("GosEvakuator.Models.OrderStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OrderID");

                    b.Property<DateTime>("Time");

                    b.Property<int>("Type");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderStatus");
                });

            modelBuilder.Entity("GosEvakuator.Models.Pricelist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Pricelist");
                });

            modelBuilder.Entity("GosEvakuator.Models.PricelistItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<int>("Discount");

                    b.Property<int>("LoadingVehicle");

                    b.Property<int>("LockedSteeringWheel");

                    b.Property<int>("LockedWheel");

                    b.Property<string>("Name");

                    b.Property<decimal>("PerKilometer");

                    b.Property<int>("PricelistID");

                    b.HasKey("ID");

                    b.HasIndex("PricelistID");

                    b.ToTable("PricelistItem");
                });

            modelBuilder.Entity("GosEvakuator.Models.Shedule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Shedule");
                });

            modelBuilder.Entity("GosEvakuator.Models.SheduleDay", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DayOfWeek");

                    b.Property<int?>("SheduleID");

                    b.HasKey("ID");

                    b.HasIndex("SheduleID");

                    b.ToTable("SheduleDay");
                });

            modelBuilder.Entity("GosEvakuator.Models.SheduleHourRange", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Beginning");

                    b.Property<int?>("DriverID");

                    b.Property<int>("End");

                    b.Property<int?>("SheduleDayID");

                    b.HasKey("ID");

                    b.HasIndex("DriverID");

                    b.HasIndex("SheduleDayID");

                    b.ToTable("SheduleHourRange");
                });

            modelBuilder.Entity("GosEvakuator.Models.Vehicle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GarageID");

                    b.Property<bool>("IsAccept");

                    b.Property<int>("MembershipID");

                    b.Property<string>("Name");

                    b.Property<string>("RegistrationNumber");

                    b.Property<int>("SheduleID");

                    b.HasKey("ID");

                    b.HasIndex("GarageID");

                    b.HasIndex("MembershipID");

                    b.HasIndex("SheduleID");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GosEvakuator.Models.Driver", b =>
                {
                    b.HasOne("GosEvakuator.Models.Garage", "Garage")
                        .WithMany()
                        .HasForeignKey("GarageID");
                });

            modelBuilder.Entity("GosEvakuator.Models.Garage", b =>
                {
                    b.HasOne("GosEvakuator.Models.City", "City")
                        .WithMany("Garages")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.Dispatcher", "Dispatcher")
                        .WithMany("Garages")
                        .HasForeignKey("DispatcherID");

                    b.HasOne("GosEvakuator.Models.Pricelist", "Pricelist")
                        .WithMany()
                        .HasForeignKey("PricelistID");
                });

            modelBuilder.Entity("GosEvakuator.Models.Membership", b =>
                {
                    b.HasOne("GosEvakuator.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("GosEvakuator.Models.Customer", "Customer")
                        .WithOne("Membership")
                        .HasForeignKey("GosEvakuator.Models.Membership", "CustomerID");

                    b.HasOne("GosEvakuator.Models.Dispatcher", "Dispatcher")
                        .WithOne("Membership")
                        .HasForeignKey("GosEvakuator.Models.Membership", "DispatcherID");

                    b.HasOne("GosEvakuator.Models.Driver", "Driver")
                        .WithOne("Membership")
                        .HasForeignKey("GosEvakuator.Models.Membership", "DriverID");
                });

            modelBuilder.Entity("GosEvakuator.Models.Order", b =>
                {
                    b.HasOne("GosEvakuator.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleID");
                });

            modelBuilder.Entity("GosEvakuator.Models.OrderStatus", b =>
                {
                    b.HasOne("GosEvakuator.Models.Order")
                        .WithMany("OrderStatuses")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("GosEvakuator.Models.PricelistItem", b =>
                {
                    b.HasOne("GosEvakuator.Models.Pricelist")
                        .WithMany("Items")
                        .HasForeignKey("PricelistID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GosEvakuator.Models.SheduleDay", b =>
                {
                    b.HasOne("GosEvakuator.Models.Shedule")
                        .WithMany("WorkWeek")
                        .HasForeignKey("SheduleID");
                });

            modelBuilder.Entity("GosEvakuator.Models.SheduleHourRange", b =>
                {
                    b.HasOne("GosEvakuator.Models.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverID");

                    b.HasOne("GosEvakuator.Models.SheduleDay")
                        .WithMany("HourRanges")
                        .HasForeignKey("SheduleDayID");
                });

            modelBuilder.Entity("GosEvakuator.Models.Vehicle", b =>
                {
                    b.HasOne("GosEvakuator.Models.Garage", "Garage")
                        .WithMany("Vehicles")
                        .HasForeignKey("GarageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.Membership", "Memebership")
                        .WithMany()
                        .HasForeignKey("MembershipID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.Shedule", "Shedule")
                        .WithMany()
                        .HasForeignKey("SheduleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GosEvakuator.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GosEvakuator.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GosEvakuator.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
