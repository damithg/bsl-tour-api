using System.Collections.Generic;

namespace BSLTours.API.Models
{
    public class DataWrapper<T>
    {
        public int Id { get; set; }
        public T Attributes { get; set; }
    }

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

        public LocationWrapper Location { get; set; }
        public List<TextItem> Highlights { get; set; }
        public List<TextItem> Inclusions { get; set; }
        public List<TextItem> WhatToBring { get; set; }

        public SeoMeta Seo { get; set; }
        public CardData Card { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }
    }

    // Sub-models

    public class LocationWrapper
    {
        public DataWrapper<LocationAttributes> Data { get; set; }
    }

    public class LocationAttributes
    {
        public string Name { get; set; }
    }

    public class TextItem
    {
        public string Text { get; set; }
    }

    public class SeoMeta
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
    }

    public class CardData
    {
        public string Header { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string[] Tags { get; set; }
        public GalleryImage Image { get; set; }
    }

    public class ExperienceDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }


        public List<HighlightDto> Highlights { get; set; }
        public List<InclusionDto> Inclusions { get; set; }
        public List<WhatToBringDto> WhatToBring { get; set; }
        public SeoMetaDto Seo { get; set; }
        public List<GalleryImageDto> GalleryImages { get; set; }
        public CardDto Card { get; set; }
    }

    public class HighlightDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class InclusionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class WhatToBringDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }


    public class SeoMetaDto
    {
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
    }


}
