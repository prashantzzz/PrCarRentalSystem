### README.md

# Car Rental Service with MailerSend Integration

This project is a Car Rental Service system that integrates the MailerSend API to send email notifications (e.g., rental confirmations) to users.

---

## Features
- **Car Rental Management:** Users can rent cars based on availability and specified dates.
- **Email Notifications:** Automated email notifications using the MailerSend API.
- **Clean Code Architecture:** Modular design for easy scalability and maintenance.

---

## Technologies Used
- **Backend:** ASP.NET Core
- **Email API:** [MailerSend](https://www.mailersend.com)
- **Dependencies:** `HttpClient`, `Newtonsoft.Json`

---

## Setup Guide

### 1. Prerequisites
- .NET 6 SDK installed
- MailerSend account with an API token

---

### 2. Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-repo/car-rental-service.git
   cd car-rental-service
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```

---

### 3. Configuration
1. **MailerSend API Token:** Replace the placeholder API token in `EmailService.cs`:
   ```csharp
   private readonly string _apiToken = "your-api-token";
   ```
2. **Sender Email:** Update the sender email address and name:
   ```csharp
   private readonly string _fromEmail = "info@yourdomain.com";
   private readonly string _fromName = "Your Company Name";
   ```

---

### 4. Running the Application
1. Build the project:
   ```bash
   dotnet build
   ```
2. Run the project:
   ```bash
   dotnet run
   ```

---

## How It Works
1. When a user rents a car, the system checks availability and calculates the total rental cost.
2. Upon successful rental, an email is sent to the user via the MailerSend API with rental details.

---

## Example Usage
Here’s how you can trigger email sending:
```csharp
await _emailService.SendRentalConfirmationEmailAsync(
    "recipient@example.com",
    "Rental Confirmation",
    "Your rental has been confirmed!",
    "<b>Your rental has been confirmed!</b>"
);
```

---

## License
This project is licensed under the MIT License.

---

Feel free to contribute and improve the system. Happy coding! 😊