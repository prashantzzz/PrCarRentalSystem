using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task SendRentalConfirmationEmailAsync(string userEmail, Car car, Rental rental)
        {
            var apiKey = _configuration["Email:MailerSendApiKey"];
            var senderEmail = _configuration["Email:From"];

            var emailPayload = new
            {
                from = new { email = senderEmail },
                to = new[] { new { email = userEmail } },
                subject = "Car Rental Confirmation",
                text = $@"
                    Dear Customer,

                    Your car rental has been confirmed!

                    Details:
                    Car: {car.Make} {car.Model} ({car.Year})
                    Start Date: {rental.StartDate:d}
                    End Date: {rental.EndDate:d}
                    Total Price: ${rental.TotalPrice}

                    Thank you for choosing our service!

                    Best regards,
                    Car Rental System Team
                ",
                html = $@"
                    <p>Dear Customer,</p>
                    <p>Your car rental has been confirmed!</p>
                    <p><strong>Details:</strong><br/>
                    Car: {car.Make} {car.Model} ({car.Year})<br/>
                    Start Date: {rental.StartDate:d}<br/>
                    End Date: {rental.EndDate:d}<br/>
                    Total Price: ${rental.TotalPrice}</p>
                    <p>Thank you for choosing our service!</p>
                    <p>Best regards,<br/>Car Rental System Team</p>
                "
            };

            var jsonPayload = JsonSerializer.Serialize(emailPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _httpClient.PostAsync("https://api.mailersend.com/v1/email", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
