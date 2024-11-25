using System.ComponentModel.DataAnnotations;
namespace PrCarRentalSystem.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Make { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation property for rental history
        public ICollection<Rental> Rentals { get; set; }
    }

}
