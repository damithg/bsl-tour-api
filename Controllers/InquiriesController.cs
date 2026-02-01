using BSLTours.API.Models;
using BSLTours.API.Models.Dtos;
using BSLTours.API.Services;
using BSLTours.Communications.Abstractions;
using BSLTours.Communications.Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSLTours.API.Controllers
{
    [ApiController]
    [Route("api/inquiries")]
    public class InquiriesController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ITurnstileService _turnstileService;

        public InquiriesController(IEmailService emailService, ITurnstileService turnstileService)
        {
            _emailService = emailService;
            _turnstileService = turnstileService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateInquiry(CreateInquiryDto inquiryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Send email notification directly
            await SendLegacyInquiryNotificationEmail(inquiryDto);

            // Send auto-reply to the sender
            //await SendAutoReply(inquiryDto.Email, inquiryDto.Name);

            return Ok(new { message = "Inquiry submitted successfully" });
        }

        //[HttpPost("dynamic")]
        //public async Task<ActionResult> CreateDynamicInquiry(DynamicInquiryRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Send email notification directly
        //    await SendDynamicInquiryNotificationEmail(request);

        //    // Send auto-reply to the sender
        //    await SendAutoReply(request.Email, request.Name);

        //    return Ok(new { message = "Dynamic inquiry submitted successfully" });
        //}

        [HttpPost("comprehensive")]
        public async Task<ActionResult> CreateComprehensiveInquiry(ComprehensiveInquiryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verify Turnstile token before processing (skip if token not provided for backward compatibility)
            if (!string.IsNullOrEmpty(request.TurnstileToken))
            {
                var clientIp = GetClientIpAddress();
                var isValidToken = await _turnstileService.VerifyTokenAsync(request.TurnstileToken, clientIp);
                if (!isValidToken)
                {
                    return BadRequest(new {
                        error = "Invalid security token. Please refresh the page and try again.",
                        details = new {
                            tokenLength = request.TurnstileToken?.Length ?? 0,
                            clientIp = clientIp,
                            timestamp = DateTime.UtcNow
                        }
                    });
                }
            }

            // Send email notification directly
            await SendComprehensiveInquiryNotificationEmail(request);

            // Send auto-reply to the sender
            var fullName = $"{request.FirstName} {request.LastName}".Trim();
            await SendAutoReply(request);

            return Ok(new { message = "Comprehensive inquiry submitted successfully" });
        }

        private async Task SendLegacyInquiryNotificationEmail(CreateInquiryDto inquiry)
        {
            try
            {
                var subject = $"New Inquiry from {inquiry.Name ?? inquiry.Email}";

                var emailBody = $@"
                    <h2>New Legacy Inquiry</h2>
                    <p><strong>From:</strong> {inquiry.Name} ({inquiry.Email})</p>
                    <p><strong>Phone:</strong> {inquiry.Phone ?? "Not provided"}</p>
                    <p><strong>Message:</strong> {inquiry.Message}</p>
                    <p><strong>Tour Interest:</strong> {inquiry.TourInterest ?? "Not specified"}</p>
                    <p><strong>Travel Date:</strong> {(inquiry.TravelDate.ToString("yyyy-MM-dd") ?? "Not specified")}</p>
                    <p><strong>Party Size:</strong> {(inquiry.TravelPartySize.ToString() ?? "Not specified")}</p>
                    <p><strong>Submitted:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                ";

                await _emailService.SendEmailAsync(
                    toEmail: "info@siprea.com",
                    subject: subject,
                    textContent: ConvertHtmlToPlainText(emailBody),
                    htmlContent: emailBody
                );
            }
            catch (Exception ex)
            {
                // Log error but don't fail the inquiry creation
                // Could add proper logging here
            }
        }

        private async Task SendDynamicInquiryNotificationEmail(DynamicInquiryRequest request)
        {
            try
            {
                var subject = $"New {request.InquiryType} Inquiry from {request.Name ?? request.Email}";

                var emailBody = $@"
                    <h2>New {request.InquiryType} Inquiry</h2>
                    <p><strong>From:</strong> {request.Name} ({request.Email})</p>
                    <p><strong>Phone:</strong> {request.Phone ?? "Not provided"}</p>
                    <p><strong>Message:</strong> {request.Message}</p>
                    <p><strong>Tour Interest:</strong> {request.TourInterest ?? "Not specified"}</p>
                    <p><strong>Travel Date:</strong> {(request.TravelDate?.ToString("yyyy-MM-dd") ?? "Not specified")}</p>
                    <p><strong>Party Size:</strong> {(request.TravelPartySize?.ToString() ?? "Not specified")}</p>
                ";

                if (request.AdditionalFields != null && request.AdditionalFields.Any())
                {
                    emailBody += "<h3>Additional Information:</h3>";
                    foreach (var field in request.AdditionalFields)
                    {
                        emailBody += $"<p><strong>{field.Key}:</strong> {field.Value}</p>";
                    }
                }

                emailBody += $"<p><strong>Submitted:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>";

                await _emailService.SendEmailAsync(
                    toEmail: "info@siprea.com",
                    subject: subject,
                    textContent: ConvertHtmlToPlainText(emailBody),
                    htmlContent: emailBody
                );
            }
            catch (Exception ex)
            {
                // Log error but don't fail the inquiry creation
                // Could add proper logging here
            }
        }

        private async Task SendComprehensiveInquiryNotificationEmail(ComprehensiveInquiryRequest request)
        {
            try
            {
                var fullName = $"{request.FirstName} {request.LastName}".Trim();
                var subject = $"New {request.InquiryType} Inquiry from {fullName}";

                var travelTimeframe = request.TravelPlanning.FlexibleDates
                    ? request.TravelPlanning.TravelDates ?? "Flexible dates"
                    : request.TravelPlanning.TravelMonth ?? "Specific dates";

                var totalTravelers = request.TravelPlanning.Adults + request.TravelPlanning.Children;

                var emailBody = $@"
                    <h2>New {request.InquiryType} Inquiry</h2>
                    <p><strong>From:</strong> {fullName} ({request.Email})</p>
                    <p><strong>Phone:</strong> {request.Phone ?? "Not provided"}</p>
                    <p><strong>Subject:</strong> {request.SubjectName ?? "General Inquiry"}</p>
                    <p><strong>Message:</strong> {request.Message}</p>

                    <h3>Travel Planning Details</h3>
                    <p><strong>Travel Dates:</strong> {travelTimeframe}</p>
                    <p><strong>Total Travelers:</strong> {totalTravelers} ({request.TravelPlanning.Adults} adults, {request.TravelPlanning.Children} children)</p>
                    <p><strong>Flexible Dates:</strong> {(request.TravelPlanning.FlexibleDates ? "Yes" : "No")}</p>

                    <h3>Additional Information</h3>
                    <p><strong>How they heard about us:</strong> {request.HearAboutUs ?? "Not specified"}</p>
                    <p><strong>Newsletter subscription:</strong> {(request.Subscribed ? "Yes" : "No")}</p>

                    <h3>Technical Details</h3>
                    <p><strong>Form Source:</strong> {request.FormSource ?? "Not specified"}</p>
                    <p><strong>IP Address:</strong> {request.IpAddress ?? "Not captured"}</p>
                    <p><strong>User Agent:</strong> {request.UserAgent ?? "Not captured"}</p>
                    <p><strong>Referrer:</strong> {request.Referrer ?? "Direct"}</p>

                    <p><strong>Submitted:</strong> {request.SubmittedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} UTC</p>
                ";

                await _emailService.SendEmailAsync(
                    toEmail: "info@bestsrilankatours.com",
                    subject: subject,
                    textContent: ConvertHtmlToPlainText(emailBody),
                    htmlContent: emailBody
                );
            }
            catch (Exception ex)
            {
                // Log error but don't fail the inquiry creation
                // Could add proper logging here
            }
        }

        private string ConvertHtmlToPlainText(string html)
        {
            return html
                .Replace("<h2>", "")
                .Replace("</h2>", "\n")
                .Replace("<h3>", "")
                .Replace("</h3>", "\n")
                .Replace("<p>", "")
                .Replace("</p>", "\n")
                .Replace("<strong>", "")
                .Replace("</strong>", "");
        }

        private async Task SendAutoReply(ComprehensiveInquiryRequest inquiry)
        {
            try
            {
                const string templateId = "41894431"; // your Postmark template ID

                var submittedAt = inquiry.SubmittedAt ?? DateTime.UtcNow;

                // Build the data model Postmark will merge into {{ ... }} in the template
                var templateData = new Dictionary<string, object>
                {
                    // Used in subject + header
                    ["guestName"] = inquiry.FirstName,                                  // "John"
                    ["tourName"] = inquiry.SubjectName ?? "Your Sri Lanka trip",        // "7-Day Cultural Heritage Tour"

                    // Trip details
                    ["travelDates"] = inquiry.TravelPlanning?.TravelDates
                                      ?? "Dates not specified",
                    ["adults"] = inquiry.TravelPlanning?.Adults ?? 0,
                    ["children"] = inquiry.TravelPlanning?.Children ?? 0,

                    // Customer's note to us
                    ["enquiryMessage"] = inquiry.Message,

                    // Contact info
                    ["guestEmail"] = inquiry.Email,
                    ["guestPhone"] = inquiry.Phone ?? string.Empty,

                    // Marketing insight
                    ["hearAboutUs"] = inquiry.HearAboutUs ?? "Not specified",

                    // Optional extras you might show in footer
                    ["submitted_date"] = submittedAt.ToString("MMMM dd, yyyy")
                };

                await _emailService.SendTemplatedEmailAsync(
                    toEmail: inquiry.Email,
                    templateId: templateId,
                    templateData: templateData
                );
            }
            catch (Exception ex)
            {
                // swallow but log
                //_logger.LogError(ex,
                //    "Failed to send auto-reply email to {Email} for subject {SubjectName}",
                //    inquiry.Email,
                //    inquiry.SubjectName
                //);
            }
        }



        private string? GetClientIpAddress()
        {
            // Try to get the real IP from headers first (for proxies/load balancers)
            var xForwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                return xForwardedFor.Split(',')[0].Trim();
            }

            var xRealIp = Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(xRealIp))
            {
                return xRealIp;
            }

            // Fallback to connection remote IP
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}