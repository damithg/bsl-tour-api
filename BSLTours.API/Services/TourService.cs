using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Services;

public class TourService : ITourService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public TourService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://graceful-happiness-10e3a700b4.strapiapp.com");
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_API_TOKEN"); // optional, if needed

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<TourDto>> GetToursAsync()
    {
        var response = await _httpClient.GetAsync("/api/tours?populate=*");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<TourDto>>>(content, _jsonOptions);

        return result?.Data ?? new List<TourDto>();
    }

    public async Task<TourDto?> GetTourBySlugAsync(string slug)
    {
        var query = $"/api/tours?filters[slug][$eq]={slug}&populate=*";
        var response = await _httpClient.GetAsync(query);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StrapiResponse<List<TourDto>>>(content, _jsonOptions);

        return result?.Data?.FirstOrDefault();
    }
}