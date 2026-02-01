# BSLTours API - Testing Guide

## Testing the Comprehensive Inquiries Endpoint

### Prerequisites

1. **Environment Setup**

   You need to set the appropriate email provider API key as an environment variable:

   **For SendGrid (currently configured):**
   ```bash
   # Windows PowerShell
   $env:SendGridApiKey = "your-sendgrid-api-key-here"

   # Windows CMD
   set SendGridApiKey=your-sendgrid-api-key-here

   # Linux/Mac
   export SendGridApiKey=your-sendgrid-api-key-here
   ```

   **For Postmark (if switching providers):**
   ```bash
   # Windows PowerShell
   $env:PostmarkServerToken = "your-postmark-server-token-here"

   # Windows CMD
   set PostmarkServerToken=your-postmark-server-token-here

   # Linux/Mac
   export PostmarkServerToken=your-postmark-server-token-here
   ```

2. **Start the API**
   ```bash
   cd C:\projects\current-work\bsl-tours-api\BSLTours.API
   dotnet run
   ```

   The API will start on `http://localhost:80` (as configured in Program.cs).

### Testing Methods

#### Method 1: Using cURL

**Basic test (without Turnstile token):**
```bash
curl -X POST http://localhost:80/api/inquiries/comprehensive \
  -H "Content-Type: application/json" \
  -d @test-comprehensive-inquiry.json
```

**Windows PowerShell:**
```powershell
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

#### Method 2: Using Postman

1. **Create a new POST request**
   - URL: `http://localhost:80/api/inquiries/comprehensive`
   - Method: `POST`
   - Headers: `Content-Type: application/json`

2. **Set the request body** (raw JSON):
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

3. **Send the request** and check:
   - Status code should be `200 OK`
   - Response body should contain success message
   - Check the email inbox at `info@siprea.com` for the notification email

#### Method 3: Using Swagger UI

1. Start the API with `dotnet run`
2. Open browser to `http://localhost:80/swagger`
3. Find the `/api/inquiries/comprehensive` POST endpoint
4. Click "Try it out"
5. Paste the JSON from `test-comprehensive-inquiry.json`
6. Click "Execute"
7. Check the response

#### Method 4: Using VS Code REST Client

Create a file `test-inquiries.http`:

```http
### Test Comprehensive Inquiry Endpoint
POST http://localhost:80/api/inquiries/comprehensive
Content-Type: application/json

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
  "message": "We are interested in booking a cultural heritage tour for our family.",
  "hearAboutUs": "Google Search",
  "subscribed": true
}
```

Click "Send Request" above the `###` line.

### What to Verify

1. **API Response**
   - ✅ Status code: `200 OK`
   - ✅ Response body: `{ "message": "Comprehensive inquiry submitted successfully" }`

2. **Email Notification**
   - ✅ Check inbox at `info@siprea.com`
   - ✅ Email should contain all inquiry details
   - ✅ Subject line should include inquiry type and customer name
   - ✅ Email should be formatted properly (HTML)

3. **Console Logs**
   Check the API console output for:
   - ✅ Email sent successfully logs
   - ✅ No errors or exceptions
   - ✅ SendGrid/Postmark API call logs

### Testing with Different Providers

#### Switch to Postmark

1. Update `Program.cs`:
   ```csharp
   // Comment out SendGrid
   // builder.Services.AddSendGridEmailProvider(options => { /* ... */ });

   // Add Postmark
   builder.Services.AddPostmarkEmailProvider(options =>
   {
       options.ServerToken = Environment.GetEnvironmentVariable("PostmarkServerToken")
           ?? throw new InvalidOperationException("PostmarkServerToken not set");
       options.DefaultFromEmail = builder.Configuration["Postmark:DefaultFromEmail"];
       options.DefaultFromName = builder.Configuration["Postmark:DefaultFromName"];
   });

   builder.Services.AddEmailService(builder.Configuration);
   ```

2. Set environment variable:
   ```bash
   export PostmarkServerToken=your-postmark-server-token
   ```

3. Restart the API and test again

#### Switch Back to SendGrid

Just reverse the changes in Program.cs and restart.

### Testing Other Inquiry Endpoints

#### Legacy Inquiry (POST /api/inquiries)
```json
{
  "name": "Jane Smith",
  "email": "jane@example.com",
  "phone": "+1-555-987-6543",
  "message": "I would like more information about your tours.",
  "tourInterest": "Beach Tours",
  "travelDate": "2025-11-15",
  "travelPartySize": 4
}
```

#### Dynamic Inquiry (POST /api/inquiries/dynamic)
```json
{
  "name": "Bob Johnson",
  "email": "bob@example.com",
  "phone": "+1-555-111-2222",
  "message": "Interested in customized tours",
  "inquiryType": "Custom",
  "tourInterest": "Adventure Tours",
  "travelDate": "2025-12-01",
  "travelPartySize": 2,
  "additionalFields": {
    "preferredActivities": "Hiking, Wildlife",
    "budget": "Medium"
  }
}
```

### Troubleshooting

#### "SendGridApiKey environment variable is not set"
- Make sure you've set the environment variable before starting the API
- Restart your terminal/PowerShell after setting the variable

#### "Failed to send email"
- Check that your API key is valid
- Check that the "from" email is verified in SendGrid/Postmark
- Check API logs for specific error messages

#### "Invalid security token"
- The `turnstileToken` field is optional for testing
- You can omit it or set it to any string for local testing
- The validation only happens if the token is provided

#### Email not received
- Check spam folder
- Verify the recipient email in InquiriesController.cs:206 (`info@siprea.com`)
- Check SendGrid/Postmark dashboard for delivery status

### Load Testing (Optional)

To test multiple requests:

```bash
# Send 10 requests
for i in {1..10}; do
  curl -X POST http://localhost:80/api/inquiries/comprehensive \
    -H "Content-Type: application/json" \
    -d @test-comprehensive-inquiry.json
  sleep 1
done
```

### Next Steps

1. **Production Testing**: Test with real API keys in a staging environment
2. **Integration Tests**: Add automated tests for the controllers
3. **Email Template Testing**: Verify email formatting in different email clients
4. **Rate Limiting**: Consider adding rate limiting for the inquiry endpoints

---

**Last Updated**: October 2025
**Maintained By**: BSLTours Development Team
