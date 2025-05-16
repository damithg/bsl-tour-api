using System.Threading.Tasks;

public interface IEmailService
{
    Task SendContactConfirmationAsync(string toEmail, string toName, string formType);
    Task SendEmailAsync(string toEmail, string toName, string subject, string plainTextContent, string htmlContent);
    Task SendTemplatedEmailAsync(string toEmail, string toName, string templateId, object dynamicData);
}