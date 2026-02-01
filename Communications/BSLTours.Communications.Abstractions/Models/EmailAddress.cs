namespace BSLTours.Communications.Abstractions.Models;

/// <summary>
/// Represents an email address with optional display name
/// </summary>
public class EmailAddress
{
    public EmailAddress(string email, string? name = null)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Name = name;
    }

    /// <summary>
    /// The email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The display name for the email address
    /// </summary>
    public string? Name { get; set; }
}
