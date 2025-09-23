using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSLTours.API.Models
{
    // Comprehensive inquiry request matching your exact payload structure
    public class ComprehensiveInquiryRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        public string InquiryType { get; set; } // "tour", "destination", "experience", "general"

        public string? SubjectName { get; set; }

        public int? SubjectId { get; set; }

        [Required]
        public TravelPlanningInfo TravelPlanning { get; set; }

        [Required]
        public string Message { get; set; }

        public string? HearAboutUs { get; set; }

        public bool Subscribed { get; set; } = false;

        // Metadata fields (auto-populated by frontend)
        public DateTime? SubmittedAt { get; set; }
        public string? UserAgent { get; set; }
        public string? Referrer { get; set; }
        public string? IpAddress { get; set; }
        public string? FormSource { get; set; }
    }

    public class TravelPlanningInfo
    {
        public bool FlexibleDates { get; set; }

        public string? TravelMonth { get; set; }

        public string? TravelDates { get; set; }

        [Required]
        public int Adults { get; set; }

        public int Children { get; set; } = 0;
    }

    // Enhanced inquiry model to store comprehensive data
    public class ComprehensiveInquiry
    {
        public int Id { get; set; }

        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        // Inquiry Details
        public string InquiryType { get; set; }
        public string? SubjectName { get; set; }
        public int? SubjectId { get; set; }
        public string Message { get; set; }

        // Travel Planning (stored as JSON)
        public string TravelPlanningJson { get; set; }

        // Marketing & Preferences
        public string? HearAboutUs { get; set; }
        public bool Subscribed { get; set; }

        // Metadata
        public DateTime SubmittedAt { get; set; }
        public string? UserAgent { get; set; }
        public string? Referrer { get; set; }
        public string? IpAddress { get; set; }
        public string? FormSource { get; set; }

        // Administrative
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
        public string? AdminNotes { get; set; }

        // Travel Planning property for easy access
        [JsonIgnore]
        public TravelPlanningInfo TravelPlanning
        {
            get
            {
                if (string.IsNullOrEmpty(TravelPlanningJson))
                    return new TravelPlanningInfo();

                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<TravelPlanningInfo>(TravelPlanningJson);
                }
                catch
                {
                    return new TravelPlanningInfo();
                }
            }
            set
            {
                TravelPlanningJson = value == null ? null : System.Text.Json.JsonSerializer.Serialize(value);
            }
        }

        // Computed properties for convenience
        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}".Trim();

        [JsonIgnore]
        public int TotalTravelers => TravelPlanning.Adults + TravelPlanning.Children;

        [JsonIgnore]
        public string TravelTimeframe => TravelPlanning.FlexibleDates
            ? TravelPlanning.TravelDates ?? "Flexible dates"
            : TravelPlanning.TravelMonth ?? "Specific dates";
    }

    // Response DTO for comprehensive inquiries
    public class ComprehensiveInquiryResponseDto
    {
        public int Id { get; set; }

        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        // Inquiry Details
        public string InquiryType { get; set; }
        public string? SubjectName { get; set; }
        public int? SubjectId { get; set; }
        public string Message { get; set; }

        // Travel Planning
        public TravelPlanningInfo TravelPlanning { get; set; }
        public int TotalTravelers { get; set; }
        public string TravelTimeframe { get; set; }

        // Marketing & Preferences
        public string? HearAboutUs { get; set; }
        public bool Subscribed { get; set; }

        // Metadata
        public DateTime SubmittedAt { get; set; }
        public string? UserAgent { get; set; }
        public string? Referrer { get; set; }
        public string? IpAddress { get; set; }
        public string? FormSource { get; set; }

        // Administrative
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
        public string? AdminNotes { get; set; }
    }
}