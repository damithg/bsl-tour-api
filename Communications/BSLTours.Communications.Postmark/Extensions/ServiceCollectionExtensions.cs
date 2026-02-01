using BSLTours.Communications.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BSLTours.Communications.Postmark.Extensions;

/// <summary>
/// Extension methods for configuring Postmark email provider
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Postmark email provider to the service collection
    /// </summary>
    public static IServiceCollection AddPostmarkEmailProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure options
        services.Configure<PostmarkOptions>(configuration.GetSection(PostmarkOptions.SectionName));

        // Register the provider
        services.AddTransient<IEmailProvider, PostmarkEmailProvider>();

        return services;
    }

    /// <summary>
    /// Adds Postmark email provider with custom configuration
    /// </summary>
    public static IServiceCollection AddPostmarkEmailProvider(
        this IServiceCollection services,
        Action<PostmarkOptions> configureOptions)
    {
        // Configure options
        services.Configure(configureOptions);

        // Register the provider
        services.AddTransient<IEmailProvider, PostmarkEmailProvider>();

        return services;
    }
}
