namespace BSLTours.Communications.Core;

/// <summary>
/// Configuration options for the email service
/// </summary>
public class EmailServiceOptions
{
    public const string SectionName = "EmailService";

    /// <summary>
    /// Default from email address
    /// </summary>
    public string? DefaultFromEmail { get; set; }

    /// <summary>
    /// Default from name
    /// </summary>
    public string? DefaultFromName { get; set; }

    /// <summary>
    /// Contact confirmation template ID
    /// </summary>
    public string? ContactConfirmationTemplateId { get; set; }
}
