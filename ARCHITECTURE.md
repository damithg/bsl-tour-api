# BSLTours - Platform Architecture

## Overview

BSLTours is a comprehensive tour booking and management platform for Best Sri Lanka Tours. This solution is designed with a modular architecture to support multiple business domains including API services, communications, authentication, order processing, and more.

## Current Solution Structure

```
BSLTours/
├── BSLTours.sln                    # Main solution file
│
├── BSLTours.API/                   # Main Web API Project
│   ├── Controllers/                # API endpoints
│   ├── Services/                   # Business logic services
│   ├── Models/                     # Data models and DTOs
│   ├── Mappers/                    # AutoMapper profiles
│   ├── Properties/                 # Launch settings
│   ├── appsettings.json           # Configuration
│   └── Program.cs                 # Application entry point
│
└── Communications/                 # Email & Communication Services
    ├── BSLTours.Communications.Abstractions/
    │   ├── IEmailProvider.cs              # Provider abstraction
    │   ├── IEmailService.cs               # Service interface
    │   └── Models/
    │       ├── EmailAddress.cs
    │       ├── EmailMessage.cs
    │       ├── TemplatedEmailMessage.cs
    │       └── EmailResult.cs
    │
    ├── BSLTours.Communications.Core/
    │   ├── EmailService.cs                # Orchestration service
    │   ├── EmailServiceOptions.cs
    │   └── Extensions/
    │       └── ServiceCollectionExtensions.cs
    │
    ├── BSLTours.Communications.SendGrid/
    │   ├── SendGridEmailProvider.cs       # SendGrid implementation
    │   ├── SendGridOptions.cs
    │   └── Extensions/
    │       └── ServiceCollectionExtensions.cs
    │
    └── BSLTours.Communications.Postmark/
        ├── PostmarkEmailProvider.cs       # Postmark implementation
        ├── PostmarkOptions.cs
        └── Extensions/
            └── ServiceCollectionExtensions.cs
```

## Architectural Principles

### 1. **Separation of Concerns**
Each shared service (Communications, Authentication, Orders, etc.) is isolated into its own project structure with clear boundaries.

### 2. **Provider Pattern**
Shared services use the provider pattern to enable:
- Easy switching between implementations (e.g., SendGrid ↔ Mailgun)
- Testability through mocking
- Extensibility without modifying core logic

### 3. **Dependency Injection**
All services are registered via extension methods for clean, fluent configuration in `Program.cs`.

### 4. **Configuration-Based**
Services are configured through `appsettings.json` and environment variables, following the 12-factor app methodology.

## Current Modules

### Communications Module

**Purpose**: Handle all email and communication needs across the platform.

**Components**:
- **Abstractions**: Defines contracts (`IEmailProvider`, `IEmailService`) and models
- **Core**: Implements high-level orchestration logic
- **SendGrid**: SendGrid-specific implementation
- **Postmark**: Postmark-specific implementation

**Available Providers**:
1. **SendGrid** - Full-featured email service with templates
2. **Postmark** - Transactional email specialist with excellent deliverability

**Configuration-Driven Provider Selection**:

The email provider is selected via `appsettings.json` - **no code changes required** to switch providers!

```json
// appsettings.json
{
  "EmailService": {
    "Provider": "SendGrid",  // or "Postmark"
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  }
}
```

```csharp
// Program.cs - Dynamic provider registration (already configured!)
var emailProvider = builder.Configuration["EmailService:Provider"]?.ToLower();

switch (emailProvider)
{
    case "sendgrid":
        builder.Services.AddSendGridEmailProvider(/* ... */);
        break;
    case "postmark":
        builder.Services.AddPostmarkEmailProvider(/* ... */);
        break;
}

builder.Services.AddEmailService(builder.Configuration);
```

**Switching Providers**:
1. Change `"Provider"` value in `appsettings.json`
2. Set appropriate environment variable
3. Restart API - Done! No code changes needed.

**In Controllers** (same regardless of provider):
```csharp
public class ContactController : ControllerBase
{
    private readonly IEmailService _emailService;

    public async Task SendEmail()
    {
        await _emailService.SendEmailAsync(
            toEmail: "customer@example.com",
            subject: "Welcome",
            htmlContent: "<p>Welcome to BSL Tours!</p>"
        );
    }
}
```

**Future Providers**:
- `BSLTours.Communications.Mailgun/` - Mailgun implementation
- `BSLTours.Communications.AwsSes/` - AWS SES implementation
- `BSLTours.Communications.Resend/` - Resend implementation

## Planned Modules

### Authentication Module

**Purpose**: Handle user authentication, authorization, and identity management.

**Proposed Structure**:
```
Authentication/
├── BSLTours.Authentication.Abstractions/
│   ├── IAuthenticationProvider.cs
│   ├── ITokenService.cs
│   └── Models/
│       ├── AuthUser.cs
│       ├── AuthToken.cs
│       └── AuthResult.cs
│
├── BSLTours.Authentication.Core/
│   ├── AuthenticationService.cs
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs
│
└── BSLTours.Authentication.Jwt/
    ├── JwtAuthenticationProvider.cs
    ├── JwtOptions.cs
    └── Extensions/
        └── ServiceCollectionExtensions.cs
```

**Future Providers**:
- JWT (JSON Web Tokens)
- OAuth 2.0 / OpenID Connect
- Azure AD B2C
- Firebase Authentication

### Orders Module

**Purpose**: Handle tour bookings, payments, and order management.

**Proposed Structure**:
```
Orders/
├── BSLTours.Orders.Abstractions/
│   ├── IOrderService.cs
│   ├── IPaymentProvider.cs
│   └── Models/
│       ├── Order.cs
│       ├── Payment.cs
│       └── OrderStatus.cs
│
├── BSLTours.Orders.Core/
│   ├── OrderService.cs
│   ├── OrderOrchestrator.cs
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs
│
└── BSLTours.Orders.Payment/
    ├── StripePaymentProvider.cs
    ├── PayPalPaymentProvider.cs
    └── Extensions/
        └── ServiceCollectionExtensions.cs
```

**Features**:
- Order creation and tracking
- Payment processing integration
- Booking confirmations
- Invoice generation

### Additional Planned Modules

1. **Notifications** (`BSLTours.Notifications.*`)
   - Push notifications
   - SMS messaging
   - In-app notifications

2. **Storage** (`BSLTours.Storage.*`)
   - File uploads
   - Image processing
   - Cloud storage (AWS S3, Azure Blob)

3. **Analytics** (`BSLTours.Analytics.*`)
   - User behavior tracking
   - Business intelligence
   - Reporting

4. **CMS Integration** (`BSLTours.Cms.*`)
   - Strapi integration (current)
   - Contentful (future)
   - Custom CMS adapters

## Naming Conventions

### Projects
- **Pattern**: `BSLTours.{Module}.{Component}`
- **Examples**:
  - `BSLTours.Communications.SendGrid`
  - `BSLTours.Authentication.Jwt`
  - `BSLTours.Orders.Payment`

### Interfaces
- **Pattern**: `I{Functionality}{Type}`
- **Examples**:
  - `IEmailProvider` - Provider abstraction
  - `IEmailService` - High-level service
  - `IAuthenticationProvider` - Auth provider abstraction

### Configuration
- **Pattern**: `{Module}Options` or `{Provider}Options`
- **Examples**:
  - `EmailServiceOptions`
  - `SendGridOptions`
  - `JwtOptions`

## Dependency Flow

```
BSLTours.API
    ↓
    ├──→ Communications.Core ──→ Communications.Abstractions
    │           ↓
    │     Communications.SendGrid ──→ Communications.Abstractions
    │
    ├──→ Authentication.Core ──→ Authentication.Abstractions
    │           ↓
    │     Authentication.Jwt ──→ Authentication.Abstractions
    │
    └──→ Orders.Core ──→ Orders.Abstractions
                ↓
          Orders.Payment ──→ Orders.Abstractions
```

**Key Principle**: The API project only references:
- `*.Abstractions` - For interfaces and models
- `*.Core` - For orchestration logic
- Specific providers (e.g., `*.SendGrid`) - For DI registration only

## Configuration Structure

### appsettings.json
```json
{
  "EmailService": {
    "Provider": "SendGrid",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours",
    "ContactConfirmationTemplateId": "d-344abfe0fe13466b8060f5c046e9f96a"
  },
  "SendGrid": {
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  },
  "Postmark": {
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  },
  "Authentication": {
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "Orders": {
    "DefaultCurrency": "USD",
    "PaymentProvider": "Stripe"
  }
}
```

### Environment Variables
Sensitive data (API keys, secrets) should be stored in environment variables:
- `SendGridApiKey` - SendGrid API key (if using SendGrid)
- `PostmarkServerToken` - Postmark Server API Token (if using Postmark)
- `StripeApiKey` - Stripe API key (future)
- `JwtSecretKey` - JWT secret key (future)
- `DatabaseConnectionString` - Database connection string (future)

## Testing Strategy

### Unit Tests
```
BSLTours.Communications.Tests/
├── Abstractions.Tests/
├── Core.Tests/
└── SendGrid.Tests/
```

### Integration Tests
```
BSLTours.API.IntegrationTests/
├── Controllers/
├── Services/
└── Infrastructure/
```

## Adding a New Module

To add a new module (e.g., Notifications):

1. **Create Projects**:
   ```bash
   dotnet new classlib -n BSLTours.Notifications.Abstractions
   dotnet new classlib -n BSLTours.Notifications.Core
   dotnet new classlib -n BSLTours.Notifications.Firebase
   ```

2. **Define Abstractions**:
   - Create `INotificationProvider.cs`
   - Create `INotificationService.cs`
   - Create models (NotificationMessage, NotificationResult, etc.)

3. **Implement Core**:
   - Create `NotificationService.cs`
   - Add dependency injection extensions

4. **Implement Provider**:
   - Create provider-specific implementation (e.g., `FirebaseNotificationProvider.cs`)
   - Add provider options
   - Add dependency injection extensions

5. **Register in API**:
   ```csharp
   builder.Services.AddFirebaseNotificationProvider(options => { /* config */ });
   builder.Services.AddNotificationService(builder.Configuration);
   ```

## Migration Guide

### From Monolithic to Modular

When extracting functionality into a new module:

1. Identify the feature to extract (e.g., email functionality)
2. Create the module structure (Abstractions, Core, Provider)
3. Move interfaces and models to Abstractions
4. Move business logic to Core
5. Move provider-specific code to Provider project
6. Update API project references
7. Update dependency injection configuration
8. Test thoroughly

**Example**: The Communications module was extracted following this pattern.

## Best Practices

1. **Keep Abstractions Clean**: No dependencies on external libraries
2. **Provider Independence**: Providers should only depend on Abstractions
3. **Configuration Over Code**: Use options pattern for all configuration
4. **Async All The Way**: All I/O operations should be async
5. **Logging**: Use `ILogger<T>` for structured logging
6. **Error Handling**: Return result objects instead of throwing exceptions where appropriate

## Future Roadmap

### Phase 1 (Current)
- ✅ Communications module with SendGrid

### Phase 2
- Authentication module with JWT
- User management
- Role-based access control

### Phase 3
- Orders module with payment integration
- Booking management
- Invoice generation

### Phase 4
- Notifications (Push, SMS)
- Analytics and reporting
- Advanced CMS features

### Phase 5
- Microservices migration (optional)
- Message queue integration
- Caching layer
- API Gateway

## Support & Documentation

- **API Documentation**: Available via Swagger at `/swagger`
- **Postman Collection**: See `BSLTours-API-Collection.postman_collection.json`
- **Deployment Guide**: See `README-DEPLOYMENT.md`
- **Changelog**: See `CHANGELOG.md`

## Contributing

When adding new features:
1. Follow the module structure outlined above
2. Update this document with new modules
3. Add appropriate unit and integration tests
4. Update Swagger documentation
5. Update Postman collection

---

**Last Updated**: October 2025
**Version**: 1.0.0
**Maintained By**: BSLTours Development Team
