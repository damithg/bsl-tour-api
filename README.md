# BSLTours.API

BSLTours.API is a comprehensive .NET 8.0 Web API backend for the Best Sri Lanka Tours website. This API serves as the bridge between the frontend client and the Strapi CMS backend, providing structured endpoints for tours, destinations, experiences, contact forms, and more.

## 🚀 Project Overview

**BSLTours.API** is designed to support a tourism website showcasing Sri Lankan tours and destinations. The API integrates with a Strapi CMS for content management and provides RESTful endpoints for:

- **Tour Packages**: Comprehensive tour information with itineraries, pricing, and bookings
- **Destinations**: Detailed destination information with attractions and features
- **Experiences**: Activity-based experiences available in different locations
- **Contact Management**: Dynamic contact forms with email notifications
- **Newsletter**: Subscription management

## 🏗️ Architecture

The application follows a clean architecture pattern:

```
BSLTours.API/
├── Controllers/              # API Controllers (HTTP endpoints)
│   ├── ToursController.cs           # Tour package endpoints
│   ├── DestinationsController.cs    # Destination endpoints
│   ├── ExperiencesController.cs     # Experience endpoints
│   ├── ContactController.cs         # Contact form handling
│   ├── InquiriesController.cs       # Customer inquiry management
│   ├── SubscribersController.cs     # Newsletter subscriptions
│   └── TestimonialsController.cs    # Customer testimonials
├── Models/                   # Domain models and DTOs
│   ├── Dtos/                        # Data Transfer Objects
│   ├── TourDto.cs                   # Tour data models
│   ├── DestinationDto.cs            # Destination data models
│   ├── ContactRequest.cs            # Contact form models
│   └── [Various other models]
├── Services/                 # Business logic and external integrations
│   ├── StrapiService.cs             # Strapi CMS integration
│   ├── EmailService.cs              # SendGrid email service
│   ├── TourService.cs               # Tour business logic
│   └── [Interface definitions]
├── Mappers/                  # AutoMapper configurations
├── Properties/               # Launch settings
├── appsettings.json          # Production configuration
├── appsettings.Development.json # Development configuration
└── Program.cs                # Application entry point
```

## 🛠️ Technologies Used

- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **C# 12** - Programming language
- **AutoMapper 14.0.0** - Object-to-object mapping
- **SendGrid 9.29.3** - Email service integration
- **Swagger/OpenAPI** - API documentation
- **Strapi CMS** - Headless content management system
- **Docker** - Containerization support

## 📱 API Endpoints

### Tours
- `GET /api/tours` - Get all tour packages
- `GET /api/tours/{slug}` - Get tour by slug
- `GET /api/tours/featured` - Get featured tours
- `GET /api/tours/card` - Get tour summary cards
- `GET /api/tours/card/featured` - Get featured tour cards

### Destinations
- `GET /api/destinations` - Get all destinations
- `GET /api/destinations/{slug}` - Get destination by slug
- `GET /api/destinations/featured` - Get featured destinations
- `GET /api/destinations/card` - Get destination summary cards
- `GET /api/destinations/card/featured` - Get featured destination cards

### Experiences
- `GET /api/experiences` - Get all experiences
- `GET /api/experiences/{slug}` - Get experience by slug
- `GET /api/experiences/featured` - Get featured experiences
- `GET /api/experiences/card` - Get experience summary cards
- `GET /api/experiences/card/featured` - Get featured experience cards

### Contact & Communication
- `POST /api/contact/send` - Submit dynamic contact forms
- `GET /api/inquiries` - Get all inquiries (admin)
- `POST /api/inquiries` - Submit new inquiry
- `POST /api/subscribers` - Newsletter subscription
- `GET /api/testimonials` - Get customer testimonials
- `POST /api/testimonials` - Submit new testimonial

## 🔧 Configuration

### Environment Settings

**Development** (`appsettings.Development.json`):
- Kestrel server on port 5001
- Enhanced logging for development
- HTTP protocol support

**Production** (`appsettings.json`):
- SendGrid email configuration
- Production logging levels
- CORS enabled for all origins

### External Integrations

**Strapi CMS**:
- Base URL: `https://graceful-happiness-10e3a700b4.strapiapp.com`
- Bearer token authentication
- Content types: tours, destinations, experiences

**SendGrid Email**:
- Transactional email service
- Template-based confirmations
- Contact form notifications

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git

### Local Development

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd BSLTours.API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the API**
   - Development: http://localhost:5001
   - Swagger Documentation: http://localhost:5001/swagger

### Docker Support

The project includes Docker support for containerized deployment:

```bash
# Build Docker image
docker build -t bsltours-api .

# Run container
docker run -p 80:80 bsltours-api
```

## 🏗️ Data Models

### Core Models

**TourDto**: Comprehensive tour information including:
- Basic details (name, slug, duration, pricing)
- Itinerary with daily activities
- Inclusions/exclusions
- Gallery images and hero image
- Related destinations
- Pricing tiers and add-ons
- Reviews and ratings

**DestinationDto**: Destination information including:
- Location details
- Attractions and features
- Image galleries
- Related tours and experiences

**ContactRequest**: Dynamic form handling with:
- Form type identification
- Custom field collections
- Email routing and templates

## 🔒 Security Features

- **CORS Configuration**: Allows cross-origin requests
- **Input Validation**: Model validation on all endpoints
- **Authentication**: Bearer token authentication for Strapi
- **Email Security**: Template-based email sending to prevent injection

## 📊 Monitoring & Observability

The application is prepared for observability with:
- Structured logging (Information level in production)
- OpenTelemetry integration points (commented out)
- ASP.NET Core instrumentation ready
- Request/response logging capabilities

## 🚢 Deployment

See the dedicated [Deployment Guide](./README-DEPLOYMENT.md) for detailed instructions on:
- IIS deployment on Windows hosting
- IONOS hosting configuration
- SSL certificate setup
- Frontend integration

### Production Considerations

- **Port Configuration**: Application is configured to listen on port 80 for DigitalOcean deployment
- **HTTPS Redirection**: Enabled for production security
- **Error Handling**: Comprehensive error responses
- **Performance**: AutoMapper for efficient object mapping

## 📝 Development Notes

### Key Patterns Used

1. **Dependency Injection**: All services registered in `Program.cs`
2. **AutoMapper**: Automatic mapping between models and DTOs
3. **Async/Await**: All controllers use async patterns for better performance
4. **RESTful Design**: Standard HTTP verbs and status codes
5. **Clean Architecture**: Separation of concerns across layers

### Extension Points

- **Authentication**: Ready for JWT or OAuth integration
- **Caching**: Can be added at service layer
- **Rate Limiting**: Can be implemented via middleware
- **Validation**: Extensible via FluentValidation
- **Logging**: Structured logging with Serilog possible

## 🤝 Contributing

1. Follow existing code patterns and conventions
2. Add appropriate unit tests for new features
3. Update API documentation for new endpoints
4. Ensure all endpoints return appropriate HTTP status codes
5. Use AutoMapper for model transformations

## 📄 License

This project is proprietary to BSL Tours / Siprea.

---

For deployment instructions, see [README-DEPLOYMENT.md](./README-DEPLOYMENT.md)