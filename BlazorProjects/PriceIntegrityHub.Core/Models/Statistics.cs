namespace PriceIntegrityHub.Core.Models;

/// <summary>
/// Summary statistics from a price comparison operation.
/// </summary>
public record ComparisonSummary(
    int Matched,
    int NotMatched,
    int NewItems,
    int Missing
);

/// <summary>
/// Extended statistics including total count.
/// </summary>
public record ValidationStatistics(
    int Total,
    int Matched,
    int NotMatched,
    int NewItems,
    int Missing
);
