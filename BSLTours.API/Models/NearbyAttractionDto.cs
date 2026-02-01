namespace BSLTours.API.Models;

public class NearbyAttractionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string Distance { get; set; }
    public string Link { get; set; }
    public ImageDto Image { get; set; }
}