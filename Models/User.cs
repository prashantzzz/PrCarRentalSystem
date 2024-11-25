using System.ComponentModel.DataAnnotations;

namespace PrCarRentalSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // Navigation property for rentals
        public ICollection<Rental> Rentals { get; set; }
    }
    
}
