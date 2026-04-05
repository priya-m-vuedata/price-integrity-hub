using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Data.Excel;

/// <summary>
/// Represents the result of an Excel import operation.
/// </summary>
public class ExcelImportResult
{
    /// <summary>
    /// Successfully imported products.
    /// </summary>
    public List<Product> Products { get; init; } = new();

    /// <summary>
    /// Total number of rows processed.
    /// </summary>
    public int TotalRowsProcessed { get; init; }

    /// <summary>
    /// Number of rows successfully imported.
    /// </summary>
    public int SuccessCount => Products.Count;

    /// <summary>
    /// Number of rows skipped (empty or invalid).
    /// </summary>
    public int SkippedCount { get; init; }

    /// <summary>
    /// Number of rows with errors.
    /// </summary>
    public int ErrorCount { get; init; }

    /// <summary>
    /// Validation errors encountered during import.
    /// </summary>
    public List<ImportError> Errors { get; init; } = new();

    /// <summary>
    /// Whether the import completed successfully (may have warnings).
    /// </summary>
    public bool IsSuccess => ErrorCount == 0 || Products.Count > 0;

    /// <summary>
    /// Whether all rows were imported without any errors.
    /// </summary>
    public bool IsPerfect => ErrorCount == 0 && SkippedCount == 0;
}

/// <summary>
/// Represents an error encountered during Excel import.
/// </summary>
public class ImportError
{
    /// <summary>
    /// The row number where the error occurred (1-based).
    /// </summary>
    public int RowNumber { get; init; }

    /// <summary>
    /// The column name or index where the error occurred.
    /// </summary>
    public string Column { get; init; } = string.Empty;

    /// <summary>
    /// Description of the error.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The raw value that caused the error.
    /// </summary>
    public string? RawValue { get; init; }

    public override string ToString() => $"Row {RowNumber}, {Column}: {Message}";
}
