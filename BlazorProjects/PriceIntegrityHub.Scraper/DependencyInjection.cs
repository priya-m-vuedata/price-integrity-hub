using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Scraper.Configuration;

namespace PriceIntegrityHub.Scraper;

/// <summary>
/// Extension methods for registering Scraper services with dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Scraper layer services to the service collection.
    /// </summary>
    public static IServiceCollection AddScraperServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration options
        services.Configure<ScraperOptions>(
            configuration.GetSection(ScraperOptions.SectionName));

        // Register services
        services.AddScoped<IWebScraper, SeleniumWebScraper>();

        return services;
    }
}
