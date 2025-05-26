using System.Collections.Generic;

namespace BSLTours.API.Models;

public class Experience
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string ShortSummary { get; set; }
    public string Duration { get; set; }
    public decimal? Price { get; set; }
    public string Difficulty { get; set; }
    public string Description { get; set; }
    public bool Featured { get; set; }
    public Location Location { get; set; }
    public List<TextItem> Highlights { get; set; }
    public List<TextItem> Inclusions { get; set; }
    public List<TextItem> WhatToBring { get; set; }
    public SeoMeta Seo { get; set; }
    public CardData Card { get; set; }
    public List<GalleryImage> GalleryImages { get; set; }
    public List<Experience> RelatedExperiences { get; set; }
}