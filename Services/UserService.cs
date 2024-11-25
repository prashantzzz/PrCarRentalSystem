using System.Security.Cryptography;
using System.Text;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            if (await _userRepository.GetUserByEmailAsync(user.Email) != null)
                throw new ArgumentException("Email already exists");

            user.Password = HashPassword(password);
            return await _userRepository.AddUserAsync(user);
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return null;

            var hashedPassword = HashPassword(password);
            if (user.Password != hashedPassword)
                return null;

            return _jwtService.GenerateToken(user);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null;
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
