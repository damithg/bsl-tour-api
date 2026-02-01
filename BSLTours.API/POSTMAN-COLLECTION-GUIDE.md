# BSLTours API Postman Collection Guide

This guide explains how to use and extend the comprehensive Postman collection for testing both the BSLTours.API and the downstream Strapi CMS service.

## 📦 Collection Contents

The collection includes:
- **BSLTours.API-Collection.postman_collection.json** - Main collection with all endpoints
- **BSLTours-Local-Environment.postman_environment.json** - Local development environment
- **BSLTours-Production-Environment.postman_environment.json** - Production environment

## 🚀 Quick Setup

### 1. Import Collection and Environments

1. Open Postman
2. Click **Import** button
3. Import all three files:
   - `BSLTours-API-Collection.postman_collection.json`
   - `BSLTours-Local-Environment.postman_environment.json`
   - `BSLTours-Production-Environment.postman_environment.json`

### 2. Select Environment

1. In the top-right corner, select either:
   - **BSLTours Local Development** (for local testing)
   - **BSLTours Production** (for production testing)

### 3. Verify Configuration

1. Go to **Environment** settings
2. Ensure all variables are properly set:
   - `api_base_url` - Your API base URL
   - `strapi_base_url` - Strapi CMS URL
   - `strapi_token` - Authentication token for Strapi

## 📋 Collection Structure

### BSLTours API Endpoints

#### Tours
- `GET /api/tours` - Get all tours
- `GET /api/tours/{slug}` - Get tour by slug
- `GET /api/tours/featured` - Get featured tours
- `GET /api/tours/card` - Get tour summary cards
- `GET /api/tours/card/featured` - Get featured tour cards

#### Destinations
- `GET /api/destinations` - Get all destinations
- `GET /api/destinations/{slug}` - Get destination by slug
- `GET /api/destinations/featured` - Get featured destinations
- `GET /api/destinations/card` - Get destination summary cards
- `GET /api/destinations/card/featured` - Get featured destination cards

#### Experiences
- `GET /api/experiences` - Get all experiences
- `GET /api/experiences/{slug}` - Get experience by slug
- `GET /api/experiences/featured` - Get featured experiences
- `GET /api/experiences/card` - Get experience summary cards
- `GET /api/experiences/card/featured` - Get featured experience cards

#### Contact & Communication
- `POST /api/contact/send` - Submit dynamic contact form
- `GET /api/inquiries` - Get all inquiries (admin)
- `POST /api/inquiries` - Submit new inquiry
- `POST /api/subscribers` - Subscribe to newsletter

#### Testimonials
- `GET /api/testimonials` - Get all testimonials
- `POST /api/testimonials` - Submit new testimonial

### Strapi CMS (Downstream) Endpoints

#### Direct Strapi Access
- Tours, Destinations, and Experiences with full Strapi query capabilities
- Media and file management endpoints
- Raw Strapi API access for debugging

## 🔧 Environment Variables

### Required Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `api_base_url` | BSLTours API base URL | `http://localhost:5001` |
| `strapi_base_url` | Strapi CMS base URL | `https://graceful-happiness-10e3a700b4.strapiapp.com` |
| `strapi_token` | Strapi authentication token | `Bearer token...` |

### Test Data Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `tour_slug` | Sample tour slug for testing | `cultural-heritage-tour` |
| `destination_slug` | Sample destination slug | `kandy` |
| `experience_slug` | Sample experience slug | `elephant-safari` |
| `test_email` | Test email address | `test@example.com` |
| `test_name` | Test user name | `Test User` |

## 🧪 Testing Examples

### Testing Tours Endpoint

1. **Get All Tours**
   ```
   GET {{api_base_url}}/api/tours
   ```

2. **Get Specific Tour**
   ```
   GET {{api_base_url}}/api/tours/{{tour_slug}}
   ```
   Make sure `tour_slug` is set in your environment variables.

3. **Compare with Strapi**
   ```
   GET {{strapi_base_url}}/api/tours?populate=*
   Authorization: Bearer {{strapi_token}}
   ```

### Testing Contact Forms

1. **Submit Contact Form**
   ```json
   POST {{api_base_url}}/api/contact/send
   {
     "formType": "Contact Inquiry",
     "email": "{{test_email}}",
     "name": "{{test_name}}",
     "fields": {
       "message": "Test message from Postman",
       "phone": "+1-555-123-4567"
     }
   }
   ```

### Testing Newsletter Subscription

1. **Subscribe to Newsletter**
   ```json
   POST {{api_base_url}}/api/subscribers
   {
     "email": "{{test_email}}",
     "name": "{{test_name}}",
     "preferences": {
       "tourUpdates": true,
       "specialOffers": true
     }
   }
   ```

## 🔍 Debugging and Monitoring

### Built-in Test Scripts

The collection includes automatic test scripts that:
- Check response times (< 5000ms)
- Validate JSON structure
- Log response details to console

### Health Checks

Use the **Health & Monitoring** folder to:
- Check API health status
- Access Swagger documentation
- Verify Strapi connectivity

### Troubleshooting

1. **Authentication Issues**
   - Verify `strapi_token` is correct and not expired
   - Check token has proper permissions

2. **Connection Issues**
   - Verify `api_base_url` is accessible
   - Check `strapi_base_url` is reachable
   - Confirm firewall/network settings

3. **Data Issues**
   - Update slug variables with real data from your CMS
   - Check if content exists in Strapi before testing API

## 🚀 Extending the Collection

### Adding New Endpoints

1. **Right-click** on the appropriate folder (Tours, Destinations, etc.)
2. Select **Add Request**
3. Configure the request:
   - Set method (GET, POST, PUT, DELETE)
   - Use environment variables: `{{api_base_url}}/api/your-endpoint`
   - Add authentication if needed

### Creating Custom Test Scripts

Add to the **Tests** tab of any request:

```javascript
pm.test("Custom validation", function () {
    const jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('expectedField');
    pm.expect(jsonData.expectedField).to.not.be.empty;
});

// Extract data for use in subsequent requests
pm.environment.set("extractedId", jsonData.id);
```

### Adding New Environments

1. **Duplicate** an existing environment
2. **Rename** it (e.g., "BSLTours Staging")
3. **Update** the variables for your new environment

### Advanced Strapi Queries

For complex Strapi queries, use these patterns:

```
# Filter by multiple fields
{{strapi_base_url}}/api/tours?filters[featured][$eq]=true&filters[price][$gte]=100

# Deep population
{{strapi_base_url}}/api/tours?populate[destinations][populate]=*

# Sorting and pagination
{{strapi_base_url}}/api/tours?sort=price:asc&pagination[page]=1&pagination[pageSize]=10
```

## 📊 Collection Automation

### Running Collections

1. **Collection Runner**
   - Click **Collections** > **Run**
   - Select specific folders or entire collection
   - Set iterations and delays

2. **Newman (CLI)**
   ```bash
   # Install Newman
   npm install -g newman

   # Run collection
   newman run BSLTours-API-Collection.postman_collection.json \
     -e BSLTours-Local-Environment.postman_environment.json \
     --reporters html
   ```

### CI/CD Integration

Add to your pipeline:

```yaml
# Example GitHub Actions step
- name: Run Postman Tests
  run: |
    npm install -g newman
    newman run postman/BSLTours-API-Collection.postman_collection.json \
      -e postman/BSLTours-Production-Environment.postman_environment.json \
      --reporters cli,junit --reporter-junit-export results.xml
```

## 🔒 Security Considerations

### Sensitive Data

1. **Never commit** real tokens to version control
2. **Use environment variables** for all sensitive data
3. **Rotate tokens** regularly
4. **Limit token permissions** to minimum required

### Environment Management

1. **Local Development**: Use separate tokens with limited permissions
2. **Production**: Use production tokens only in secure environments
3. **Team Sharing**: Share collection without environment files, let team members create their own

## 📝 Best Practices

### Request Organization

1. **Group related endpoints** in folders
2. **Use descriptive names** for requests
3. **Add documentation** in request descriptions
4. **Include example responses** in request documentation

### Variable Management

1. **Use environment variables** for all dynamic values
2. **Prefix variables** by type (e.g., `api_`, `strapi_`, `test_`)
3. **Document variable purposes** in descriptions
4. **Keep environments in sync** with similar variable names

### Testing Strategy

1. **Test happy paths** first
2. **Add error case tests** for robustness
3. **Validate data structure** in tests
4. **Check performance** with response time tests
5. **Test end-to-end flows** using multiple requests

## 🆘 Support and Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure API has proper CORS configuration
2. **SSL Issues**: Use `http://` for local development
3. **Token Expiry**: Check if Strapi token needs renewal
4. **Slug Not Found**: Verify slugs exist in your Strapi CMS

### Getting Help

1. Check the **Console** tab in Postman for detailed error logs
2. Use **Swagger documentation** at `{{api_base_url}}/swagger`
3. Test **Strapi endpoints directly** to isolate issues
4. Verify **environment variable values** are correct

---

This collection is designed to grow with your API. Add new endpoints, create custom test scenarios, and integrate with your development workflow for comprehensive API testing and monitoring.