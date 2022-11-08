using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightReservation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=Dotnet; Database=FlightReservation;User ID=sa;Password=wipro@123");
        }

        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<FlightDetail> FlightDetails { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }

    }
}
