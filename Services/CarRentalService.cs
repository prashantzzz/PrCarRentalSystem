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
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null || !car.IsAvailable)
                return false;

            var isAvailable = await CheckCarAvailabilityAsync(carId, startDate, endDate);
            if (!isAvailable)
                return false;

            var totalPrice = await CalculateRentalPriceAsync(carId, startDate, endDate);

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

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                var user = await _context.Users.FindAsync(userId);
                await _emailService.SendRentalConfirmationEmailAsync(user.Email, car, rental);
            }

            return result;
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
