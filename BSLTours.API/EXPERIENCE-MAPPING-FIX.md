# Experience Mapping Fix Documentation

## Issue Identified

The `GetExperienceBySlug` endpoint in the `ExperiencesController` was not properly mapping all properties from the downstream Strapi service. This was causing incomplete data to be returned when fetching individual experiences by their slug.

## Root Cause Analysis

### Primary Issue
The `GetExperienceBySlugAsync` method in `StrapiService` was using a non-standard endpoint pattern (`/api/experiences/by-slug/{slug}`) instead of the standard Strapi filtering approach used by other methods.

### Secondary Issues
1. **Inconsistent Query Pattern**: Other methods like `GetTourBySlugAsync` and `GetDestinationBySlugAsync` use standard Strapi filtering with `filters[slug][$eq]=value`
2. **Incomplete Population**: The `BuildExperiencePopulateQuery` was missing some important relations like `relatedExperiences`
3. **Missing SEO Meta Population**: SEO meta images weren't being fully populated

## Changes Made

### 1. Fixed StrapiService.GetExperienceBySlugAsync()

**Before:**
```csharp
public async Task<ExperienceDto?> GetExperienceBySlugAsync(string slug)
{
    var encodedSlug = Uri.EscapeDataString(slug);
    var url = $"/api/experiences/by-slug/{encodedSlug}";  // ❌ Non-standard endpoint

    var result = JsonSerializer.Deserialize<StrapiResponse<Experience>>(json, _jsonOptions);  // ❌ Single object response
    return result?.Data is not null ? _mapper.Map<ExperienceDto>(result.Data) : null;
}
```

**After:**
```csharp
public async Task<ExperienceDto?> GetExperienceBySlugAsync(string slug)
{
    var encodedSlug = Uri.EscapeDataString(slug);
    var query = $"/api/experiences?filters[slug][$eq]={encodedSlug}" + StrapiQueryBuilder.BuildExperiencePopulateQuery();  // ✅ Standard filtering with full population

    var result = JsonSerializer.Deserialize<StrapiResponse<List<Experience>>>(content, _jsonOptions);  // ✅ List response with filtering
    var experience = result?.Data?.FirstOrDefault();
    return experience != null ? _mapper.Map<ExperienceDto>(experience) : null;
}
```

### 2. Enhanced BuildExperiencePopulateQuery()

**Before:**
```csharp
public static string BuildExperiencePopulateQuery()
{
    return string.Join("",
        "?populate[card][populate][image]=true",
        "&populate[highlights]=true",
        "&populate[inclusions]=true",
        "&populate[whatToBring]=true",
        "&populate[galleryImage]=true",
        "&populate[seo]=true",  // ❌ Incomplete SEO population
        "&populate[location]=true",
        // ❌ Missing relatedExperiences
        "&publicationState=preview"
    );
}
```

**After:**
```csharp
public static string BuildExperiencePopulateQuery()
{
    return string.Join("",
        "?populate[card][populate][image]=true",
        "&populate[highlights]=true",
        "&populate[inclusions]=true",
        "&populate[whatToBring]=true",
        "&populate[galleryImage]=true",
        "&populate[seo][populate][metaImage]=true",  // ✅ Full SEO population including images
        "&populate[location]=true",
        "&populate[relatedExperiences][populate][card][populate][image]=true",  // ✅ Related experiences with cards
        "&populate[relatedExperiences][populate][galleryImage]=true",           // ✅ Related experiences with gallery images
        "&populate[relatedExperiences][populate][seo]=true",                    // ✅ Related experiences with SEO data
        "&publicationState=preview"
    );
}
```

## Properties Now Properly Mapped

With these changes, the following properties are now correctly populated when fetching experiences by slug:

### Core Experience Data
- ✅ `Id`, `DocumentId`, `Title`, `Slug`
- ✅ `ShortSummary`, `Description`, `Duration`
- ✅ `Price`, `Difficulty`, `Featured`
- ✅ `CreatedAt`, `UpdatedAt`

### Rich Content
- ✅ `Location` (with all location details)
- ✅ `Highlights` (converted from TextItem[] to string[])
- ✅ `Inclusions` (converted from TextItem[] to string[])
- ✅ `WhatToBring` (converted from TextItem[] to string[])

### Media & Presentation
- ✅ `Card` (with image and all card data)
- ✅ `GalleryImages` (complete gallery with metadata)
- ✅ `Seo` (including meta images, titles, descriptions)

### Relations
- ✅ `RelatedExperiences` (with their cards, gallery images, and SEO data)

## Testing the Fix

### Using Postman Collection

1. **Import the updated Postman collection**
2. **Set environment variables:**
   ```
   api_base_url: http://localhost:5001
   experience_slug: [real-experience-slug-from-your-cms]
   ```
3. **Test the endpoint:**
   ```
   GET {{api_base_url}}/api/experiences/{{experience_slug}}
   ```

### Expected Response Structure

```json
{
  "id": 1,
  "documentId": "abc123",
  "title": "Experience Title",
  "slug": "experience-slug",
  "shortSummary": "Brief description",
  "duration": "2 hours",
  "price": 75.00,
  "difficulty": "Easy",
  "description": "Detailed description",
  "featured": true,
  "createdAt": "2024-01-01T00:00:00.000Z",
  "updatedAt": "2024-01-01T00:00:00.000Z",
  "location": {
    "name": "Location Name",
    "latitude": 6.9271,
    "longitude": 79.8612
  },
  "highlights": [
    "Highlight 1",
    "Highlight 2"
  ],
  "inclusions": [
    "Inclusion 1",
    "Inclusion 2"
  ],
  "whatToBring": [
    "Item 1",
    "Item 2"
  ],
  "seo": {
    "metaTitle": "SEO Title",
    "metaDescription": "SEO Description",
    "metaImage": {
      "url": "https://example.com/image.jpg",
      "alternativeText": "Alt text"
    }
  },
  "card": {
    "header": "Card Title",
    "summary": "Card summary",
    "image": {
      "publicId": "cloudinary-public-id",
      "alt": "Image alt text"
    },
    "tags": ["tag1", "tag2"]
  },
  "galleryImages": [
    {
      "publicId": "gallery-image-1",
      "alt": "Gallery image",
      "caption": "Image caption"
    }
  ],
  "relatedExperiences": [
    {
      "id": 2,
      "title": "Related Experience",
      "slug": "related-experience-slug",
      "card": { /* card data */ },
      "galleryImages": [ /* gallery data */ ],
      "seo": { /* seo data */ }
    }
  ]
}
```

## Performance Considerations

### Query Optimization
- The new query uses standard Strapi filtering which is optimized
- Population is done in a single request rather than multiple API calls
- Related experiences are populated with only necessary fields to avoid over-fetching

### Caching Recommendations
- Consider implementing response caching for experience data
- Cache key should include the slug and any population parameters
- TTL should be based on content update frequency

## Compatibility

### Breaking Changes
- **None**: This is a bug fix that enhances data completeness without changing the API contract

### Backward Compatibility
- ✅ All existing fields remain unchanged
- ✅ Response structure is maintained
- ✅ Only enhancement is more complete data population

## Validation Steps

1. **Compare with other endpoints**: The slug endpoint now behaves consistently with list endpoints
2. **Verify AutoMapper**: All mappings are working correctly with enhanced data
3. **Test related experiences**: Ensure circular references are handled properly
4. **Check performance**: Monitor response times with enhanced population

## Future Enhancements

### Potential Improvements
1. **Selective Population**: Allow clients to specify which relations to populate
2. **Pagination for Related**: If related experiences grow large, consider pagination
3. **Image Optimization**: Add responsive image URLs for different screen sizes
4. **Search Integration**: Enhanced data makes search indexing more comprehensive

### Monitoring
- Track response times for slug endpoints
- Monitor data completeness in production logs
- Set up alerts for mapping failures

---

This fix ensures that the Experience slug endpoint returns complete, consistent data that matches the quality and completeness of other endpoints in the API.