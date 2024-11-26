using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrCarRentalSystem.Interfaces;
using PrCarRentalSystem.Models;

namespace PrCarRentalSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _emailUsername;
        private readonly string _emailPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _fromEmail = configuration["Email:From"];
            _emailUsername = configuration["Email:Username"];
            _emailPassword = configuration["Email:Password"];
            _smtpHost = configuration["Email:Host"];
            _smtpPort = int.Parse(configuration["Email:Port"]);
            _logger = logger;
        }

        public async Task SendRentalConfirmationEmailAsync(string userEmail, Car car, Rental rental)
        {
            var subject = "Car Rental Confirmation";
            var plainText = $@"
                Dear Customer,

                Your car rental has been confirmed!

                Details:
                Car: {car.Make} {car.Model} ({car.Year})
                Start Date: {rental.StartDate:yyyy-MM-dd}
                End Date: {rental.EndDate:yyyy-MM-dd}
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
                Start Date: {rental.StartDate:yyyy-MM-dd}<br/>
                End Date: {rental.EndDate:yyyy-MM-dd}<br/>
                Total Price: ${rental.TotalPrice}</p>
                <p>Thank you for choosing our service!</p>
                <p>Best regards,<br/>Car Rental System Team</p>
            ";

            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailUsername, _emailPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(userEmail);
                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainText, null, "text/plain"));
                mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlContent, null, "text/html"));

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email successfully sent to {userEmail}.", userEmail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {userEmail}.", userEmail);
                    throw;
                }
            }
        }
    }
}
