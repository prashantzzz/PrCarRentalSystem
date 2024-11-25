using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}