using System.Collections.Generic;

namespace BSLTours.API.Models;

public class RelatedExperienceDto
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string ShortSummary { get; set; }
    public string Duration { get; set; }
    public decimal? Price { get; set; }
    public string Difficulty { get; set; }

    public CardDto Card { get; set; }
    public SeoMetaDto Seo { get; set; }  // Optional if not null
}
