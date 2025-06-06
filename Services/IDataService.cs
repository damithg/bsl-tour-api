using System.Collections.Generic;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;

namespace BSLTours.API.Services
{
    public interface IDataService
    {
        // Tour Packages
        Task<IEnumerable<TourPackage>> GetTourPackagesAsync();
        Task<IEnumerable<TourPackage>> GetFeaturedTourPackagesAsync();
        Task<TourPackage> GetTourPackageByIdAsync(int id);
        Task<TourPackage> GetTourPackageBySlugAsync(string slug);
        Task<TourPackage> CreateTourPackageAsync(CreateTourPackageDto tourPackageDto);
        
        // Destinations
        Task<IEnumerable<Destination>> GetDestinationsAsync();
        Task<Destination> GetDestinationByIdAsync(int id);
        Task<Destination> GetDestinationBySlugAsync(string slug);
        Task<Destination> CreateDestinationAsync(CreateDestinationDto destinationDto);
        
        // Testimonials
        Task<IEnumerable<Testimonial>> GetTestimonialsAsync();
        Task<Testimonial> CreateTestimonialAsync(CreateTestimonialDto testimonialDto);
        
        // Inquiries
        Task<IEnumerable<Inquiry>> GetInquiriesAsync();
        Task<Inquiry> CreateInquiryAsync(CreateInquiryDto inquiryDto);
        
        // Subscribers
        Task<Subscriber> GetSubscriberByEmailAsync(string email);
        Task<Subscriber> AddSubscriberAsync(CreateSubscriberDto subscriberDto);
    }


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


        Task<List<object>> GetFeaturedCardsAsync(string contentType);
    }
}