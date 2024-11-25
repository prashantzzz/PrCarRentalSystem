using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface ICarRepository
    {
        Task<Car> AddCarAsync(Car car);
        Task<Car> GetCarByIdAsync(int id);
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int id);
        Task<bool> UpdateCarAvailabilityAsync(int carId, bool isAvailable);
    }
}
