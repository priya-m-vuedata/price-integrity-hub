using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Data.Configuration;
using PriceIntegrityHub.Data.Excel;
using PriceIntegrityHub.Data.Storage;

namespace PriceIntegrityHub.Data;

/// <summary>
/// Extension methods for registering Data layer services with dependency injection.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Data layer services to the service collection.
    /// </summary>
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration options
        services.Configure<ExcelOptions>(
            configuration.GetSection(ExcelOptions.SectionName));

        services.Configure<DataStoreOptions>(
            configuration.GetSection(DataStoreOptions.SectionName));

        // Register services
        services.AddScoped<IExcelReader, ExcelReader>();
        services.AddScoped<IResultExporter, ExcelResultExporter>();
        services.AddSingleton<IDataStore, InMemoryDataStore>(); // Singleton to maintain state

        return services;
    }
}
