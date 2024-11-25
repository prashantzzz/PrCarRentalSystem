using SendGrid;
using SendGrid.Helpers.Mail;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;
using System.Threading.Tasks;

namespace PrCarRentalSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRentalConfirmationEmailAsync(string userEmail, Car car, Rental rental)
        {
            // Get SendGrid API key from appsettings.json
            var apiKey = _configuration["Email:SendGridApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(_configuration["Email:From"], "Car Rental System");
            var subject = "Car Rental Confirmation";
            var to = new EmailAddress(userEmail);

            var plainTextContent = $@"
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
            ";

            var htmlContent = $@"
                <p>Dear Customer,</p>
                <p>Your car rental has been confirmed!</p>
                <p><strong>Details:</strong><br/>
                Car: {car.Make} {car.Model} ({car.Year})<br/>
                Start Date: {rental.StartDate:d}<br/>
                End Date: {rental.EndDate:d}<br/>
                Total Price: ${rental.TotalPrice}</p>
                <p>Thank you for choosing our service!</p>
                <p>Best regards,<br/>Car Rental System Team</p>
            ";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            // Send the email using SendGrid
            var response = await client.SendEmailAsync(msg);
        }
    }
}
