using System;
using System.Collections.Generic;

namespace BSLTours.API.Models
{
    public class TourDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool Featured { get; set; }
        public string Summary { get; set; }
        public string Duration { get; set; }
        public decimal StartingFrom { get; set; }
        public string Currency { get; set; }
        public ImageDto HeroImage { get; set; }
        public List<GalleryImageDto> GalleryImages { get; set; }
        public List<ItineraryDayDto> Itinerary { get; set; }
        public List<string> Inclusions { get; set; }
        public List<string> Exclusions { get; set; }
        public string AccommodationInfo { get; set; }
        public List<RelatedDestinationDto> RelatedDestinations { get; set; }
        public List<FaqDto> Faqs { get; set; }
        public List<TourPricingTierDto> PricingTiers { get; set; } // Deluxe / Standard etc.
        public List<TourAddOnDto> OptionalAddOns { get; set; }
        public string OperatedBy { get; set; }
        public string GuideProfileUrl { get; set; }
        public string Category { get; set; } // e.g., Adventure, Luxury, Family
        public List<string> Tags { get; set; } // e.g., ["wildlife", "beaches", "UNESCO"]
        public List<DateTime> DepartureDates { get; set; }
        public string BookingWindow { get; set; } // e.g., "Available Year-Round", "June–August Only"
        public List<ReviewDto> Reviews { get; set; }
        public string MapEmbedUrl { get; set; }
        public string RouteOverview { get; set; }
        public int? MinGroupSize { get; set; }
        public int? MaxGroupSize { get; set; }
        public string BookingUrl { get; set; }
        public List<string> LanguagesSupported { get; set; }
        public CardDto Card { get; set; }

    }

    public class ItineraryDayDto
    {
        public int Day { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImageDto Image { get; set; }
    }

    public class RelatedDestinationDto
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public ImageDto Image { get; set; }
    }

    public class TourPricingTierDto
    {
        public string Label { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class TourAddOnDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class ReviewDto
    {
        public string Reviewer { get; set; }
        public string Country { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1–5
    }
}
