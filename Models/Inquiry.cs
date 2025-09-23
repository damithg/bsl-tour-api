using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSLTours.API.Models
{
    public class Inquiry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string TourInterest { get; set; }
        public DateTime TravelDate { get; set; }
        public int TravelPartySize { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }

        // New dynamic fields support
        public string InquiryType { get; set; } = "General";
        public string AdditionalFieldsJson { get; set; } // Store as JSON string

        [JsonIgnore] // Don't serialize this in API responses
        public Dictionary<string, string> AdditionalFields
        {
            get
            {
                if (string.IsNullOrEmpty(AdditionalFieldsJson))
                    return new Dictionary<string, string>();

                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(AdditionalFieldsJson);
                }
                catch
                {
                    return new Dictionary<string, string>();
                }
            }
            set
            {
                AdditionalFieldsJson = value == null ? null : System.Text.Json.JsonSerializer.Serialize(value);
            }
        }
    }

    // Legacy DTO for backward compatibility
    public class CreateInquiryDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string TourInterest { get; set; }
        public DateTime TravelDate { get; set; }
        public int TravelPartySize { get; set; }
    }

    // New dynamic inquiry DTO
    public class DynamicInquiryRequest
    {
        [Required]
        public string InquiryType { get; set; } = "General";

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? Name { get; set; }
        public string? Message { get; set; }

        // Core fields that might be commonly used
        public string? Phone { get; set; }
        public string? TourInterest { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? TravelPartySize { get; set; }

        // Dynamic fields for any additional data
        public Dictionary<string, string>? AdditionalFields { get; set; }
    }

    // Enhanced response DTO with dynamic fields
    public class InquiryResponseDto
    {
        public int Id { get; set; }
        public string InquiryType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public string TourInterest { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? TravelPartySize { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
    }
}
