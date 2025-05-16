using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISendGridClient _sendGridClient;
    private readonly EmailAddress _fromEmail;
    private readonly string _contactConfirmationTemplateId;

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        var apiKey = _configuration["SendGrid:ApiKey"];
        _contactConfirmationTemplateId = _configuration["SendGrid:Templates:ContactConfirmation"];

        _sendGridClient = new SendGridClient(apiKey);
        _fromEmail = new EmailAddress(
            _configuration["SendGrid:FromEmail"] ?? "noreply@example.com",
            _configuration["SendGrid:FromName"] ?? "BSL Tours"
        );
    }

    public async Task SendContactConfirmationAsync(string toEmail, string toName, string formType)
    {
        var dynamicData = new
        {
            name = toName,
            formType = formType
        };

        await SendTemplatedEmailAsync(
            toEmail: toEmail,
            toName: toName,
            templateId: _contactConfirmationTemplateId,
            dynamicData: dynamicData
        );
    }


    public async Task SendEmailAsync(string toEmail, string toName, string subject, string plainTextContent, string htmlContent)
    {
        var to = new EmailAddress(toEmail, toName);
        var msg = MailHelper.CreateSingleEmail(_fromEmail, to, subject, plainTextContent, htmlContent);

        try
        {
            var response = await _sendGridClient.SendEmailAsync(msg);
            if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogWarning("Failed to send email. Status: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email to {Email}", toEmail);
            throw;
        }
    }

    public async Task SendTemplatedEmailAsync(string toEmail, string toName, string templateId, object dynamicData)
    {
        var to = new EmailAddress(toEmail, toName);
        var msg = new SendGridMessage
        {
            From = _fromEmail,
            TemplateId = templateId
        };
        msg.AddTo(to);
        msg.SetTemplateData(dynamicData);

        try
        {
            var response = await _sendGridClient.SendEmailAsync(msg);
            if ((int)response.StatusCode >= 400)
            {
                _logger.LogWarning("Failed to send templated email. Status: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending templated email to {Email}", toEmail);
            throw;
        }
    }

}