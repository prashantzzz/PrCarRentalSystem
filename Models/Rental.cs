﻿namespace PrCarRentalSystem.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Car Car { get; set; }
    }
}
