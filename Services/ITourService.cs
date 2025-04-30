using BSLTours.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BSLTours.API.Services;

public interface ITourService
{
    Task<List<TourDto>> GetToursAsync();
    Task<TourDto> GetTourBySlugAsync(string slug);
}