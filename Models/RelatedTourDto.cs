namespace BSLTours.API.Models;

public class RelatedTourDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Summary { get; set; }
    public string Duration { get; set; }
    public decimal StartingFrom { get; set; }
    public string Currency { get; set; }
    public string Link { get; set; }
    public ImageDto Image { get; set; }
}