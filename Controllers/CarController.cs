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
        public async Task<IActionResult> RentCar([FromQuery] int carId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id")?.Value);

                // Validate input data
                if (startDate >= endDate)
                {
                    var error = "Invalid rental period: Start date must be earlier than end date.";
                    Console.WriteLine(error);
                    return BadRequest(error);
                }

                if (carId <= 0)
                {
                    var error = "Invalid carId: carId must be greater than 0.";
                    Console.WriteLine(error);
                    return BadRequest(error);
                }

                var success = await _carRentalService.RentCarAsync(userId, carId, startDate, endDate);

                if (!success)
                {
                    var error = $"Unable to rent the car. UserId: {userId}, CarId: {carId}, StartDate: {startDate}, EndDate: {endDate}. Possible causes: Car unavailable, invalid data, or service error.";
                    Console.WriteLine(error);
                    return BadRequest(error);
                }

                return Ok($"Car rented successfully for userId {userId} to carId {carId}.");
            }
            catch (Exception ex)
            {
                var error = $"An unexpected error occurred while processing the rental. Details: {ex.Message}";
                Console.WriteLine(error);
                return StatusCode(500, error); // Internal Server Error
            }
        }

    }
}