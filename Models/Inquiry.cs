using System;

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
    }

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
}
