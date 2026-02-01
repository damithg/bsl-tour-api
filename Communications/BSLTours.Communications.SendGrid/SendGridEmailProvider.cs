using BSLTours.Communications.Abstractions;
using BSLTours.Communications.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BSLTours.Communications.SendGrid;

/// <summary>
/// SendGrid implementation of the email provider
/// </summary>
public class SendGridEmailProvider : IEmailProvider
{
    private readonly SendGridClient _client;
    private readonly SendGridOptions _options;
    private readonly ILogger<SendGridEmailProvider> _logger;

    public SendGridEmailProvider(
        IOptions<SendGridOptions> options,
        ILogger<SendGridEmailProvider> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("SendGrid API key is not configured");
        }

        _client = new SendGridClient(_options.ApiKey);
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            var msg = new SendGridMessage();

            // Set from address
            var from = message.From ?? new Abstractions.Models.EmailAddress(
                _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                _options.DefaultFromName);
            msg.SetFrom(new global::SendGrid.Helpers.Mail.EmailAddress(from.Email, from.Name));

            // Set recipients
            foreach (var to in message.To)
            {
                msg.AddTo(new global::SendGrid.Helpers.Mail.EmailAddress(to.Email, to.Name));
            }

            // Set CC
            foreach (var cc in message.Cc)
            {
                msg.AddCc(new global::SendGrid.Helpers.Mail.EmailAddress(cc.Email, cc.Name));
            }

            // Set BCC
            foreach (var bcc in message.Bcc)
            {
                msg.AddBcc(new global::SendGrid.Helpers.Mail.EmailAddress(bcc.Email, bcc.Name));
            }

            // Set subject and content
            msg.SetSubject(message.Subject);

            if (!string.IsNullOrWhiteSpace(message.TextContent))
            {
                msg.AddContent(MimeType.Text, message.TextContent);
            }

            if (!string.IsNullOrWhiteSpace(message.HtmlContent))
            {
                msg.AddContent(MimeType.Html, message.HtmlContent);
            }

            var response = await _client.SendEmailAsync(msg, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully via SendGrid. Status: {StatusCode}", response.StatusCode);
                return EmailResult.Success("Email sent successfully", (int)response.StatusCode);
            }
            else
            {
                var body = await response.Body.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Failed to send email via SendGrid. Status: {StatusCode}, Body: {Body}",
                    response.StatusCode, body);
                return EmailResult.Failure(
                    "Failed to send email",
                    body,
                    (int)response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email via SendGrid");
            return EmailResult.Failure("Exception occurred while sending email", ex.Message);
        }
    }

    public async Task<EmailResult> SendTemplatedEmailAsync(TemplatedEmailMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            var msg = new SendGridMessage();

            // Set from address
            var from = message.From ?? new Abstractions.Models.EmailAddress(
                _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                _options.DefaultFromName);
            msg.SetFrom(new global::SendGrid.Helpers.Mail.EmailAddress(from.Email, from.Name));

            // Set recipients
            foreach (var to in message.To)
            {
                msg.AddTo(new global::SendGrid.Helpers.Mail.EmailAddress(to.Email, to.Name));
            }

            // Set template
            msg.SetTemplateId(message.TemplateId);

            // Set template data
            if (message.TemplateData.Any())
            {
                msg.SetTemplateData(message.TemplateData);
            }

            var response = await _client.SendEmailAsync(msg, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Templated email sent successfully via SendGrid. Template: {TemplateId}, Status: {StatusCode}",
                    message.TemplateId, response.StatusCode);
                return EmailResult.Success("Templated email sent successfully", (int)response.StatusCode);
            }
            else
            {
                var body = await response.Body.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Failed to send templated email via SendGrid. Template: {TemplateId}, Status: {StatusCode}, Body: {Body}",
                    message.TemplateId, response.StatusCode, body);
                return EmailResult.Failure(
                    "Failed to send templated email",
                    body,
                    (int)response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending templated email via SendGrid");
            return EmailResult.Failure("Exception occurred while sending templated email", ex.Message);
        }
    }
}
