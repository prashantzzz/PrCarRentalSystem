using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICarRentalService _carRentalService;

        public UserController(
            IUserService userService,
            ICarRentalService carRentalService)
        {
            _userService = userService;
            _carRentalService = carRentalService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user, string password)
        {
            try
            {
                var result = await _userService.RegisterUserAsync(user, password);
                return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string email, string password)
        {
            var token = await _userService.AuthenticateUserAsync(email, password);
            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("rentals")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUserRentals()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var rentals = await _carRentalService.GetUserRentalsAsync(userId);
            return Ok(rentals);
        }
    }
}