# Troubleshooting 404 Error - Experience Slug Endpoint

## Issue
After implementing the mapping fix, the Experience slug endpoint started returning 404 errors.

## Root Cause
The original implementation used a **custom Strapi route** (`/api/experiences/by-slug/{slug}`) which differs from the standard pattern used by Tours and Destinations.

## Solution Implemented
I've implemented a **fallback approach** that tries both endpoint patterns:

### Method 1: Standard Filtering (Preferred)
```
GET /api/experiences?filters[slug][$eq]={slug}&populate[...]
```
- Uses standard Strapi filtering
- Includes full population query for complete data
- Consistent with Tours and Destinations endpoints

### Method 2: Custom Route Fallback
```
GET /api/experiences/by-slug/{slug}
```
- Falls back to original custom route if standard filtering fails
- Still uses enhanced AutoMapper for better data mapping
- Maintains backward compatibility

## Testing the Fix

### Using Postman - Test Both Endpoints Directly

1. **Test Standard Filtering** (against Strapi directly):
   ```
   GET {{strapi_base_url}}/api/experiences?filters[slug][$eq]=your-experience-slug&populate=*
   Authorization: Bearer {{strapi_token}}
   ```

2. **Test Custom Route** (against Strapi directly):
   ```
   GET {{strapi_base_url}}/api/experiences/by-slug/your-experience-slug
   Authorization: Bearer {{strapi_token}}
   ```

3. **Test Your API** (should work now with fallback):
   ```
   GET {{api_base_url}}/api/experiences/your-experience-slug
   ```

### Quick Diagnosis

Run these requests in order to identify which pattern your Strapi supports:

#### Step 1: Check if experiences exist
```bash
GET {{strapi_base_url}}/api/experiences
Authorization: Bearer {{strapi_token}}
```

#### Step 2: Get a real slug from the response
Look for `"slug": "some-experience-slug"` in the response and use that value.

#### Step 3: Test standard filtering
```bash
GET {{strapi_base_url}}/api/experiences?filters[slug][$eq]=some-experience-slug
Authorization: Bearer {{strapi_token}}
```

#### Step 4: Test custom route (if Step 3 fails)
```bash
GET {{strapi_base_url}}/api/experiences/by-slug/some-experience-slug
Authorization: Bearer {{strapi_token}}
```

## Possible Issues & Solutions

### Issue 1: No experiences exist in Strapi
**Symptoms**: Empty array response from `/api/experiences`
**Solution**: Create test experience content in your Strapi admin panel

### Issue 2: Slug field doesn't exist
**Symptoms**: Standard filtering returns empty results
**Solution**: Check your Strapi experience content type has a `slug` field

### Issue 3: Custom route was removed
**Symptoms**: Both endpoints return 404
**Solution**: The custom route might have been a custom API route that no longer exists

### Issue 4: Population issues
**Symptoms**: Endpoint works but returns incomplete data
**Solution**: The fallback approach should handle this by using AutoMapper

## Configuration Check

### Verify Strapi Setup
1. **Admin Panel**: Log into your Strapi admin
2. **Content Types**: Check if "Experiences" content type exists
3. **Fields**: Verify the experience has these fields:
   - `slug` (text)
   - `title` (text)
   - `description` (rich text)
   - `highlights` (component)
   - `inclusions` (component)
   - `card` (component)
   - `galleryImage` (media)

### Verify API Permissions
1. **Settings → Roles → Public**: Ensure `experiences` has `find` and `findOne` permissions
2. **API Tokens**: Ensure your token has access to experiences

## Emergency Revert

If the fallback approach doesn't work, you can quickly revert to the original:

```csharp
public async Task<ExperienceDto?> GetExperienceBySlugAsync(string slug)
{
    var encodedSlug = Uri.EscapeDataString(slug);
    var url = $"/api/experiences/by-slug/{encodedSlug}";

    try
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<Experience>>(json, _jsonOptions);

        return result?.Data is not null
            ? _mapper.Map<ExperienceDto>(result.Data)
            : null;
    }
    catch (Exception ex)
    {
        return null;
    }
}
```

## Logging for Debugging

To help debug, you can temporarily enable logging by uncommenting the logger lines:

```csharp
//_logger.LogWarning(ex, "Standard filtering failed for experience slug '{Slug}', trying fallback", slug);
//_logger.LogError(ex, "Both methods failed for experience slug '{Slug}'", slug);
```

This will show you which method is being used and any errors.

## Next Steps

1. **Test the endpoint** - It should now work with the fallback approach
2. **Check logs** - See which method is being used
3. **Optimize** - Once you know which pattern works, you can optimize the code
4. **Update documentation** - Document which pattern your Strapi instance uses

The fallback approach ensures the endpoint works regardless of your Strapi configuration while maintaining the enhanced mapping capabilities.