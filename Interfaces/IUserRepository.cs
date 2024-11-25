using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
