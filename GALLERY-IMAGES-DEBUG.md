# Gallery Images Debugging Guide

## Issue
Gallery images are present in the downstream Strapi service response but not appearing in the BSLTours.API response.

## Changes Made

### 1. Enhanced Population Query
Updated `BuildExperiencePopulateQuery()` to include both possible field names:
```csharp
"&populate[galleryImages]=true",  // Plural (standard)
"&populate[galleryImage]=true",   // Singular (backup)
```

### 2. Custom AutoMapper Resolver
Added `GalleryImageResolver` to handle the mapping explicitly:
```csharp
public class GalleryImageResolver : IValueResolver<Experience, ExperienceDto, List<GalleryImageDto>>
{
    public List<GalleryImageDto> Resolve(Experience source, ExperienceDto destination, List<GalleryImageDto> destMember, ResolutionContext context)
    {
        var galleryImages = source.GalleryImages;
        if (galleryImages == null || !galleryImages.Any())
            return new List<GalleryImageDto>();

        return galleryImages.Select(img => new GalleryImageDto
        {
            PublicId = img.PublicId,
            Alt = img.Alt,
            Caption = img.Caption
        }).ToList();
    }
}
```

## Debugging Steps

### Step 1: Test Strapi Directly
Test your Strapi service directly to see the exact field names:

```bash
GET {{strapi_base_url}}/api/experiences?populate=*
Authorization: Bearer {{strapi_token}}
```

Look for gallery image fields in the response. They might be named:
- `galleryImages` (plural)
- `galleryImage` (singular)
- `gallery_images` (snake_case)
- `Gallery` (different name entirely)

### Step 2: Test Specific Experience with Gallery Images
```bash
GET {{strapi_base_url}}/api/experiences/[slug-with-gallery-images]?populate=*
Authorization: Bearer {{strapi_token}}
```

### Step 3: Check Population Query
Test the exact population query being used:
```bash
GET {{strapi_base_url}}/api/experiences?filters[slug][$eq]=your-slug&populate[galleryImages]=true&populate[galleryImage]=true
Authorization: Bearer {{strapi_token}}
```

### Step 4: Test Your API
```bash
GET {{api_base_url}}/api/experiences/your-slug
```

## Common Issues & Solutions

### Issue 1: Field Name Mismatch
**Symptoms**: Strapi returns gallery images but API doesn't
**Solution**: Check the exact field name in Strapi response and update the Experience model

If your Strapi uses a different field name, update the Experience model:
```csharp
public class Experience
{
    // ... other properties

    [JsonPropertyName("gallery_images")]  // If using snake_case
    public List<GalleryImage> GalleryImages { get; set; }

    // OR

    [JsonPropertyName("gallery")]  // If using different name
    public List<GalleryImage> GalleryImages { get; set; }
}
```

### Issue 2: Strapi Media Field Structure
**Symptoms**: Field exists but has different structure
**Solution**: Check if Strapi returns media objects differently

Strapi media might return:
```json
{
  "galleryImages": {
    "data": [
      {
        "id": 1,
        "attributes": {
          "url": "...",
          "alternativeText": "...",
          "caption": "..."
        }
      }
    ]
  }
}
```

In this case, update the GalleryImage model to match Strapi's structure.

### Issue 3: Permissions
**Symptoms**: Field is missing entirely from Strapi response
**Solution**: Check Strapi permissions

1. In Strapi admin: **Settings → Roles → Public**
2. Ensure **experiences** has permission to **find** and **findOne**
3. Check that **Media Library** has **find** permission

### Issue 4: Media Not Uploaded
**Symptoms**: Field exists but is empty array
**Solution**: Ensure gallery images are actually uploaded in Strapi admin

## Testing Gallery Images Fix

### Using Postman

1. **Test Strapi directly** (both endpoints):
   ```
   GET {{strapi_base_url}}/api/experiences?populate=*
   GET {{strapi_base_url}}/api/experiences/by-slug/your-slug
   ```

2. **Compare the responses** - Look for:
   - Field name used for gallery images
   - Structure of the image objects
   - Whether images are present at all

3. **Test your API**:
   ```
   GET {{api_base_url}}/api/experiences/your-slug
   ```

4. **Check the response** for `galleryImages` array

### Expected Response Structure

Your API should return:
```json
{
  "id": 1,
  "title": "Experience Title",
  "galleryImages": [
    {
      "publicId": "cloudinary-id-or-url",
      "alt": "Image description",
      "caption": "Image caption"
    }
  ]
}
```

## Quick Fix Options

### Option 1: If field name is different
Update Experience model with correct JSON property name:
```csharp
[JsonPropertyName("actual_field_name")]
public List<GalleryImage> GalleryImages { get; set; }
```

### Option 2: If structure is different
Update GalleryImage model to match Strapi structure:
```csharp
public class GalleryImage
{
    [JsonPropertyName("url")]
    public string PublicId { get; set; }

    [JsonPropertyName("alternativeText")]
    public string Alt { get; set; }

    public string Caption { get; set; }
}
```

### Option 3: If using Strapi's data wrapper
Update the resolver to handle Strapi's response structure:
```csharp
public class GalleryImageResolver : IValueResolver<Experience, ExperienceDto, List<GalleryImageDto>>
{
    public List<GalleryImageDto> Resolve(Experience source, ExperienceDto destination, List<GalleryImageDto> destMember, ResolutionContext context)
    {
        // Handle Strapi's {data: [...]} wrapper structure
        // This depends on your exact Strapi response format
    }
}
```

## Next Steps

1. **Run the debugging steps** to identify the exact issue
2. **Compare Strapi response** with your API response
3. **Apply the appropriate fix** based on findings
4. **Test with multiple experiences** to ensure consistency

The enhanced population query and custom resolver should handle most common scenarios, but you may need to adjust based on your specific Strapi configuration.