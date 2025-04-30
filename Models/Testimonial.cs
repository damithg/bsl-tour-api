using System;

namespace BSLTours.API.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAvatar { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string TourPackage { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTestimonialDto
    {
        public string CustomerName { get; set; }
        public string CustomerAvatar { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public string TourPackage { get; set; }
    }
}
