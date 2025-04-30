# BSLTours.API

This is the .NET Core API backend for the Best Sri Lanka Tours website. It provides REST API endpoints for tour packages, destinations, testimonials, inquiries, and newsletter subscriptions.

## Technologies Used

- ASP.NET Core 8.0
- C# 12
- Swagger for API documentation
- In-memory data storage (can be replaced with a database in production)

## Project Structure

```
BSLTours.API/
├── Controllers/           # API Controllers
├── Models/                # Domain models and DTOs
├── Services/              # Business logic and data access
├── Properties/            # Launch settings
├── appsettings.json       # Application settings
├── Program.cs             # Application entry point
└── BSLTours.API.csproj    # Project file
```

## Features

- **Tour Packages**: Browse all tour packages, view featured packages, and get package details
- **Destinations**: Browse all destinations and destination details
- **Testimonials**: View customer testimonials
- **Inquiries**: Submit and manage customer inquiries
- **Newsletter**: Subscribe to the newsletter

## API Endpoints

### Tour Packages
- `GET /api/tour-packages` - Get all tour packages
- `GET /api/tour-packages/featured` - Get featured tour packages
- `GET /api/tour-packages/{id}` - Get a specific tour package by ID
- `POST /api/tour-packages` - Create a new tour package

### Destinations
- `GET /api/destinations` - Get all destinations
- `GET /api/destinations/{id}` - Get a specific destination by ID
- `POST /api/destinations` - Create a new destination

### Testimonials
- `GET /api/testimonials` - Get all testimonials
- `POST /api/testimonials` - Create a new testimonial

### Inquiries
- `GET /api/inquiries` - Get all inquiries (admin only)
- `POST /api/inquiries` - Submit a new inquiry

### Newsletter
- `POST /api/subscribers` - Subscribe to the newsletter

## Getting Started

1. Make sure you have .NET 8.0 SDK installed
2. Clone this repository
3. Navigate to the BSLTours.API directory
4. Run `dotnet restore` to restore dependencies
5. Run `dotnet build` to build the project
6. Run `dotnet run` to start the API

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7048

## Swagger Documentation

When running in development mode, Swagger documentation is available at:
- https://localhost:7048/swagger

## Deployment

See the [Deployment Guide](./README-DEPLOYMENT.md) for instructions on deploying to production.