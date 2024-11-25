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
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Specify column type
            modelBuilder.Entity<Rental>()
                .Property(r => r.TotalPrice)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasColumnName("Id");

        }
    }
}
