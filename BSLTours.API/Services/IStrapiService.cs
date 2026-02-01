using System.Collections.Generic;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;

namespace BSLTours.API.Services;

public interface IStrapiService
{
    Task<List<DestinationDto>> GetDestinationsAsync();
    Task<List<DestinationDto>> GetFeaturedDestinationsAsync();
    Task<DestinationDto?> GetDestinationBySlugAsync(string slug);


    Task<List<TourDto>> GetToursAsync();
    Task<TourDto?> GetTourBySlugAsync(string slug);

    Task<List<TourDto>> GetFeaturedToursAsync();

    // Optional: add by slug, etc. in future

    Task<List<ExperienceDto>> GetExperiencesAsync();
    Task<ExperienceDto?> GetExperienceBySlugAsync(string slug);
    Task<List<ExperienceDto>> GetFeaturedExperiencesAsync();


    Task<List<SummaryCardDto>> GetCardsAsync(string contentType, bool? isFeatured);

}