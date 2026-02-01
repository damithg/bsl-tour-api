using BSLTours.Communications.Abstractions;
using BSLTours.Communications.Abstractions.Models;
using Microsoft.Extensions.Options;

namespace BSLTours.Communications.Core;

/// <summary>
/// High-level email service that orchestrates email sending through providers
/// </summary>
public class EmailService : IEmailService
{
    private readonly IEmailProvider _emailProvider;
    private readonly EmailServiceOptions _options;

    public EmailService(
        IEmailProvider emailProvider,
        IOptions<EmailServiceOptions> options)
    {
        _emailProvider = emailProvider ?? throw new ArgumentNullException(nameof(emailProvider));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<EmailResult> SendEmailAsync(
        string toEmail,
        string subject,
        string? textContent = null,
        string? htmlContent = null,
        string? fromEmail = null,
        string? fromName = null,
        CancellationToken cancellationToken = default)
    {
        var message = new EmailMessage
        {
            From = new EmailAddress(
                fromEmail ?? _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                fromName ?? _options.DefaultFromName),
            Subject = subject,
            TextContent = textContent,
            HtmlContent = htmlContent
        };

        message.To.Add(new EmailAddress(toEmail));

        return await _emailProvider.SendEmailAsync(message, cancellationToken);
    }

    public async Task<EmailResult> SendTemplatedEmailAsync(
        string toEmail,
        string templateId,
        Dictionary<string, object> templateData,
        string? fromEmail = null,
        string? fromName = null,
        CancellationToken cancellationToken = default)
    {
        var message = new TemplatedEmailMessage
        {
            From = new EmailAddress(
                fromEmail ?? _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                fromName ?? _options.DefaultFromName),
            TemplateId = templateId,
            TemplateData = templateData
        };

        message.To.Add(new EmailAddress(toEmail));

        return await _emailProvider.SendTemplatedEmailAsync(message, cancellationToken);
    }

    public async Task<EmailResult> SendContactConfirmationAsync(
        string toEmail,
        string userName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ContactConfirmationTemplateId))
        {
            throw new InvalidOperationException("Contact confirmation template ID is not configured");
        }

        var templateData = new Dictionary<string, object>
        {
            { "userName", userName }
        };

        return await SendTemplatedEmailAsync(
            toEmail,
            _options.ContactConfirmationTemplateId,
            templateData,
            cancellationToken: cancellationToken);
    }
}
