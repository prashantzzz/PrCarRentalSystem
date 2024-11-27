# Car Rental System by Prashant

## Overview
This project is a **Car Rental System** developed in **C#** using **ASP.NET Core**. The system allows users to register, log in, rent cars, and manage rental records. Admins can manage car listings, update car details, and handle rental requests.
Screenshots file is also there as PrCarRentalAssign.pdf

## Features
- User Registration & Login
- Car Management (CRUD operations)
- Car Rental Booking
- Email Notifications on Rental Confirmation
- JWT-based Authentication & Authorization

## Project Structure

### Root Directory
- **AboutCurrentProject.txt**: A text file with more information about the project.
- **appsettings.json**: Configuration settings for production environment.
- **README.md**: This file.
- **Program.cs**: The entry point for the application.

### Folders
- **Controllers**: Contains API controllers to handle user and car-related requests.
  - `CarController.cs`: Handles car-related operations.
  - `UserController.cs`: Handles user-related operations.

- **Data**: Contains files related to database configuration and entity models.
  - `ApplicationDbContext.cs`: The database context class.
  - `CarConfiguration.cs`, `UserConfiguration.cs`: Entity configurations for `Car` and `User` models.

- **Filters**: Contains custom filters for request validation.
  - `ValidationFilter.cs`: Validates input data.

- **Interfaces**: Contains interfaces for services and repositories.
  - `ICarRentalService.cs`, `ICarRepository.cs`, `IEmailService.cs`, `IJwtService.cs`, `IUserRepository.cs`, `IUserService.cs`: Service and repository interfaces.

- **Middleware**: Contains middleware for JWT authentication.
  - `JwtMiddleware.cs`: Middleware for handling JWT token authentication.

- **Migrations**: Database migrations for setting up and updating the database schema.
  - `20241125100159_InitialCreate.cs`: Initial database migration.

- **Models**: Contains models representing the data entities.
  - `Car.cs`, `Rental.cs`, `User.cs`: Entity classes for `Car`, `Rental`, and `User`.

- **Repositories**: Contains implementation of data repositories.
  - `CarRepository.cs`, `UserRepository.cs`: Repositories for accessing car and user data.

- **Services**: Contains business logic services.
  - `CarRentalService.cs`, `EmailService.cs`, `JWTService.cs`, `UserService.cs`: Services for handling car rentals, email notifications, JWT token management, and user operations.

- **Shared**: Contains shared constants and helpers.
  - `AuthorizationConstants.cs`: Contains authorization constants for JWT tokens.

## API Endpoints

### Car Endpoints
![image](https://github.com/user-attachments/assets/3eef5062-5182-4495-9e77-b6bf40161483)

- **GET /api/car**
  - Retrieve a list of all available cars.
  
- **POST /api/car**
  - Add a new car to the system (Admin only).
  
- **GET /api/car/{id}**
  - Retrieve details of a specific car by ID.
  
- **PUT /api/car/{id}**
  - Update the details of an existing car (Admin only).
  
- **DELETE /api/car/{id}**
  - Delete a car from the system (Admin only).
  
- **POST /api/car/rent**
  - Rent a car (User authenticated with JWT).

### User Endpoints
- **POST /api/user/register**
  - Register a new user.
  
- **POST /api/user/login**
  - Login to the system and obtain a JWT token.
  
- **GET /api/user/{id}**
  - Retrieve details of a specific user by ID.
  
- **GET /api/user/rentals**
  - Retrieve a list of rentals made by the authenticated user.

## Setup & Configuration

### 1. **Clone the repository**:
   Clone the repository to your local machine.

### 2. **Database Setup**:
   - The project uses **Entity Framework Core** for database management.
   - Run migrations to create the database schema.
   - Update `appsettings.json` with the correct connection string and email service credentials.

### 3. **Email Configuration**:
   - Email notifications are sent via SMTP. Make sure to update the email configuration in `appsettings.json` for Gmail or any other SMTP server you use.

### 4. **JWT Authentication**:
   - The system uses **JWT** for authenticating users and their role. The signing keys and token expiration settings are in `appsettings.json`.

### 5. **Run the Project**:
   - Open the project in Visual Studio and hit the Http button.
   - The API will be accessible via `https://localhost:{port}`.

## Running Migrations
Commands to apply migrations to the database:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Example Usage

1. **Register a User**:
   - `POST /api/user/register` with `username`, `email`, `password`, etc.
   
2. **Login a User**:
   - `POST /api/user/login` with `username` and `password` to get a JWT token.

3. **Rent a Car**:
   - `POST /api/car/rent` with a JWT token in the Authorization header, along with `carId`, `startDate`, and `endDate` parameters.

## Conclusion
This project provides a simple but functional car rental system using **ASP.NET Core**. It implements essential features like user registration, car management, rental booking, and email notifications with **JWT authentication** for secure access.

