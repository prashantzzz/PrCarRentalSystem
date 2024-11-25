using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrCarRentalSystem.Models;
namespace PrCarRentalSystem.Data
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            builder.HasMany(u => u.Rentals)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
