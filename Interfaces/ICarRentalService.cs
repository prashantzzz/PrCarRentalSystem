using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface ICarRentalService
    {
        Task<bool> RentCarAsync(int userId, int carId, DateTime startDate, DateTime endDate);
        Task<bool> CheckCarAvailabilityAsync(int carId, DateTime startDate, DateTime endDate);
        Task<decimal> CalculateRentalPriceAsync(int carId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Rental>> GetUserRentalsAsync(int userId);
    }
}
