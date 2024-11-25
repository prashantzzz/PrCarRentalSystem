using Microsoft.EntityFrameworkCore;
using PrCarRentalSystem.Models;
namespace PrCarRentalSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // Specify column type for the TotalPrice property
            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");

        }
    }
}
