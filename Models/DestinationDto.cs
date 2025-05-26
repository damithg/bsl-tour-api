using System;
using System.Collections.Generic;

namespace BSLTours.API.Models;

public class DestinationDto
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Excerpt { get; set; }
    public string ShortDescription { get; set; }
    public bool Featured { get; set; }
    public string Region { get; set; }
    public string Address { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string RecommendedDuration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime PublishedAt { get; set; }
    public OverviewDto Overview { get; set; }
    public List<SubSectionDto> SubSections { get; set; }
    public HeroImageDto HeroImage { get; set; }
    public FeaturesSectionDto FeaturesSection { get; set; }
    public List<GalleryImageDto> GalleryImages { get; set; }
    public List<FaqDto> Faqs { get; set; }
    public QuoteBlockDto QuoteBlock { get; set; }
    public VideoBlockDto VideoBlock { get; set; }
    public List<RelatedTourDto> RelatedTours { get; set; }
    public List<NearbyAttractionDto> NearbyAttractions { get; set; }
    public EssentialInfoDto EssentialInfo { get; set; }
    public CardDto Card { get; set; }
}