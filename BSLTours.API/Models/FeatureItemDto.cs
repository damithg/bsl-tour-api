namespace BSLTours.API.Models;

public class FeatureItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public ImageDto Image { get; set; }
}