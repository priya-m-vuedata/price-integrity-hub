namespace PriceIntegrityHub.Data.Configuration;

/// <summary>
/// Configuration options for Excel file operations.
/// These settings can be configured in appsettings.json under the "Excel" section.
/// </summary>
public class ExcelOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "Excel";

    /// <summary>
    /// The worksheet index to read from (0-based).
    /// Default: 0 (first worksheet)
    /// </summary>
    public int WorksheetIndex { get; set; } = 0;

    /// <summary>
    /// The row number where headers are located (1-based).
    /// Default: 1
    /// </summary>
    public int HeaderRow { get; set; } = 1;

    /// <summary>
    /// The row number where data starts (1-based).
    /// Default: 2 (assumes row 1 is headers)
    /// </summary>
    public int DataStartRow { get; set; } = 2;

    /// <summary>
    /// Column mapping configuration for product data.
    /// </summary>
    public ColumnMapping Columns { get; set; } = new();

    /// <summary>
    /// Whether to skip rows with empty product names.
    /// Default: true
    /// </summary>
    public bool SkipEmptyRows { get; set; } = true;

    /// <summary>
    /// Whether to trim whitespace from string values.
    /// Default: true
    /// </summary>
    public bool TrimValues { get; set; } = true;

    /// <summary>
    /// Maximum number of rows to read (0 = unlimited).
    /// Default: 0
    /// </summary>
    public int MaxRows { get; set; } = 0;
}

/// <summary>
/// Column mapping configuration for Excel import.
/// </summary>
public class ColumnMapping
{
    /// <summary>
    /// Column index for Product Name (1-based, where A=1, B=2, etc.).
    /// Default: 1 (Column A)
    /// </summary>
    public int ProductName { get; set; } = 1;

    /// <summary>
    /// Column index for Price (1-based).
    /// Default: 2 (Column B)
    /// </summary>
    public int Price { get; set; } = 2;

    /// <summary>
    /// Optional column index for Status (1-based). Set to 0 to ignore.
    /// Default: 0 (not used)
    /// </summary>
    public int Status { get; set; } = 0;
}
