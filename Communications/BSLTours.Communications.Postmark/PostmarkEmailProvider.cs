using BSLTours.Communications.Abstractions;
using BSLTours.Communications.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace BSLTours.Communications.Postmark;

/// <summary>
/// Postmark implementation of the email provider
/// </summary>
public class PostmarkEmailProvider : IEmailProvider
{
    private readonly PostmarkClient _client;
    private readonly PostmarkOptions _options;
    private readonly ILogger<PostmarkEmailProvider> _logger;

    public PostmarkEmailProvider(
        IOptions<PostmarkOptions> options,
        ILogger<PostmarkEmailProvider> logger)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(_options.ServerToken))
        {
            throw new InvalidOperationException("Postmark Server Token is not configured");
        }

        _client = new PostmarkClient(_options.ServerToken);
    }

    public async Task<EmailResult> SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get from address
            var from = message.From ?? new Abstractions.Models.EmailAddress(
                _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                _options.DefaultFromName);

            // Postmark only supports single recipient per API call for non-batch sends
            // We'll send to the first recipient and log if there are multiple
            if (!message.To.Any())
            {
                throw new InvalidOperationException("No recipients specified");
            }

            var primaryRecipient = message.To.First();

            if (message.To.Count > 1)
            {
                _logger.LogWarning("Postmark standard send only supports single recipient. Sending to first recipient only. Consider using batch send.");
            }

            var postmarkMessage = new PostmarkMessage
            {
                From = string.IsNullOrWhiteSpace(from.Name)
                    ? from.Email
                    : $"{from.Name} <{from.Email}>",
                To = string.IsNullOrWhiteSpace(primaryRecipient.Name)
                    ? primaryRecipient.Email
                    : $"{primaryRecipient.Name} <{primaryRecipient.Email}>",
                Subject = message.Subject,
                TextBody = message.TextContent,
                HtmlBody = message.HtmlContent
            };

            // Add CC recipients
            if (message.Cc.Any())
            {
                postmarkMessage.Cc = string.Join(", ", message.Cc.Select(cc =>
                    string.IsNullOrWhiteSpace(cc.Name) ? cc.Email : $"{cc.Name} <{cc.Email}>"));
            }

            // Add BCC recipients
            if (message.Bcc.Any())
            {
                postmarkMessage.Bcc = string.Join(", ", message.Bcc.Select(bcc =>
                    string.IsNullOrWhiteSpace(bcc.Name) ? bcc.Email : $"{bcc.Name} <{bcc.Email}>"));
            }

            var response = await _client.SendMessageAsync(postmarkMessage);

            if (response.Status == PostmarkStatus.Success)
            {
                _logger.LogInformation("Email sent successfully via Postmark. MessageID: {MessageId}", response.MessageID);
                return EmailResult.Success("Email sent successfully", 200);
            }
            else
            {
                _logger.LogError("Failed to send email via Postmark. Status: {Status}, Message: {Message}, ErrorCode: {ErrorCode}",
                    response.Status, response.Message, response.ErrorCode);
                return EmailResult.Failure(
                    $"Failed to send email: {response.Message}",
                    $"Error Code: {response.ErrorCode}",
                    (int)response.ErrorCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email via Postmark");
            return EmailResult.Failure("Exception occurred while sending email", ex.Message);
        }
    }

    public async Task<EmailResult> SendTemplatedEmailAsync(TemplatedEmailMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get from address
            var from = message.From ?? new Abstractions.Models.EmailAddress(
                _options.DefaultFromEmail ?? throw new InvalidOperationException("No from email specified"),
                _options.DefaultFromName);

            // Postmark only supports single recipient per API call for non-batch sends
            if (!message.To.Any())
            {
                throw new InvalidOperationException("No recipients specified");
            }

            var primaryRecipient = message.To.First();

            if (message.To.Count > 1)
            {
                _logger.LogWarning("Postmark templated send only supports single recipient. Sending to first recipient only. Consider using batch send.");
            }

            var templatedMessage = new TemplatedPostmarkMessage
            {
                From = string.IsNullOrWhiteSpace(from.Name)
                    ? from.Email
                    : $"{from.Name} <{from.Email}>",
                To = string.IsNullOrWhiteSpace(primaryRecipient.Name)
                    ? primaryRecipient.Email
                    : $"{primaryRecipient.Name} <{primaryRecipient.Email}>",
                TemplateId = long.TryParse(message.TemplateId, out var templateId)
                    ? templateId
                    : throw new InvalidOperationException($"Invalid Postmark template ID: {message.TemplateId}. Must be a numeric ID."),
                TemplateModel = message.TemplateData
            };

            var response = await _client.SendMessageAsync(templatedMessage);

            if (response.Status == PostmarkStatus.Success)
            {
                _logger.LogInformation("Templated email sent successfully via Postmark. Template: {TemplateId}, MessageID: {MessageId}",
                    message.TemplateId, response.MessageID);
                return EmailResult.Success("Templated email sent successfully", 200);
            }
            else
            {
                _logger.LogError("Failed to send templated email via Postmark. Template: {TemplateId}, Status: {Status}, Message: {Message}, ErrorCode: {ErrorCode}",
                    message.TemplateId, response.Status, response.Message, response.ErrorCode);
                return EmailResult.Failure(
                    $"Failed to send templated email: {response.Message}",
                    $"Error Code: {response.ErrorCode}",
                    (int)response.ErrorCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending templated email via Postmark");
            return EmailResult.Failure("Exception occurred while sending templated email", ex.Message);
        }
    }
}
