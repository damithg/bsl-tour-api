using System.Collections.Generic;

namespace BSLTours.API.Models;

public class FeaturesSectionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<FeatureItemDto> Items { get; set; }
}