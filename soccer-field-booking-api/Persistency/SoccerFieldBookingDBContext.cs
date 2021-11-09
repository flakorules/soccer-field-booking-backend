using Microsoft.EntityFrameworkCore;
using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Persistency
{
    public class SoccerFieldBookingDBContext:DbContext
    {
        public SoccerFieldBookingDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { set; get; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
