using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface IEmailService
    {
        Task SendRentalConfirmationEmailAsync(string userEmail, Car car, Rental rental);
    }
}