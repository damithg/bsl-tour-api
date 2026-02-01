namespace BSLTours.Communications.Postmark;

/// <summary>
/// Configuration options for Postmark email provider
/// </summary>
public class PostmarkOptions
{
    public const string SectionName = "Postmark";

    /// <summary>
    /// Postmark Server API Token (should be stored in environment variables or secrets)
    /// </summary>
    public string ServerToken { get; set; } = string.Empty;

    /// <summary>
    /// Default from email address
    /// </summary>
    public string? DefaultFromEmail { get; set; }

    /// <summary>
    /// Default from name
    /// </summary>
    public string? DefaultFromName { get; set; }
}
