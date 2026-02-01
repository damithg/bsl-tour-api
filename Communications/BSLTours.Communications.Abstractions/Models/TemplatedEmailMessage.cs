namespace BSLTours.Communications.Abstractions.Models;

/// <summary>
/// Represents an email message using a template
/// </summary>
public class TemplatedEmailMessage
{
    public TemplatedEmailMessage()
    {
        To = new List<EmailAddress>();
        TemplateData = new Dictionary<string, object>();
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
    /// The template ID from the email provider
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;

    /// <summary>
    /// Dynamic data to be substituted in the template
    /// </summary>
    public Dictionary<string, object> TemplateData { get; set; }
}
