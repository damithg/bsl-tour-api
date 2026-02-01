# Dynamic Inquiries API Enhancement Guide

## Overview

The Inquiries API has been enhanced to support dynamic fields, allowing the frontend to submit any custom fields without requiring API updates. This makes the system flexible and future-proof for various inquiry types.

## Key Features

### ✅ **Backward Compatibility**
- Original `/api/inquiries` endpoint still works
- Existing `CreateInquiryDto` structure unchanged
- No breaking changes to current implementations

### ✅ **Dynamic Field Support**
- New `/api/inquiries/dynamic` endpoint
- Accepts any custom fields in `additionalFields` object
- Automatic email notifications include all dynamic fields
- Flexible inquiry types (General, Tour Booking, Group Inquiry, etc.)

### ✅ **Enhanced Management**
- Get individual inquiries by ID
- Mark inquiries as processed
- Rich response format with all dynamic fields
- Email notifications with complete field data

## API Endpoints

### 1. Get All Inquiries
```http
GET /api/inquiries
```

**Response:**
```json
[
  {
    "id": 1,
    "inquiryType": "Tour Booking",
    "name": "John Doe",
    "email": "john@example.com",
    "phone": "+1-555-123-4567",
    "message": "Interested in cultural tours",
    "tourInterest": "Cultural Heritage Tour",
    "travelDate": "2024-12-15",
    "travelPartySize": 4,
    "isProcessed": false,
    "createdAt": "2024-01-15T10:30:00Z",
    "additionalFields": {
      "accommodationPreference": "Luxury",
      "dietaryRestrictions": "Vegetarian",
      "specialRequests": "Anniversary celebration"
    }
  }
]
```

### 2. Create Legacy Inquiry (Backward Compatible)
```http
POST /api/inquiries
```

**Request:**
```json
{
  "name": "Jane Smith",
  "email": "jane@example.com",
  "phone": "+1-555-987-6543",
  "message": "Planning a family trip",
  "tourInterest": "Wildlife Safari",
  "travelDate": "2024-11-20",
  "travelPartySize": 6
}
```

### 3. Create Dynamic Inquiry (New Enhanced Endpoint)
```http
POST /api/inquiries/dynamic
```

**Request:**
```json
{
  "inquiryType": "Group Booking",
  "email": "sarah@company.com",
  "name": "Sarah Johnson",
  "message": "Corporate team building trip inquiry",
  "phone": "+1-555-456-7890",
  "tourInterest": "Adventure Package",
  "travelDate": "2024-10-15",
  "travelPartySize": 25,
  "additionalFields": {
    "companyName": "TechCorp Solutions",
    "budgetRange": "$50,000 - $75,000",
    "corporateDiscountCode": "CORP2024",
    "teamBuildingActivities": "Yes",
    "cateringRequirements": "Vegetarian and Halal options",
    "transportationNeeds": "Bus charter from Colombo",
    "specialRequests": "Team photos and certificates",
    "contactPersonRole": "HR Manager",
    "urgencyLevel": "High",
    "flexibleDates": "Yes, +/- 1 week"
  }
}
```

### 4. Get Individual Inquiry
```http
GET /api/inquiries/{id}
```

### 5. Mark Inquiry as Processed
```http
PUT /api/inquiries/{id}/processed
```

## Use Cases & Examples

### Tourist Family Inquiry
```json
{
  "inquiryType": "Family Tour",
  "email": "family@example.com",
  "name": "The Johnson Family",
  "message": "Planning our first visit to Sri Lanka",
  "additionalFields": {
    "childrenAges": "8, 12, 15",
    "wheelchairAccessNeeded": "No",
    "photographyInterest": "Wildlife and landscapes",
    "cookingClassInterest": "Yes",
    "preferredGuideLanguage": "English",
    "accommodationBudget": "Mid-range",
    "travelInsurance": "Required"
  }
}
```

### Luxury Honeymoon Inquiry
```json
{
  "inquiryType": "Honeymoon Package",
  "email": "couple@example.com",
  "name": "Alex & Jamie",
  "message": "Honeymoon trip in December",
  "additionalFields": {
    "anniversaryDate": "2024-12-20",
    "romanticDinnerRequest": "Beachside dinner",
    "spaPreferences": "Couples massage",
    "roomType": "Ocean view suite",
    "weddingCelebration": "Just married",
    "photographyPackage": "Professional photographer",
    "specialOccasionBudget": "Premium"
  }
}
```

### Adventure Group Inquiry
```json
{
  "inquiryType": "Adventure Group",
  "email": "adventures@group.com",
  "name": "Mountain Climbers Club",
  "message": "Seeking challenging adventures",
  "additionalFields": {
    "experienceLevel": "Advanced",
    "equipmentNeeded": "Climbing gear, camping equipment",
    "fitnessRequirements": "High",
    "emergencyContacts": "Provided separately",
    "medicalCertificates": "All members certified",
    "groupLeaderCertification": "Yes",
    "riskWaivers": "Acknowledged"
  }
}
```

## Implementation Benefits

### For Frontend Developers
- **No API Changes Needed**: Add new form fields without backend updates
- **Flexible Forms**: Create specialized inquiry forms for different purposes
- **Type Safety**: Strong typing with TypeScript interfaces
- **Validation**: Built-in email validation, custom validation possible

### For Backend/API
- **Future-Proof**: Handle any new fields automatically
- **Clean Architecture**: Separation between core fields and dynamic fields
- **Email Integration**: All fields automatically included in notifications
- **Database Efficiency**: JSON storage for flexible field structure

### For Business/Admin
- **Complete Information**: All submitted data captured and emailed
- **Inquiry Management**: Track and process inquiries with full context
- **Reporting**: Rich data for analysis and customer insights
- **Customer Service**: Complete inquiry history with all details

## Email Notifications

Dynamic inquiries automatically generate rich email notifications:

```html
<h2>New Group Booking Inquiry</h2>
<p><strong>From:</strong> Sarah Johnson (sarah@company.com)</p>
<p><strong>Phone:</strong> +1-555-456-7890</p>
<p><strong>Message:</strong> Corporate team building trip inquiry</p>
<p><strong>Tour Interest:</strong> Adventure Package</p>
<p><strong>Travel Date:</strong> 2024-10-15</p>
<p><strong>Party Size:</strong> 25</p>

<h3>Additional Information:</h3>
<p><strong>Company Name:</strong> TechCorp Solutions</p>
<p><strong>Budget Range:</strong> $50,000 - $75,000</p>
<p><strong>Corporate Discount Code:</strong> CORP2024</p>
<p><strong>Team Building Activities:</strong> Yes</p>
<p><strong>Catering Requirements:</strong> Vegetarian and Halal options</p>
<!-- ... all other dynamic fields ... -->

<p><strong>Submitted:</strong> 2024-01-15 10:30:00 UTC</p>
```

## Migration Strategy

### Phase 1: Immediate (No Changes Required)
- Current inquiry forms continue working unchanged
- Existing integrations remain functional

### Phase 2: Gradual Enhancement
- New forms can use `/api/inquiries/dynamic` endpoint
- Add dynamic fields as needed for new use cases
- Test with non-critical forms first

### Phase 3: Full Migration (Optional)
- Migrate existing forms to dynamic endpoint
- Deprecate legacy endpoint if desired
- Full utilization of dynamic capabilities

## Technical Notes

### Data Storage
- Core fields stored in standard database columns
- Dynamic fields stored as JSON in `AdditionalFieldsJson` column
- Automatic serialization/deserialization handled by model

### Performance Considerations
- JSON field indexing available if needed for searching
- Email generation is async and won't block API response
- Response size optimized with DTO mapping

### Validation
- Core fields have standard validation (Required, EmailAddress)
- Dynamic fields can have client-side validation
- Server-side custom validation can be added for specific inquiry types

## Future Enhancements

### Possible Extensions
- **Field Validation Rules**: Dynamic validation based on inquiry type
- **Template System**: Pre-defined field sets for common inquiry types
- **Analytics**: Track which dynamic fields are most commonly used
- **Auto-Response**: Dynamic email responses based on inquiry type
- **Integration**: Connect with CRM systems using dynamic field mapping

This enhancement makes the inquiries system highly flexible while maintaining full backward compatibility and providing rich functionality for both developers and business users.