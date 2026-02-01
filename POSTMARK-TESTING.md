# Testing with Postmark Provider

Your API is now configured to use **Postmark** as the email provider! Here's how to test it.

## Step 1: Get Your Postmark Server Token

1. **Sign up for Postmark** (if you haven't already):
   - Go to https://postmarkapp.com/
   - Create a free account (includes 100 emails/month)

2. **Get your Server API Token**:
   - Log in to Postmark dashboard
   - Go to **Servers** → Select your server (or create one)
   - Go to **API Tokens** tab
   - Copy your **Server API Token**

3. **Verify sender signature**:
   - Go to **Sender Signatures** in Postmark
   - Add and verify `info@siprea.com` (or your from email)
   - Check your email and click the verification link

## Step 2: Set Environment Variable

**Windows PowerShell:**
```powershell
$env:PostmarkServerToken = "your-postmark-server-token-here"
```

**Windows CMD:**
```cmd
set PostmarkServerToken=your-postmark-server-token-here
```

**Linux/Mac:**
```bash
export PostmarkServerToken=your-postmark-server-token-here
```

## Step 3: Start the API

```bash
cd C:\projects\current-work\bsl-tours-api\BSLTours.API
dotnet run
```

The API will start on `http://localhost:80`

## Step 4: Test the Endpoint

### Option A: Using PowerShell (Recommended for Windows)

```powershell
# Navigate to project root
cd C:\projects\current-work\bsl-tours-api

# Load the test JSON and send request
$json = Get-Content test-comprehensive-inquiry.json -Raw
$response = Invoke-WebRequest -Uri "http://localhost:80/api/inquiries/comprehensive" `
  -Method POST `
  -ContentType "application/json" `
  -Body $json

# View response
$response.Content
```

### Option B: Using Swagger UI

1. Open browser: `http://localhost:80/swagger`
2. Find `/api/inquiries/comprehensive` endpoint
3. Click **"Try it out"**
4. Paste this JSON:

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "+1-555-123-4567",
  "inquiryType": "tour",
  "subjectName": "7-Day Cultural Heritage Tour",
  "subjectId": 123,
  "travelPlanning": {
    "flexibleDates": false,
    "travelMonth": "December 2025",
    "travelDates": "2025-12-15 to 2025-12-22",
    "adults": 2,
    "children": 1
  },
  "message": "We are interested in booking a cultural heritage tour.",
  "hearAboutUs": "Google Search",
  "subscribed": true
}
```

5. Click **"Execute"**

### Option C: Using cURL

```bash
curl -X POST http://localhost:80/api/inquiries/comprehensive \
  -H "Content-Type: application/json" \
  -d @test-comprehensive-inquiry.json
```

## What to Verify

### 1. API Response
✅ **Status Code**: `200 OK`
✅ **Response Body**:
```json
{
  "message": "Comprehensive inquiry submitted successfully"
}
```

### 2. Console Output
Look for this in your API console:
```
info: BSLTours.Communications.Postmark.PostmarkEmailProvider[0]
      Email sent successfully via Postmark. MessageID: xxxxx-xxxxx-xxxxx
```

### 3. Email Received
- Check inbox at `info@siprea.com`
- Email should arrive within seconds
- Subject: "New tour Inquiry from John Doe"
- Body should contain all inquiry details formatted as HTML

### 4. Postmark Dashboard
- Go to Postmark dashboard → **Activity**
- You should see the sent email listed
- Click to view delivery status and email content

## Differences Between Postmark and SendGrid

| Feature | Postmark | SendGrid |
|---------|----------|----------|
| **Template ID Format** | Numeric (e.g., `12345`) | String (e.g., `d-xxx`) |
| **Multiple Recipients** | Single per API call* | Multiple supported |
| **Default Port** | N/A (API only) | N/A (API only) |
| **Dashboard** | https://postmarkapp.com | https://app.sendgrid.com |
| **Free Tier** | 100 emails/month | 100 emails/day |
| **Specialization** | Transactional emails | Bulk + Transactional |

*Postmark's standard send supports one primary recipient. Use batch API for multiple recipients.

## Switching Back to SendGrid

If you want to switch back to SendGrid:

1. **Edit Program.cs** (lines 23-40):

```csharp
// Option 1: SendGrid Provider (Currently Active)
builder.Services.AddSendGridEmailProvider(options =>
{
    options.ApiKey = Environment.GetEnvironmentVariable("SendGridApiKey")
        ?? throw new InvalidOperationException("SendGridApiKey environment variable is not set");
    options.DefaultFromEmail = builder.Configuration["SendGrid:DefaultFromEmail"];
    options.DefaultFromName = builder.Configuration["SendGrid:DefaultFromName"];
});

// Option 2: Postmark Provider
//builder.Services.AddPostmarkEmailProvider(options =>
//{
//    options.ServerToken = Environment.GetEnvironmentVariable("PostmarkServerToken")
//        ?? throw new InvalidOperationException("PostmarkServerToken environment variable is not set");
//    options.DefaultFromEmail = builder.Configuration["Postmark:DefaultFromEmail"];
//    options.DefaultFromName = builder.Configuration["Postmark:DefaultFromName"];
//});

builder.Services.AddEmailService(builder.Configuration);
```

2. **Set SendGrid environment variable**:
```powershell
$env:SendGridApiKey = "your-sendgrid-api-key"
```

3. **Restart the API**

## Troubleshooting

### Error: "PostmarkServerToken environment variable is not set"

**Solution**: Make sure you've set the environment variable in the same terminal where you run `dotnet run`.

```powershell
# Set it first
$env:PostmarkServerToken = "your-token-here"

# Then run
dotnet run
```

### Error: "Sender signature not verified"

**Solution**:
1. Go to Postmark dashboard → Sender Signatures
2. Verify the email address you're using as "from" email
3. Check your email inbox and click the verification link

### Error: "Invalid Postmark template ID"

**Solution**: Postmark template IDs must be numeric. If you're using SendGrid-style template IDs (e.g., `d-xxx`), you need to:
1. Create a template in Postmark dashboard
2. Get the numeric template ID
3. Update your code to use the numeric ID

### Email not received

**Solution**:
1. Check Postmark dashboard → Activity
2. Look for the email send status
3. Check spam folder
4. Verify sender signature is confirmed
5. Check bounce/complaint logs in Postmark

### Warning about single recipient

If you see this warning:
```
Postmark standard send only supports single recipient. Sending to first recipient only.
```

**This is expected behavior**. Postmark's standard `SendMessage` API only supports one primary recipient. The email will still send successfully to the first recipient in the list. For multiple recipients, you'd need to use Postmark's batch send API (not implemented in current version).

## Testing with Templates

To test templated emails with Postmark:

1. **Create a template in Postmark**:
   - Go to Templates → Create Template
   - Design your template
   - Note the template ID (numeric, e.g., `12345`)

2. **Send using template**:
```csharp
await _emailService.SendTemplatedEmailAsync(
    toEmail: "customer@example.com",
    templateId: "12345",  // Postmark template ID (numeric)
    templateData: new Dictionary<string, object>
    {
        { "name", "John Doe" },
        { "submitted_date", "October 25, 2025" }
    }
);
```

## Current Configuration Files

- **Program.cs**: Lines 32-39 - Postmark provider registration
- **appsettings.json**: `Postmark` section with default from email/name
- **InquiriesController.cs**: Line 206 - Recipient email (`info@siprea.com`)

## Next Steps

1. ✅ Test basic email sending with Postmark
2. Create templates in Postmark dashboard (if using templated emails)
3. Update template IDs in code to use Postmark numeric IDs
4. Monitor email deliverability in Postmark Activity dashboard
5. Consider implementing batch send for multiple recipients

---

**Provider Comparison at a Glance:**

| Aspect | Your Choice |
|--------|-------------|
| Currently Active | ✅ **Postmark** |
| Alternative | SendGrid (commented out in Program.cs) |
| Switch Method | Edit Program.cs, toggle comments |
| Controllers | ✅ No changes needed (uses IEmailService) |

The beauty of your architecture is that **switching providers requires zero changes to your controllers or business logic!** Just toggle the provider registration in Program.cs and restart. 🚀

---

**Last Updated**: October 2025
