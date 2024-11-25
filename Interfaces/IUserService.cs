using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task<string> AuthenticateUserAsync(string email, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
