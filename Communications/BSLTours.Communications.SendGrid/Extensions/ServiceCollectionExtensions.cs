using BSLTours.Communications.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSLTours.Communications.SendGrid.Extensions;

/// <summary>
/// Extension methods for configuring SendGrid email provider
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds SendGrid email provider to the service collection
    /// </summary>
    public static IServiceCollection AddSendGridEmailProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure options
        services.Configure<SendGridOptions>(configuration.GetSection(SendGridOptions.SectionName));

        // Register the provider
        services.AddTransient<IEmailProvider, SendGridEmailProvider>();

        return services;
    }

    /// <summary>
    /// Adds SendGrid email provider with custom configuration
    /// </summary>
    public static IServiceCollection AddSendGridEmailProvider(
        this IServiceCollection services,
        Action<SendGridOptions> configureOptions)
    {
        // Configure options
        services.Configure(configureOptions);

        // Register the provider
        services.AddTransient<IEmailProvider, SendGridEmailProvider>();

        return services;
    }
}
