using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSLTours.API.Models
{
    public class ContactFormRequest
    {
        [Required]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string Subject { get; set; } = "";

        [Required]
        public string Message { get; set; } = "";
    }

    public class DynamicContactFormRequest
    {
        [Required]
        public string FormType { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        public string? Name { get; set; }
        public Dictionary<string, string>? Fields { get; set; }
    }

}