using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrCarRentalSystem.Models;
namespace PrCarRentalSystem.Data
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Make).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Model).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Year).IsRequired();
            builder.Property(c => c.PricePerDay).IsRequired().HasPrecision(18, 2);

            builder.HasMany(c => c.Rentals)
                   .WithOne(r => r.Car)
                   .HasForeignKey(r => r.CarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
