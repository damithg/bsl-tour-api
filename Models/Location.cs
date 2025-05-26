namespace BSLTours.API.Models;

public class Location
{
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
}