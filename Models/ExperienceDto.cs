using System;
using System.Collections.Generic;

namespace BSLTours.API.Models;

public class ExperienceDto
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string ShortSummary { get; set; }
    public string Duration { get; set; }
    public decimal? Price { get; set; }
    public string Difficulty { get; set; }
    public string Description { get; set; }
    public bool Featured { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public LocationDto Location { get; set; }
    public List<string> Highlights { get; set; }
    public List<string> Inclusions { get; set; }
    public List<string> WhatToBring { get; set; }

    public SeoMetaDto Seo { get; set; }
    public CardDto Card { get; set; }
    public List<GalleryImageDto> GalleryImages { get; set; }

    public List<RelatedExperienceDto> RelatedExperiences { get; set; }
}
