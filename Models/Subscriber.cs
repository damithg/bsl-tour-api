using System;

namespace BSLTours.API.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSubscriberDto
    {
        public string Email { get; set; }
    }
}
