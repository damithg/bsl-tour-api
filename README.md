# BSLTours API

Modern, modular REST API for Best Sri Lanka Tours - a comprehensive tour booking and management platform.

## 🏗️ Architecture

This project uses a **modular, provider-based architecture** that separates concerns and allows easy swapping of service implementations without changing business logic.

```
BSLTours/
├── BSLTours.API/                    # Main Web API
├── Communications/                   # Email & Communication Module
│   ├── BSLTours.Communications.Abstractions/    # Interfaces & Models
│   ├── BSLTours.Communications.Core/            # Orchestration Layer
│   ├── BSLTours.Communications.SendGrid/        # SendGrid Implementation
│   └── BSLTours.Communications.Postmark/        # Postmark Implementation
├── ARCHITECTURE.md                  # Detailed architecture documentation
└── BSLTours.sln                     # Solution file
```

See [ARCHITECTURE.md](ARCHITECTURE.md) for complete architectural details.

## 🚀 Quick Start

### Prerequisites

- **.NET 8.0 SDK** or later
- **Email Provider Account** (SendGrid or Postmark)
- **IDE**: Visual Studio 2022, VS Code, or Rider

### 1. Clone & Build

```bash
git clone <repository-url>
cd bsl-tours-api
dotnet restore
dotnet build
```

### 2. Configure Email Provider

The API uses a **configuration-driven approach** - no code changes needed to switch email providers!

#### Option A: Use SendGrid

**Update `appsettings.json`:**
```json
{
  "EmailService": {
    "Provider": "SendGrid",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  }
}
```

**Set environment variable:**
```powershell
# Windows PowerShell
$env:SendGridApiKey = "SG.your-sendgrid-api-key-here"

# Linux/Mac
export SendGridApiKey="SG.your-sendgrid-api-key-here"
```

**Get SendGrid API Key:**
1. Sign up at https://sendgrid.com
2. Go to Settings → API Keys
3. Create API key with "Mail Send" permissions

#### Option B: Use Postmark

**Update `appsettings.json`:**
```json
{
  "EmailService": {
    "Provider": "Postmark",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  }
}
```

**Set environment variable:**
```powershell
# Windows PowerShell
$env:PostmarkServerToken = "your-postmark-server-token-here"

# Linux/Mac
export PostmarkServerToken="your-postmark-server-token-here"
```

**Get Postmark Server Token:**
1. Sign up at https://postmarkapp.com (free: 100 emails/month)
2. Go to Servers → Select server → API Tokens
3. Copy your Server API Token
4. **Important**: Verify sender signature in Postmark dashboard!

### 3. Run the API

```bash
cd BSLTours.API
dotnet run
```

The API will start at `http://localhost:80`

### 4. Test the API

**Using Swagger UI:**
- Open browser: `http://localhost:80/swagger`
- Try the `/api/inquiries/comprehensive` endpoint

**Using PowerShell:**
```powershell
$json = Get-Content test-comprehensive-inquiry.json -Raw
Invoke-WebRequest -Uri "http://localhost:80/api/inquiries/comprehensive" `
  -Method POST `
  -ContentType "application/json" `
  -Body $json
```

## 📧 Email Provider Setup

### Switching Providers

**No code changes required!** Just update configuration and restart:

1. Edit `appsettings.json` → Change `"Provider"` value
2. Set appropriate environment variable
3. Restart API

See [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md) for detailed guide.

### Troubleshooting Email Issues

Run the diagnostic script:
```powershell
.\check-postmark-setup.ps1
```

**Common Issues:**

| Error | Solution |
|-------|----------|
| "SendGridApiKey environment variable is not set" | Set env var: `$env:SendGridApiKey = "your-key"` |
| "PostmarkServerToken environment variable is not set" | Set env var: `$env:PostmarkServerToken = "your-token"` |
| "Request does not contain a valid Server token" | Check token is correct & verify sender signature in Postmark |
| "Sender signature not verified" | Go to Postmark dashboard → Sender Signatures → Verify email |

## 📋 Available Endpoints

### Tours
- **GET** `/api/tours` - Get all tours
- **GET** `/api/tours/{slug}` - Get tour by slug
- **GET** `/api/tours/featured` - Get featured tours
- **GET** `/api/tours/card` - Get tour cards (summary)
- **GET** `/api/tours/card/featured` - Get featured tour cards

### Destinations
- **GET** `/api/destinations` - Get all destinations
- **GET** `/api/destinations/{slug}` - Get destination by slug
- **GET** `/api/destinations/featured` - Get featured destinations
- **GET** `/api/destinations/card` - Get destination cards
- **GET** `/api/destinations/card/featured` - Get featured destination cards

### Experiences
- **GET** `/api/experiences` - Get all experiences
- **GET** `/api/experiences/{slug}` - Get experience by slug
- **GET** `/api/experiences/featured` - Get featured experiences
- **GET** `/api/experiences/card` - Get experience cards
- **GET** `/api/experiences/card/featured` - Get featured experience cards

### Contact
- **POST** `/api/contact/send` - Send contact form (dynamic fields)

### Inquiries
- **POST** `/api/inquiries` - Create legacy inquiry
- **POST** `/api/inquiries/comprehensive` - Create comprehensive inquiry (recommended)

See [TESTING.md](TESTING.md) for request/response examples and testing guide.

## 🧪 Testing

### Run Diagnostic Check

```powershell
.\check-postmark-setup.ps1
```

### Test Comprehensive Inquiry Endpoint

A sample test file is provided:

```powershell
cd BSLTours.API
dotnet run

# In another terminal:
cd ..
$json = Get-Content test-comprehensive-inquiry.json -Raw
Invoke-WebRequest -Uri "http://localhost:80/api/inquiries/comprehensive" `
  -Method POST `
  -ContentType "application/json" `
  -Body $json
```

**Expected Response:**
```json
{
  "message": "Comprehensive inquiry submitted successfully"
}
```

**Verify Email Sent:**
- Check console logs for "Email sent successfully"
- Check recipient inbox (`info@siprea.com`)
- Check provider dashboard (SendGrid Activity / Postmark Activity)

## 🛠️ Configuration

### appsettings.json Structure

```json
{
  "EmailService": {
    "Provider": "SendGrid",              // "SendGrid" or "Postmark"
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours",
    "ContactConfirmationTemplateId": "d-xxx"
  },
  "SendGrid": {
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  },
  "Postmark": {
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  },
  "Turnstile": {
    "SecretKey": "your-cloudflare-turnstile-secret"
  }
}
```

### Environment Variables

Required environment variables based on selected provider:

| Provider | Environment Variable | Where to Get It |
|----------|---------------------|-----------------|
| SendGrid | `SendGridApiKey` | SendGrid Dashboard → Settings → API Keys |
| Postmark | `PostmarkServerToken` | Postmark Dashboard → Servers → API Tokens |

**Security Best Practice:** Never commit API keys to source control. Use environment variables or secret management tools.

### Environment-Specific Configuration

Use environment-specific config files:

- `appsettings.Development.json` - Local development
- `appsettings.Staging.json` - Staging environment
- `appsettings.Production.json` - Production environment

Example:
```json
// appsettings.Production.json
{
  "EmailService": {
    "Provider": "Postmark"  // Use Postmark in production
  }
}
```

## 📚 Documentation

- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Complete architectural overview, design patterns, and roadmap
- **[PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md)** - Email provider switching guide
- **[TESTING.md](TESTING.md)** - Testing guide for all endpoints and methods
- **[POSTMARK-TESTING.md](POSTMARK-TESTING.md)** - Postmark-specific testing guide

## 🏢 Project Structure

### BSLTours.API
Main web API project containing:
- **Controllers/** - API endpoints
- **Services/** - Business logic (Strapi, Tours, Turnstile)
- **Models/** - Data models and DTOs
- **Mappers/** - AutoMapper profiles

### Communications Module

**Provider Pattern Implementation:**

```
Communications/
├── Abstractions/           # Interfaces all providers must implement
│   ├── IEmailProvider.cs
│   ├── IEmailService.cs
│   └── Models/
├── Core/                   # Orchestration layer
│   └── EmailService.cs
├── SendGrid/               # SendGrid implementation
│   └── SendGridEmailProvider.cs
└── Postmark/               # Postmark implementation
    └── PostmarkEmailProvider.cs
```

**Key Benefits:**
- ✅ Switch providers via configuration only
- ✅ Zero changes to controllers/business logic
- ✅ Easy to add new providers (Mailgun, AWS SES, etc.)
- ✅ Testable and maintainable

## 🔧 Development

### Adding a New Email Provider

Want to add Mailgun, AWS SES, or another provider?

1. **Create provider project:**
   ```bash
   dotnet new classlib -n BSLTours.Communications.Mailgun -f net8.0
   ```

2. **Implement IEmailProvider:**
   ```csharp
   public class MailgunEmailProvider : IEmailProvider
   {
       public async Task<EmailResult> SendEmailAsync(EmailMessage message, CancellationToken ct)
       {
           // Implementation
       }
   }
   ```

3. **Add to Program.cs switch:**
   ```csharp
   case "mailgun":
       builder.Services.AddMailgunEmailProvider(/* ... */);
       break;
   ```

4. **Done!** No changes to controllers needed.

### Running in Development

```bash
# Watch mode (auto-reload on changes)
dotnet watch run

# Specific environment
dotnet run --environment Staging
```

## 🚢 Deployment

### DigitalOcean App Platform (Current)

The API is deployed on DigitalOcean App Platform using Docker.

**Important Configuration:**
- **Source Directory**: Leave empty (repo root) - the Dockerfile is at the root level
- **Dockerfile Path**: `/Dockerfile`

**Required Environment Variables** (set in DigitalOcean App Settings):
- `PostmarkServerToken` - Your Postmark API token
- `SendGridApiKey` - Your SendGrid API key (if using SendGrid)
- `Turnstile__SecretKey` - Cloudflare Turnstile secret (for CAPTCHA)

**Build Process:**
The root `Dockerfile` handles the multi-project build:
1. Copies solution file and all project files
2. Restores NuGet packages
3. Builds and publishes `BSLTours.API`
4. Creates runtime image

### Environment Variables for Production

Set these in your hosting environment:

**DigitalOcean App Platform:**
1. Go to Apps → Your App → Settings
2. Click on your component → Environment Variables
3. Add required variables (mark sensitive ones as encrypted)

**Azure App Service:**
```bash
az webapp config appsettings set --name your-app --resource-group your-rg \
  --settings EmailService__Provider="Postmark" \
             PostmarkServerToken="your-token"
```

**Docker:**
```bash
docker run -e EmailService__Provider=Postmark \
           -e PostmarkServerToken=your-token \
           your-api-image
```

**Kubernetes:**
```yaml
env:
  - name: EmailService__Provider
    value: "Postmark"
  - name: PostmarkServerToken
    valueFrom:
      secretKeyRef:
        name: email-secrets
        key: postmark-token
```

### Build for Production

```bash
dotnet publish BSLTours.API/BSLTours.API.csproj -c Release -o ./publish
```

## 🤝 Contributing

When adding new features:
1. Follow the modular architecture pattern
2. Use dependency injection
3. Add to solution file: `dotnet sln add YourProject.csproj`
4. Update relevant documentation
5. Add tests (future)

## 📝 API Documentation

When the API is running, access interactive API documentation:
- **Swagger UI**: `http://localhost:80/swagger`

## 🔐 Security Notes

- **API Keys**: Store in environment variables, never in source control
- **Turnstile**: CAPTCHA protection on inquiry endpoints
- **CORS**: Currently allows all origins (configure for production)
- **HTTPS**: Enable HTTPS redirection in production

## 🆘 Getting Help

### Quick Diagnostic

```powershell
# Check email provider setup
.\check-postmark-setup.ps1

# View API logs
dotnet run --verbosity detailed
```

### Common Questions

**Q: How do I switch from SendGrid to Postmark?**
A: Change `"Provider"` in `appsettings.json`, set environment variable, restart. See [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md).

**Q: Why am I getting "Server token invalid"?**
A: Run `.\check-postmark-setup.ps1` to diagnose. Usually means env var not set or sender signature not verified.

**Q: Can I use different providers in dev vs production?**
A: Yes! Use `appsettings.Development.json` and `appsettings.Production.json`.

**Q: How do I add a new provider like Mailgun?**
A: See "Adding a New Email Provider" section above.

## 📦 Dependencies

### Main API
- ASP.NET Core 8.0
- AutoMapper
- Swashbuckle (Swagger)

### Communications Module
- SendGrid (v9.29.3)
- Postmark (v5.2.0)
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Logging

## 📄 License

[Your License Here]

## 🎯 Roadmap

See [ARCHITECTURE.md](ARCHITECTURE.md) for detailed roadmap.

### Current Phase
- ✅ Communications module with SendGrid
- ✅ Communications module with Postmark
- ✅ Configuration-driven provider selection

### Planned
- Authentication module (JWT)
- Orders/Payment module
- Notifications (Push, SMS)
- Storage module (file uploads)

---

## 📝 Developer Notes

### Repository Structure (February 2025)

The git repository was restructured to properly include the Communications module:

```
bsl-tours-api/              ← Git repo root
├── .git/
├── Dockerfile              ← Root-level Dockerfile for multi-project build
├── BSLTours.sln
├── BSLTours.API/           ← Main API project
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   └── ...
├── Communications/         ← Email provider projects (sibling to API)
│   ├── BSLTours.Communications.Abstractions/
│   ├── BSLTours.Communications.Core/
│   ├── BSLTours.Communications.SendGrid/
│   └── BSLTours.Communications.Postmark/
└── docs & config files...
```

**Key Points:**
- The `Communications` folder is a **sibling** to `BSLTours.API`, not nested inside it
- Project references use `../Communications/...` paths
- The root `Dockerfile` handles building both API and Communications projects
- `launchSettings.json` is gitignored (contains local API keys)

### API Keys & Secrets

**Never commit API keys to git.** Use:
- Environment variables for local development
- DigitalOcean environment variables for production
- `launchSettings.json` is gitignored for local secrets

### Postman Collection

The Postman collection (`BSLTours.API/BSLTours-API-Collection.postman_collection.json`) is kept in sync with actual API endpoints. Import it into Postman for testing.

### Current Email Configuration

- **Production**: Postmark (configured via `PostmarkServerToken` env var)
- **Admin notifications**: Sent to `info@bestsrilankatours.com`
- **Contact form notifications**: Sent to `info@siprea.com`
- **Auto-reply template**: Postmark template ID `41894431`

---

**Version**: 1.1.0
**Last Updated**: February 2025
**Maintained By**: BSLTours Development Team

For detailed architecture and design decisions, see [ARCHITECTURE.md](ARCHITECTURE.md).
