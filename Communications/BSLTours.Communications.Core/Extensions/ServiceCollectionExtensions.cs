using BSLTours.Communications.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSLTours.Communications.Core.Extensions;

/// <summary>
/// Extension methods for configuring email service
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the email service to the service collection
    /// </summary>
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure options
        services.Configure<EmailServiceOptions>(configuration.GetSection(EmailServiceOptions.SectionName));

        // Register the service
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }

    /// <summary>
    /// Adds the email service with custom configuration
    /// </summary>
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        Action<EmailServiceOptions> configureOptions)
    {
        // Configure options
        services.Configure(configureOptions);

        // Register the service
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
