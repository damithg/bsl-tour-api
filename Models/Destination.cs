using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace BSLTours.API.Models
{
    public class Destination
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Excerpt { get; set; }

        // Hero Section
        public string ShortDescription { get; set; }
        public bool Featured { get; set; }
        public GalleryImage HeroImage { get; set; }
        public List<string> Tags { get; set; }


        // Location information
        public string Region { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }

        // Overview with subsections
        public OverviewBlock Overview { get; set; }

        // Features Section (with custom title)
        public FeatureSection FeaturesSection { get; set; }

        // Highlights
        public List<string> Highlights { get; set; }

        // Gallery
        public List<GalleryImage> Images { get; set; }

        // Local Experiences
        public List<Experience> Experiences { get; set; }

        // Quote block
        public QuoteBlock QuoteBlock { get; set; }

        // Video section
        public VideoBlock Video { get; set; }

        // Travel tips
        public List<string> TravelTips { get; set; }

        // FAQs
        public List<FAQ> FAQs { get; set; }

        // Essential info
        public EssentialInfo EssentialInfo { get; set; }

        // Weather info
        public WeatherInfo Weather { get; set; }

        public string RecommendedDuration { get; set; }

        // Nearby Attractions with images and links
        public List<NearbyAttraction> NearbyAttractions { get; set; }

        // Social
        public SocialLinks SocialMedia { get; set; }

        public List<RelatedTour> RelatedTours { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


    public class OverviewBlock
    {
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public GalleryImage Image { get; set; }
        public List<OverviewSubSection> SubSections { get; set; }
    }

    public class OverviewSubSection
    {
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public GalleryImage Image { get; set; }
    }


    public class FeatureSection
    {
        public string Title { get; set; }
        public List<Feature> Items { get; set; }
    }

    public class Feature
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public GalleryImage Image { get; set; }
    }

    public class Experience
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class QuoteBlock
    {
        public string Content { get; set; }
        public string Author { get; set; }
    }

    public class VideoBlock
    {
        public string Title { get; set; }
        public string VideoUrl { get; set; }
    }

    public class FAQ
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }


    public class EssentialInfo
    {
        public Dictionary<string, string> OpeningHours { get; set; }
        public EntranceFees EntranceFees { get; set; }
        public string BestTimeToVisit { get; set; }
        public string NearestAirport { get; set; }
        public List<string> TransportOptions { get; set; }
        public List<string> Accessibility { get; set; }
        public List<string> TravelTips { get; set; }
    }

    public class CurrencyInfo
    {
        public string Primary { get; set; }
        public string Local { get; set; }
    }

    public class WeatherInfo
    {
        public string Summary { get; set; }
        public string TemperatureRange { get; set; }
        public string RainySeason { get; set; }
    }

    public class NearbyAttraction
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Distance { get; set; }
        public string Link { get; set; }
        public GalleryImage Image { get; set; }
    }

    public class SocialLinks
    {
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
    }


    public class GalleryImage
    {
        public string PublicId { get; set; } // E.g. "destinations/sacred-city-of-anuradhapura-4"
        public string Alt { get; set; }
        public string Caption { get; set; }
        public string Orientation { get; set; } // landscape / portrait / square (optional)

        private const string CloudName = "drsjp6bqz";

        public string BaseUrl => $"https://res.cloudinary.com/{CloudName}/image/upload/{PublicId}.jpg";

        public string Small => Transform("w_400,h_300,c_fill");
        public string Medium => Transform("w_800,h_600,c_fill");
        public string Large => Transform("w_1600,h_900,c_fill");

        private string Transform(string transformation)
        {
            return $"https://res.cloudinary.com/{CloudName}/image/upload/{transformation}/{PublicId}.jpg";
        }
    }

    public class Section
    {
        public string Type { get; set; } // e.g., "text", "quote", "video"
        public string Title { get; set; } // Optional: for section title
        public string Content { get; set; } // Main content or URL for videos
        public string Author { get; set; } // Optional: for quotes
    }

    public class LocalExperience
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class RelatedTour
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Excerpt { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }


    public class DestinationImageSet
    {
        public string BaseUrl { get; set; } // e.g., "https://res.cloudinary.com/mycloud/image/upload/v1711738476/galle-fort.jpg"
        public string PublicId { get; set; } // optional, e.g. "galle-fort"

        // Optional predefined variants
        public string Thumbnail => Transform("w_300,h_200,c_fill");
        public string Card => Transform("w_600,h_400,c_fill");
        public string Banner => Transform("w_1600,h_800,c_fill");

        private string Transform(string transformation)
        {
            if (string.IsNullOrEmpty(BaseUrl)) return null;
            return BaseUrl.Replace("/upload/", $"/upload/{transformation}/");
        }
    }



    public class CreateDestinationDto
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Excerpt { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DestinationImageSet Images { get; set; }
        public bool Featured { get; set; }

        // Location
        public string Region { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        // SEO
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public List<string> MetaKeywords { get; set; }
        public List<string> Tags { get; set; }
        public string Category { get; set; }

        // Practical info
        public string BestTimeToVisit { get; set; }
        public string NearestAirport { get; set; }
        public List<string> TransportOptions { get; set; }
        public List<string> TravelTips { get; set; }

        // Highlights
        public List<string> Highlights { get; set; }

        // Optional sections you might allow admins to add
        public List<Feature> Features { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }
        public List<FAQ> FAQs { get; set; }
        public List<Section> Sections { get; set; }
        public List<LocalExperience> LocalExperiences { get; set; }
        public string RecommendedDuration { get; set; }
    }
}