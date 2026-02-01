# Email Provider Switching Guide

## Enterprise-Grade Configuration

The email provider is now **configuration-driven** - no code changes required! Simply update `appsettings.json` to switch between providers.

## How to Switch Providers

### Switch to SendGrid

**1. Update `appsettings.json`:**
```json
{
  "EmailService": {
    "Provider": "SendGrid",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours",
    "ContactConfirmationTemplateId": "d-344abfe0fe13466b8060f5c046e9f96a"
  }
}
```

**2. Set environment variable:**
```powershell
$env:SendGridApiKey = "your-sendgrid-api-key"
```

**3. Restart your API - Done!**

### Switch to Postmark

**1. Update `appsettings.json`:**
```json
{
  "EmailService": {
    "Provider": "Postmark",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours",
    "ContactConfirmationTemplateId": "d-344abfe0fe13466b8060f5c046e9f96a"
  }
}
```

**2. Set environment variable:**
```powershell
$env:PostmarkServerToken = "your-postmark-server-token"
```

**3. Restart your API - Done!**

## Environment-Specific Configuration

You can have different providers for different environments using `appsettings.{Environment}.json`:

### appsettings.Development.json
```json
{
  "EmailService": {
    "Provider": "SendGrid"
  }
}
```

### appsettings.Production.json
```json
{
  "EmailService": {
    "Provider": "Postmark"
  }
}
```

### appsettings.Staging.json
```json
{
  "EmailService": {
    "Provider": "SendGrid"
  }
}
```

## Configuration via Environment Variables (Docker/Cloud)

For containerized deployments, you can override the provider using environment variables:

```bash
# Docker
docker run -e EmailService__Provider=Postmark \
           -e PostmarkServerToken=your-token \
           your-api-image

# Kubernetes ConfigMap
apiVersion: v1
kind: ConfigMap
metadata:
  name: api-config
data:
  EmailService__Provider: "Postmark"
```

## Supported Providers

| Provider Name | Configuration Value | Environment Variable Required |
|---------------|---------------------|-------------------------------|
| SendGrid      | `"SendGrid"`        | `SendGridApiKey`              |
| Postmark      | `"Postmark"`        | `PostmarkServerToken`         |

*Provider names are case-insensitive*

## What Happens Under the Hood

When your API starts, it reads the `EmailService:Provider` value from configuration and dynamically registers the appropriate email provider implementation:

```csharp
// Program.cs (you don't need to modify this!)
var emailProvider = builder.Configuration["EmailService:Provider"]?.ToLower()
    ?? throw new InvalidOperationException("EmailService:Provider is not configured");

switch (emailProvider)
{
    case "sendgrid":
        builder.Services.AddSendGridEmailProvider(/* ... */);
        break;
    case "postmark":
        builder.Services.AddPostmarkEmailProvider(/* ... */);
        break;
    default:
        throw new InvalidOperationException(
            $"Unknown email provider '{emailProvider}'. Supported: SendGrid, Postmark");
}
```

## Testing Provider Switching

### Test 1: Verify SendGrid

```bash
# 1. Update appsettings.json to use SendGrid
# 2. Set API key
$env:SendGridApiKey = "your-key"
# 3. Run API
dotnet run
# 4. Send test request - should use SendGrid
```

### Test 2: Switch to Postmark

```bash
# 1. Update appsettings.json to use Postmark
# 2. Set server token
$env:PostmarkServerToken = "your-token"
# 3. Run API
dotnet run
# 4. Send test request - should use Postmark
```

## Error Handling

### Error: "EmailService:Provider is not configured in appsettings.json"

**Cause**: The `Provider` field is missing from `appsettings.json`

**Solution**: Add the `Provider` field to your `EmailService` configuration:
```json
{
  "EmailService": {
    "Provider": "SendGrid"
  }
}
```

### Error: "Unknown email provider 'xyz'"

**Cause**: The provider name in configuration doesn't match a supported provider

**Solution**: Use one of the supported values:
- `"SendGrid"`
- `"Postmark"`

### Error: "SendGridApiKey environment variable is not set"

**Cause**: You selected SendGrid but didn't set the API key

**Solution**: Set the environment variable before starting the API:
```powershell
$env:SendGridApiKey = "your-api-key"
```

### Error: "PostmarkServerToken environment variable is not set"

**Cause**: You selected Postmark but didn't set the server token

**Solution**: Set the environment variable before starting the API:
```powershell
$env:PostmarkServerToken = "your-server-token"
```

## Adding a New Provider (Future)

When adding a new provider (e.g., Mailgun, AWS SES), you only need to:

1. Create the provider implementation in `Communications/BSLTours.Communications.{Provider}/`
2. Add a case to the switch statement in `Program.cs`
3. Document the environment variable required
4. Update this guide

**No changes to controllers or business logic required!** ✅

## Best Practices

1. **Development**: Use SendGrid or Postmark based on your preference
2. **Staging**: Match your production provider for realistic testing
3. **Production**: Choose based on your needs:
   - **Postmark**: Best for transactional emails, excellent deliverability
   - **SendGrid**: Good for both transactional and marketing emails

4. **Configuration Management**:
   - Store API keys in environment variables (never in appsettings.json)
   - Use separate API keys for dev/staging/production
   - Rotate keys regularly
   - Use secret management tools (Azure Key Vault, AWS Secrets Manager) in production

## Architecture Benefits

✅ **Zero Code Changes** - Switch providers via config only
✅ **Environment-Specific** - Different providers per environment
✅ **Runtime Selection** - Dynamic provider registration
✅ **Fail-Fast** - Clear error messages if misconfigured
✅ **Extensible** - Easy to add new providers
✅ **Testable** - Controllers are provider-agnostic

---

**Last Updated**: October 2025
**Current Design**: Configuration-driven provider selection
