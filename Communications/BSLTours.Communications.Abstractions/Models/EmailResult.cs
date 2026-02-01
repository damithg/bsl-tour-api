namespace BSLTours.Communications.Abstractions.Models;

/// <summary>
/// Represents the result of an email send operation
/// </summary>
public class EmailResult
{
    public EmailResult(bool isSuccess, string? message = null, string? errorDetails = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorDetails = errorDetails;
    }

    /// <summary>
    /// Indicates whether the email was sent successfully
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// A message describing the result
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Detailed error information if the operation failed
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// The HTTP status code from the provider (if applicable)
    /// </summary>
    public int? StatusCode { get; set; }

    public static EmailResult Success(string? message = null, int? statusCode = null)
    {
        return new EmailResult(true, message) { StatusCode = statusCode };
    }

    public static EmailResult Failure(string message, string? errorDetails = null, int? statusCode = null)
    {
        return new EmailResult(false, message, errorDetails) { StatusCode = statusCode };
    }
}
