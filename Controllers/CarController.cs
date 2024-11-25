using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ICarRentalService _carRentalService;

        public CarController(
            ICarRepository carRepository,
            ICarRentalService carRentalService)
        {
            _carRepository = carRepository;
            _carRentalService = carRentalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAvailableCars()
        {
            var cars = await _carRepository.GetAvailableCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
                return NotFound();

            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Car>> AddCar(Car car)
        {
            var result = await _carRepository.AddCarAsync(car);
            return CreatedAtAction(nameof(GetCar), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCar(int id, Car car)
        {
            if (id != car.Id)
                return BadRequest();

            var success = await _carRepository.UpdateCarAsync(car);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var success = await _carRepository.DeleteCarAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("rent")]
        [Authorize]
        public async Task<IActionResult> RentCar(int carId, DateTime startDate, DateTime endDate)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var success = await _carRentalService.RentCarAsync(userId, carId, startDate, endDate);

            if (!success)
                return BadRequest("Unable to rent the car");

            return Ok();
        }
    }
}