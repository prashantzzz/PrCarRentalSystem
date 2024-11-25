using Microsoft.EntityFrameworkCore;
using PrCarRentalSystem.Data;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _context.Cars
                .Where(c => c.IsAvailable)
                .ToListAsync();
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await GetCarByIdAsync(id);
            if (car == null) return false;

            _context.Cars.Remove(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCarAvailabilityAsync(int carId, bool isAvailable)
        {
            var car = await GetCarByIdAsync(carId);
            if (car == null) return false;

            car.IsAvailable = isAvailable;
            return await UpdateCarAsync(car);
        }
    }
}
