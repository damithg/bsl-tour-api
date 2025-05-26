using System;

namespace BSLTours.API.Models;

public class ImageDto
{
    public string PublicId { get; set; }
    public string Alt { get; set; }
    public string Caption { get; set; }
    public string Orientation { get; set; }

    private const string CloudName = "drsjp6bqz";

    // Remove known file extensions if present
    private string CleanPublicId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(PublicId)) return string.Empty;

            return PublicId
                .Replace(".jpg", "", StringComparison.OrdinalIgnoreCase)
                .Replace(".jpeg", "", StringComparison.OrdinalIgnoreCase)
                .Replace(".png", "", StringComparison.OrdinalIgnoreCase);
        }
    }

    public string BaseUrl => $"https://res.cloudinary.com/{CloudName}/image/upload/{CleanPublicId}.jpg";
    public string Small => Transform("w_400,h_300,c_fill");
    public string Medium => Transform("w_800,h_600,c_fill");
    public string Large => Transform("w_1600,h_900,c_fill");

    private string Transform(string transformation) =>
        $"https://res.cloudinary.com/{CloudName}/image/upload/{transformation}/{CleanPublicId}.jpg";
}