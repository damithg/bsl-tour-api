using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using BSLTours.API.Models;
using BSLTours.Communications.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> HandleDynamicContactForm([FromBody] DynamicContactFormRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append($"<p><strong>Form Type:</strong> {request.FormType}</p>");
            htmlBuilder.Append($"<p><strong>Email:</strong> {request.Email}</p>");
            if (!string.IsNullOrWhiteSpace(request.Name))
                htmlBuilder.Append($"<p><strong>Name:</strong> {request.Name}</p>");

            if (request.Fields != null)
            {
                foreach (var field in request.Fields)
                {
                    htmlBuilder.Append($"<p><strong>{field.Key}:</strong> {field.Value}</p>");
                }
            }

            string htmlContent = htmlBuilder.ToString();
            string plainTextContent = Regex.Replace(htmlContent, "<.*?>", string.Empty);

            var subject = $"New Form Submission: {request.FormType}";

            // 1. Send internal notification
            await _emailService.SendEmailAsync(
                toEmail: "info@siprea.com",
                subject: subject,
                textContent: plainTextContent,
                htmlContent: htmlContent
            );

            // 2. Optionally, send user confirmation using template
            await _emailService.SendContactConfirmationAsync(
                toEmail: request.Email,
                userName: request.Name ?? request.Email
            );

            return Ok(new { success = true });
        }
    }
}
