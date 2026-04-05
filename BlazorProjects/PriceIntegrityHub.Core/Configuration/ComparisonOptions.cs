namespace PriceIntegrityHub.Core.Configuration;

/// <summary>
/// Configuration options for price comparison operations.
/// </summary>
public class ComparisonOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "Comparison";

    /// <summary>
    /// Tolerance percentage for price matching (default: 1.0%)
    /// Prices within this percentage difference are considered "Matched"
    /// </summary>
    public decimal TolerancePercentage { get; set; } = 1.0m;

    /// <summary>
    /// Whether to use case-insensitive product name matching (default: true)
    /// </summary>
    public bool CaseInsensitiveMatching { get; set; } = true;
}
