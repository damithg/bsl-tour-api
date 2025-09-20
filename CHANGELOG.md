# Changelog

All notable changes to the BSLTours.API project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Comprehensive project documentation
- Enhanced README with detailed architecture overview
- Complete API endpoint documentation

## [2.1.0] - 2024-07-09

### Added
- Enhanced Strapi service integration
- Improved error handling and logging
- Additional endpoint for card-based data retrieval

### Changed
- Updated service layer architecture
- Optimized data mapping strategies

## [2.0.0] - 2024-06-06

### Added
- **New Controllers**:
  - `ExperiencesController` - Activity-based experience management
  - Enhanced `DestinationsController` with featured destination support
  - Enhanced `ToursController` with card-based endpoints

### Changed
- **Breaking Changes**:
  - Updated API endpoint structure for better consistency
  - Modified response formats for tours and destinations
  - Enhanced DTO models for richer data representation

### Added
- **New Endpoints**:
  - `GET /api/tours/card` - Summary card format for tours
  - `GET /api/tours/card/featured` - Featured tour cards
  - `GET /api/destinations/card` - Summary card format for destinations
  - `GET /api/destinations/card/featured` - Featured destination cards
  - `GET /api/experiences` - Full experience listings
  - `GET /api/experiences/{slug}` - Individual experience details
  - `GET /api/experiences/featured` - Featured experiences
  - `GET /api/experiences/card` - Experience summary cards

### Improved
- Enhanced AutoMapper configurations
- Better separation of DTOs and domain models
- Improved Strapi CMS integration

## [1.8.0] - 2024-05-26

### Added
- **AutoMapper Integration**:
  - Automatic object-to-object mapping between models and DTOs
  - Configured mapping profiles for all major entities
  - Improved performance through efficient data transformation

### Changed
- **Model Architecture**:
  - Separated DTOs from domain models for better API design
  - Enhanced data structure for tours, destinations, and experiences
  - Improved relationship mapping between entities

### Added
- **New DTO Models**:
  - `SummaryCardDto` - Lightweight card representations
  - `CardDetailsDto` - Detailed card information
  - Enhanced image and gallery DTOs
  - Location and relationship DTOs

## [1.7.0] - 2024-05-18

### Added
- **Enhanced Configuration**:
  - Updated project file with latest dependencies
  - Improved build and deployment settings
  - Better development environment configuration

### Changed
- Optimized dependency injection configuration
- Enhanced logging and monitoring setup
- Improved application startup performance

## [1.6.0] - 2024-05-16

### Added
- **Email Service Enhancement**:
  - SendGrid integration with template support
  - Contact confirmation email automation
  - Dynamic form handling with custom fields
  - Enhanced email routing and notification system

### Added
- **New Features**:
  - `DynamicContactFormRequest` model for flexible form handling
  - Template-based email confirmations
  - Improved contact form processing with validation

### Changed
- Enhanced `ContactController` with dynamic form support
- Improved email service architecture
- Better error handling and validation

## [1.5.0] - 2024-04-30

### Added
- **Infrastructure Improvements**:
  - Docker support with Dockerfile
  - Docker ignore configuration
  - GitHub Actions workflow setup
  - Enhanced deployment pipeline

### Added
- **Development Tooling**:
  - `.dockerignore` for optimized container builds
  - Enhanced `.gitignore` for better version control
  - Development environment improvements

## [1.0.0] - 2024-04-12

### Added
- **Initial Release**:
  - Core API structure with ASP.NET Core 8.0
  - Basic CRUD operations for tours, destinations, and testimonials
  - Swagger/OpenAPI documentation
  - CORS configuration for cross-origin requests

### Added
- **Core Controllers**:
  - `ToursController` - Tour package management
  - `DestinationsController` - Destination information
  - `TestimonialsController` - Customer testimonials
  - `InquiriesController` - Customer inquiry handling
  - `SubscribersController` - Newsletter subscription management
  - `ContactController` - Basic contact form handling

### Added
- **Data Models**:
  - `TourPackage` and `TourDto` - Tour information structure
  - `Destination` and related models - Location information
  - `Testimonial` - Customer feedback structure
  - `Inquiry` - Customer inquiry model
  - `Subscriber` - Newsletter subscription model

### Added
- **Services**:
  - `TourService` - Tour business logic
  - `EmailService` - Basic email functionality
  - `IDataService` - Data access interface

### Added
- **Configuration**:
  - Production and development appsettings
  - Kestrel server configuration
  - Basic logging setup
  - Initial dependency injection configuration

## Architecture Evolution

### Phase 1: Foundation (v1.0.0)
- Basic REST API structure
- Simple CRUD operations
- In-memory data storage
- Basic email notifications

### Phase 2: CMS Integration (v1.5.0 - v1.7.0)
- Strapi CMS integration
- Enhanced data models
- Improved email services
- Docker containerization

### Phase 3: Enhanced Features (v2.0.0 - v2.1.0)
- Card-based data representations
- Experience management
- Featured content support
- AutoMapper integration
- Enhanced error handling

### Phase 4: Documentation & Optimization (Current)
- Comprehensive documentation
- Performance optimizations
- Enhanced developer experience
- Production-ready configuration

## Migration Notes

### Upgrading from v1.x to v2.x
- **Breaking Changes**: API endpoint structure has changed
- **New Dependencies**: AutoMapper is now required
- **Data Format**: Response formats have been enhanced with new DTOs
- **Configuration**: Review appsettings for new configuration options

### Key Improvements
- Better separation of concerns with DTOs
- Enhanced performance through AutoMapper
- More flexible endpoint structure
- Improved error handling and logging
- Better documentation and developer experience

---

**Note**: This changelog was created retroactively based on git history and project analysis. Future changes will be documented as they occur.