using BSLTours.Communications.Abstractions.Models;

namespace BSLTours.Communications.Abstractions;

/// <summary>
/// Abstraction for email provider implementations (SendGrid, Mailgun, etc.)
/// </summary>
public interface IEmailProvider
{
    /// <summary>
    /// Send a standard email message
    /// </summary>
    Task<EmailResult> SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send an email using a template
    /// </summary>
    Task<EmailResult> SendTemplatedEmailAsync(TemplatedEmailMessage message, CancellationToken cancellationToken = default);
}
