namespace PriceIntegrityHub.Data.Configuration;

/// <summary>
/// Configuration options for the in-memory data store.
/// These settings can be configured in appsettings.json under the "DataStore" section.
/// </summary>
public class DataStoreOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "DataStore";

    /// <summary>
    /// Maximum number of validation runs to keep in history.
    /// Older runs are automatically removed when limit is exceeded.
    /// Default: 100
    /// </summary>
    public int MaxHistoryCount { get; set; } = 100;

    /// <summary>
    /// Maximum number of comparison results to store.
    /// Default: 10000
    /// </summary>
    public int MaxResultsCount { get; set; } = 10000;

    /// <summary>
    /// Maximum number of uploaded products to store.
    /// Default: 10000
    /// </summary>
    public int MaxProductsCount { get; set; } = 10000;

    /// <summary>
    /// Whether to use UTC time for timestamps.
    /// Default: true
    /// </summary>
    public bool UseUtcTime { get; set; } = true;
}
