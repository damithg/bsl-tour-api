using BSLTours.Communications.Abstractions.Models;

namespace BSLTours.Communications.Abstractions;

/// <summary>
/// High-level email service interface for business logic
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send a standard email with subject, text and/or HTML content
    /// </summary>
    Task<EmailResult> SendEmailAsync(
        string toEmail,
        string subject,
        string? textContent = null,
        string? htmlContent = null,
        string? fromEmail = null,
        string? fromName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send an email using a provider template
    /// </summary>
    Task<EmailResult> SendTemplatedEmailAsync(
        string toEmail,
        string templateId,
        Dictionary<string, object> templateData,
        string? fromEmail = null,
        string? fromName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a contact confirmation email using the configured template
    /// </summary>
    Task<EmailResult> SendContactConfirmationAsync(
        string toEmail,
        string userName,
        CancellationToken cancellationToken = default);
}
