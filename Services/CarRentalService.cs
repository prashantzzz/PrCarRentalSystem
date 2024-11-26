using PrCarRentalSystem.Data;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;
using Microsoft.EntityFrameworkCore; // for Include() and AnyAsync() types..

namespace PrCarRentalSystem.Services
{
    public class CarRentalService : ICarRentalService
    {
        private readonly ICarRepository _carRepository;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public CarRentalService(
            ICarRepository carRepository,
            IEmailService emailService,
            ApplicationDbContext context)
        {
            _carRepository = carRepository;
            _emailService = emailService;
            _context = context;
        }

        public async Task<bool> RentCarAsync(int userId, int carId, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Check if the car exists and is available
                var car = await _carRepository.GetCarByIdAsync(carId);
                if (car == null)
                {
                    Console.WriteLine($"Car with ID {carId} not found.");
                    return false;
                }

                if (!car.IsAvailable)
                {
                    Console.WriteLine($"Car with ID {carId} is not available for rental.");
                    return false;
                }

                // Check availability for the specified date range
                var isAvailable = await CheckCarAvailabilityAsync(carId, startDate, endDate);
                if (!isAvailable)
                {
                    Console.WriteLine($"Car with ID {carId} is already rented for the selected dates: {startDate} to {endDate}.");
                    return false;
                }

                // Calculate rental price
                var totalPrice = await CalculateRentalPriceAsync(carId, startDate, endDate);

                // Create and save the rental record
                var rental = new Rental
                {
                    UserId = userId,
                    CarId = carId,
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalPrice = totalPrice
                };

                await _context.Rentals.AddAsync(rental);
                await _carRepository.UpdateCarAvailabilityAsync(carId, false);

                var saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                {
                    //Console.WriteLine($"Failed to save rental record for car ID {carId} and user ID {userId}. Changes not persisted.");
                    return false;
                }

                Console.WriteLine($"Successfully saved rental record for car ID {carId} and user ID {userId}. Rental ID: {rental.Id}");

                // Send confirmation email
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    try
                    {
                        await _emailService.SendRentalConfirmationEmailAsync(user.Email, car, rental);
                        Console.WriteLine($"Confirmation email sent to {user.Email}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending confirmation email to user {user.Email}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"User with ID {userId} not found for email notification.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in RentCarAsync: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> CheckCarAvailabilityAsync(int carId, DateTime startDate, DateTime endDate)
        {
            var existingRentals = await _context.Rentals
                .Where(r => r.CarId == carId &&
                           ((startDate >= r.StartDate && startDate <= r.EndDate) ||
                            (endDate >= r.StartDate && endDate <= r.EndDate)))
                .AnyAsync();

            return !existingRentals;
        }

        public async Task<decimal> CalculateRentalPriceAsync(int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
                throw new ArgumentException("Car not found");

            var days = (endDate - startDate).Days;
            return car.PricePerDay * days;
        }

        public async Task<IEnumerable<Rental>> GetUserRentalsAsync(int userId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
