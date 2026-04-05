using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceIntegrityHub.Core.Configuration;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Services;

namespace PriceIntegrityHub.Core;

/// <summary>
/// Extension methods for registering Core services with dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Core layer services to the service collection.
    /// </summary>
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration options
        services.Configure<ComparisonOptions>(
            configuration.GetSection(ComparisonOptions.SectionName));

        // Register services
        services.AddScoped<IPriceComparisonService, PriceComparisonService>();

        return services;
    }
}
