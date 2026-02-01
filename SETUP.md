# Quick Setup Guide

Get BSLTours API up and running in 5 minutes!

## Step 1: Choose Your Email Provider

You need to pick one email provider (you can switch later):

| Provider | Best For | Free Tier | Setup Time |
|----------|----------|-----------|------------|
| **SendGrid** | Marketing + Transactional | 100 emails/day | ~3 min |
| **Postmark** | Transactional emails | 100 emails/month | ~3 min |

👉 **Recommendation**: Start with **SendGrid** if you're unsure.

---

## Step 2: Get API Credentials

### Option A: SendGrid Setup

1. **Sign up**: https://sendgrid.com
2. **Get API key**:
   - Go to: Settings → API Keys
   - Click "Create API Key"
   - Name: `BSLTours Development`
   - Permissions: Select "Mail Send" (Full Access)
   - Click "Create & View"
   - **Copy the key** (you won't see it again!)

3. **Verify sender** (optional but recommended):
   - Go to: Settings → Sender Authentication
   - Verify your from email address

### Option B: Postmark Setup

1. **Sign up**: https://postmarkapp.com
2. **Get server token**:
   - Go to: Servers → Select your server (or create one)
   - Click "API Tokens" tab
   - Copy your "Server API Token"

3. **Verify sender** (REQUIRED for Postmark):
   - Go to: Sender Signatures
   - Add: `info@bestsrilankatours.com` (or your from email)
   - Check email and click verification link
   - Wait for "Verified" status ✅

---

## Step 3: Configure the API

### 3.1 Update Configuration File

Open: `BSLTours.API/appsettings.json`

**For SendGrid:**
```json
{
  "EmailService": {
    "Provider": "SendGrid",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  }
}
```

**For Postmark:**
```json
{
  "EmailService": {
    "Provider": "Postmark",
    "DefaultFromEmail": "info@bestsrilankatours.com",
    "DefaultFromName": "BSL Tours"
  }
}
```

### 3.2 Set Environment Variable

Open PowerShell (or terminal) and run:

**For SendGrid:**
```powershell
$env:SendGridApiKey = "SG.paste-your-sendgrid-api-key-here"
```

**For Postmark:**
```powershell
$env:PostmarkServerToken = "paste-your-postmark-server-token-here"
```

⚠️ **Important**: Set this in the SAME terminal where you'll run the API!

---

## Step 4: Build & Run

In the same terminal where you set the environment variable:

```powershell
# Navigate to project
cd C:\projects\current-work\bsl-tours-api

# Build solution
dotnet build

# Run API
cd BSLTours.API
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:80
```

---

## Step 5: Test It!

### Option A: Use Swagger UI (Easiest)

1. Open browser: http://localhost/swagger
2. Find `/api/inquiries/comprehensive`
3. Click "Try it out"
4. Paste this JSON:
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "+1-555-123-4567",
  "inquiryType": "tour",
  "subjectName": "Test Tour",
  "travelPlanning": {
    "flexibleDates": false,
    "travelMonth": "December 2025",
    "adults": 2,
    "children": 0
  },
  "message": "This is a test inquiry."
}
```
5. Click "Execute"
6. Check response: Should be `200 OK`

### Option B: Use PowerShell

Open a NEW terminal (keep API running):

```powershell
cd C:\projects\current-work\bsl-tours-api

$json = Get-Content test-comprehensive-inquiry.json -Raw
Invoke-WebRequest -Uri "http://localhost:80/api/inquiries/comprehensive" `
  -Method POST `
  -ContentType "application/json" `
  -Body $json
```

### ✅ Success Indicators

1. **API Response**: `200 OK` with message "Comprehensive inquiry submitted successfully"
2. **Console Logs**: Look for:
   ```
   info: Email sent successfully via SendGrid
   ```
   or
   ```
   info: Email sent successfully via Postmark
   ```
3. **Email Received**: Check inbox at `info@siprea.com`
4. **Provider Dashboard**:
   - SendGrid: https://app.sendgrid.com/email_activity
   - Postmark: https://account.postmarkapp.com/servers/{server}/streams/outbound/activity

---

## 🚨 Troubleshooting

### Error: "SendGridApiKey environment variable is not set"

**Fix:**
```powershell
# Set the variable
$env:SendGridApiKey = "your-api-key"

# Verify it's set
echo $env:SendGridApiKey

# Restart API
cd BSLTours.API
dotnet run
```

### Error: "PostmarkServerToken environment variable is not set"

**Fix:**
```powershell
# Set the variable
$env:PostmarkServerToken = "your-token"

# Verify it's set
echo $env:PostmarkServerToken

# Restart API
cd BSLTours.API
dotnet run
```

### Error: "Request does not contain a valid Server token" (Postmark)

**Causes:**
1. Token is incorrect → Copy it again from Postmark dashboard
2. Sender not verified → Check "Sender Signatures" in Postmark

**Fix:**
```powershell
# 1. Run diagnostic
.\check-postmark-setup.ps1

# 2. Verify sender in Postmark dashboard
# 3. Set correct token
$env:PostmarkServerToken = "correct-token-here"

# 4. Restart API
cd BSLTours.API
dotnet run
```

### Error: "Could not find file 'BSLTours.API.csproj'"

**Fix:**
```powershell
# Make sure you're in the API directory
cd C:\projects\current-work\bsl-tours-api\BSLTours.API
dotnet run
```

### Email not received

**Check:**
1. ✅ Console shows "Email sent successfully"?
2. ✅ Check spam folder
3. ✅ Check provider dashboard for delivery status
4. ✅ Verify sender email in provider settings

---

## 🔄 Switch Providers Later

Want to switch from SendGrid to Postmark (or vice versa)?

1. Update `appsettings.json` → Change `"Provider"` value
2. Set new environment variable
3. Restart API

**No code changes needed!** See [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md) for details.

---

## 📚 Next Steps

- **Read Full Documentation**: [README.md](README.md)
- **Understand Architecture**: [ARCHITECTURE.md](ARCHITECTURE.md)
- **Learn Provider Switching**: [PROVIDER-SWITCHING.md](PROVIDER-SWITCHING.md)
- **Test All Endpoints**: [TESTING.md](TESTING.md)

---

## 🆘 Still Stuck?

Run the diagnostic script:
```powershell
.\check-postmark-setup.ps1
```

This will check:
- ✅ Environment variables are set
- ✅ Configuration is correct
- ✅ Token format is valid
- ✅ Setup requirements

---

**Happy Coding!** 🚀
