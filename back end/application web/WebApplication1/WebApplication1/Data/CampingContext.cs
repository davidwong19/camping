using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class CampingContext : DbContext
    {
       
        public CampingContext(DbContextOptions<CampingContext> options) : base(options) { }

       
        public DbSet<User> Users { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<CampingSpot> CampingSpots { get; set; }
       public DbSet<Booking> Bookings { get; set; }
      public DbSet<Review> Reviews { get; set; }
       public DbSet<Comment> Comments { get; set; }
        public DbSet<OwnerCampingSpotBooked> OwnerCampingSpotsBooked { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         // test of het werkt
            modelBuilder.Entity<Owner>().HasData(
                new Owner { Id = 1, Username = "testuser", Email = "testuser@example.com", Password = "password123" }
            );
        }

    }

}
