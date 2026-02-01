namespace BSLTours.Communications.Abstractions.Models;

/// <summary>
/// Represents a standard email message
/// </summary>
public class EmailMessage
{
    public EmailMessage()
    {
        To = new List<EmailAddress>();
        Cc = new List<EmailAddress>();
        Bcc = new List<EmailAddress>();
    }

    /// <summary>
    /// The sender's email address
    /// </summary>
    public EmailAddress? From { get; set; }

    /// <summary>
    /// List of recipients
    /// </summary>
    public List<EmailAddress> To { get; set; }

    /// <summary>
    /// List of CC recipients
    /// </summary>
    public List<EmailAddress> Cc { get; set; }

    /// <summary>
    /// List of BCC recipients
    /// </summary>
    public List<EmailAddress> Bcc { get; set; }

    /// <summary>
    /// Email subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Plain text content
    /// </summary>
    public string? TextContent { get; set; }

    /// <summary>
    /// HTML content
    /// </summary>
    public string? HtmlContent { get; set; }
}
