namespace BSLTours.Communications.SendGrid;

/// <summary>
/// Configuration options for SendGrid email provider
/// </summary>
public class SendGridOptions
{
    public const string SectionName = "SendGrid";

    /// <summary>
    /// SendGrid API key (should be stored in environment variables or secrets)
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Default from email address
    /// </summary>
    public string? DefaultFromEmail { get; set; }

    /// <summary>
    /// Default from name
    /// </summary>
    public string? DefaultFromName { get; set; }
}
