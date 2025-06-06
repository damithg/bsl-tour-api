using AutoMapper;
using BSLTours.API.Models;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BSLTours.API.Models.Dtos;


namespace BSLTours.API.Services;

public class StrapiService :IStrapiService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly JsonSerializerOptions _jsonOptions;

    public StrapiService(HttpClient httpClient, IMapper mapper)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _httpClient.BaseAddress = new Uri("https://graceful-happiness-10e3a700b4.strapiapp.com");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "1fbd114f8cbcc4d58c1808af61b4c682a5337233075eb967b61118cc7861710ba9f9c63acd30dd025f1ecb3ec4259c4629eab0a4e7abcdf41d68cb80651dbbc0aac72cd9ecfe86b5eab8425acfdcb7c834215131e82eb6afb755c66e4a6261a71428987b733de4b26226f1a46343f9c548e7f21f76dce9d2678a338ba5d11c5e");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<DestinationDto>> GetDestinationsAsync()
    {
        var query = "/api/destinations?" + StrapiQueryBuilder.GetDestinationPopulateQuery().TrimStart('&');

        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<DestinationDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<DestinationDto>();
    }

    public async Task<List<DestinationCardDto>> GetFeaturedDestinationCardsAsync()
    {
        var query = "api/destinations?filters[featured][$eq]=true&populate[card][populate]=image";
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<DestinationCardDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<DestinationCardDto>();
    }

    public async Task<List<DestinationDto>> GetFeaturedDestinationsAsync()
    {
        var query = "api/destinations?filters[featured][$eq]=true&populate[card][populate]=image";
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<DestinationDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<DestinationDto>();
    }


    public async Task<DestinationDto?> GetDestinationBySlugAsync(string slug)
    {
        var query = $"/api/destinations?filters[slug][$eq]={Uri.EscapeDataString(slug)}" + StrapiQueryBuilder.GetDestinationPopulateQuery();

        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<DestinationDto>>>(content, _jsonOptions);

        return result?.Data?.FirstOrDefault();
    }

    public async Task<List<TourDto>> GetToursAsync()
    {
        var query = "/api/tours?" + StrapiQueryBuilder.GetTourPopulateQuery().TrimStart('&');

        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<TourDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<TourDto>();
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug)
    {
        var query = $"/api/tours?filters[slug][$eq]={Uri.EscapeDataString(slug)}" + StrapiQueryBuilder.GetTourPopulateQuery();

        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<TourDto>>>(content, _jsonOptions);

        return result?.Data?.FirstOrDefault();
    }

    public async Task<List<TourDto>> GetFeaturedToursAsync()
    {
        var query = "api/tours?filters[featured][$eq]=true&populate[card][populate]=image";
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<TourDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<TourDto>();
    }

    public async Task<List<ExperienceDto>> GetExperiencesAsync()
    {
        return await GetExperiencesFromStrapiAsync("/api/experiences" + StrapiQueryBuilder.BuildExperiencePopulateQuery());

    }


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
            //_logger.LogError(ex, "Error fetching experience with slug '{Slug}'", slug);
            return null;
        }
    }


    public async Task<List<ExperienceDto>> GetFeaturedExperiencesAsync()
    {
        var url = "/api/experiences" +
                  "?filters%5Bfeatured%5D%5B%24eq%5D=true" +
                  "&populate%5Bcard%5D%5Bpopulate%5D%5Bimage%5D=%2A" +
                  "&populate%5BgalleryImage%5D=%2A" +
                  "&populate%5Bseo%5D%5Bpopulate%5D%5BmetaImage%5D%5Bfields%5D%5B0%5D=url" +
                  "&populate%5Bseo%5D%5Bpopulate%5D%5BmetaImage%5D%5Bfields%5D%5B1%5D=alternativeText" +
                  "&populate%5Bseo%5D%5Bpopulate%5D%5BmetaImage%5D%5Bfields%5D%5B2%5D=caption" +
                  "&publicationState=preview";
        return await GetExperiencesFromStrapiAsync(url);
    }

    // Shared helper
    private async Task<List<ExperienceDto>> GetExperiencesFromStrapiAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<StrapiResponse<List<Experience>>>(content, _jsonOptions);

            return result?.Data != null
                ? result.Data.Select(e => _mapper.Map<ExperienceDto>(e)).ToList()
                : new List<ExperienceDto>();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error fetching experiences from Strapi with URL {Url}", url);
            return new List<ExperienceDto>();
        }
    }

}

public class StrapiResponse<T>
{
    public T Data { get; set; }
}




public static class StrapiQueryBuilder
{
    public static string BuildExperiencePopulateQuery()
    {
        return string.Join("",
            "?populate[card][populate][image]=true",
            "&populate[highlights]=true",
            "&populate[inclusions]=true",
            "&populate[whatToBring]=true",
            "&populate[galleryImage]=true",
            "&populate[seo]=true",
            "&populate[location]=true",
            "&publicationState=preview"
        );
    }

    public static string GetDestinationPopulateQuery()
    {
        return string.Join("",
            "&populate[overview][populate][image]=true",
            "&populate[subSections][populate][image]=true",
            "&populate[heroImage]=true",
            "&populate[featuresSection][populate][items][populate][image]=true",
            "&populate[galleryImages]=true",
            "&populate[faqs]=true",
            "&populate[quoteBlock]=true",
            "&populate[videoBlock]=true",
            "&populate[relatedTours][populate][image]=true",
            "&populate[nearbyAttractions][populate][image]=true",
            "&populate[essentialInfo]=true",
            "&populate[card][populate][image]=true"
        );
    }


    public static string GetBySlugQuery(string slug)
    {
        return $"/api/destinations?filters[slug][$eq]={Uri.EscapeDataString(slug)}" + GetDestinationPopulateQuery();
    }

    public static string GetAllDestinationsQuery()
    {
        return "/api/destinations" + GetDestinationPopulateQuery();
    }

    public static string GetTourPopulateQuery()
    {
        return string.Join("",
            "&populate[heroImage]=true",
            "&populate[galleryImages]=true",
            "&populate[itinerary][populate][image]=true",
            "&populate[relatedDestinations][populate][image]=true",
            "&populate[faqs]=true",
            "&populate[pricingTiers]=true",
            "&populate[optionalAddOns]=true",
            "&populate[card][populate][image]=true",
            "&populate[reviews]=true"
        );
    }

}
