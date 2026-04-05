using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Models;
using PriceIntegrityHub.Data.Configuration;

namespace PriceIntegrityHub.Data.Excel;

/// <summary>
/// Reads product data from Excel files using EPPlus.
/// Configured via ExcelOptions in appsettings.json.
/// </summary>
public class ExcelReader : IExcelReader
{
    private readonly ILogger<ExcelReader> _logger;
    private readonly ExcelOptions _options;

    public ExcelReader(
        ILogger<ExcelReader> logger,
        IOptions<ExcelOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new ExcelOptions();

        // Set EPPlus license context (required for EPPlus 5+)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<List<Product>> ReadExcelFileAsync(Stream fileStream)
    {
        ArgumentNullException.ThrowIfNull(fileStream);

        var result = await ReadExcelWithValidationAsync(fileStream);

        if (result.Errors.Any())
        {
            _logger.LogWarning(
                "Excel import completed with {ErrorCount} errors. First error: {FirstError}",
                result.ErrorCount, result.Errors.FirstOrDefault());
        }

        return result.Products;
    }

    /// <summary>
    /// Reads Excel file with detailed validation results.
    /// </summary>
    public async Task<ExcelImportResult> ReadExcelWithValidationAsync(Stream fileStream)
    {
        ArgumentNullException.ThrowIfNull(fileStream);

        var products = new List<Product>();
        var errors = new List<ImportError>();
        var skippedCount = 0;
        var totalRows = 0;

        try
        {
            using var package = new ExcelPackage(fileStream);

            var worksheet = GetWorksheet(package);
            ValidateWorksheet(worksheet);

            var rowCount = CalculateRowCount(worksheet);
            totalRows = rowCount - _options.DataStartRow + 1;

            _logger.LogInformation(
                "Reading Excel file: Worksheet={Index}, Rows={RowCount}, Columns=[Name={NameCol}, Price={PriceCol}]",
                _options.WorksheetIndex, totalRows, _options.Columns.ProductName, _options.Columns.Price);

            for (int row = _options.DataStartRow; row <= rowCount; row++)
            {
                var rowResult = ProcessRow(worksheet, row, errors);

                if (rowResult.Product != null)
                {
                    products.Add(rowResult.Product);
                }
                else if (rowResult.WasSkipped)
                {
                    skippedCount++;
                }
            }

            LogImportSummary(products.Count, skippedCount, errors.Count);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, "Invalid Excel file format");
            errors.Add(new ImportError
            {
                RowNumber = 0,
                Column = "File",
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading Excel file");
            throw;
        }

        return await Task.FromResult(new ExcelImportResult
        {
            Products = products,
            TotalRowsProcessed = totalRows,
            SkippedCount = skippedCount,
            ErrorCount = errors.Count,
            Errors = errors
        });
    }

    private ExcelWorksheet GetWorksheet(ExcelPackage package)
    {
        if (package.Workbook.Worksheets.Count == 0)
        {
            throw new InvalidDataException("Excel file contains no worksheets");
        }

        if (_options.WorksheetIndex >= package.Workbook.Worksheets.Count)
        {
            throw new InvalidDataException(
                $"Worksheet index {_options.WorksheetIndex} is out of range. File has {package.Workbook.Worksheets.Count} worksheet(s).");
        }

        return package.Workbook.Worksheets[_options.WorksheetIndex];
    }

    private void ValidateWorksheet(ExcelWorksheet worksheet)
    {
        if (worksheet.Dimension == null)
        {
            throw new InvalidDataException("Worksheet is empty");
        }

        var columnCount = worksheet.Dimension.Columns;

        if (_options.Columns.ProductName > columnCount)
        {
            throw new InvalidDataException(
                $"Product Name column ({_options.Columns.ProductName}) exceeds worksheet columns ({columnCount})");
        }

        if (_options.Columns.Price > columnCount)
        {
            throw new InvalidDataException(
                $"Price column ({_options.Columns.Price}) exceeds worksheet columns ({columnCount})");
        }
    }

    private int CalculateRowCount(ExcelWorksheet worksheet)
    {
        var actualRows = worksheet.Dimension?.Rows ?? 0;

        if (_options.MaxRows > 0)
        {
            var maxDataRow = _options.DataStartRow + _options.MaxRows - 1;
            return Math.Min(actualRows, maxDataRow);
        }

        return actualRows;
    }

    private (Product? Product, bool WasSkipped) ProcessRow(
        ExcelWorksheet worksheet,
        int row,
        List<ImportError> errors)
    {
        // Extract product name
        var nameValue = worksheet.Cells[row, _options.Columns.ProductName].Value;
        var productName = ExtractStringValue(nameValue);

        // Skip empty rows if configured
        if (string.IsNullOrEmpty(productName))
        {
            if (_options.SkipEmptyRows)
            {
                _logger.LogDebug("Row {Row}: Skipped (empty product name)", row);
                return (null, WasSkipped: true);
            }

            errors.Add(new ImportError
            {
                RowNumber = row,
                Column = "ProductName",
                Message = "Product name is required",
                RawValue = nameValue?.ToString()
            });
            return (null, WasSkipped: false);
        }

        // Extract price
        var priceValue = worksheet.Cells[row, _options.Columns.Price].Value;
        var priceResult = ExtractDecimalValue(priceValue);

        if (!priceResult.Success)
        {
            errors.Add(new ImportError
            {
                RowNumber = row,
                Column = "Price",
                Message = $"Invalid price value: '{priceValue}'",
                RawValue = priceValue?.ToString()
            });
            // Continue with price = 0 instead of skipping
        }

        // Extract optional status
        var status = "Loaded";
        if (_options.Columns.Status > 0)
        {
            var statusValue = worksheet.Cells[row, _options.Columns.Status].Value;
            status = ExtractStringValue(statusValue) ?? "Loaded";
        }

        var product = new Product
        {
            Name = productName,
            Price = priceResult.Value,
            Status = status
        };

        _logger.LogDebug("Row {Row}: Loaded '{Name}' - £{Price}", row, productName, priceResult.Value);

        return (product, WasSkipped: false);
    }

    private string? ExtractStringValue(object? value)
    {
        if (value == null)
            return null;

        var stringValue = value.ToString();

        if (_options.TrimValues && stringValue != null)
        {
            stringValue = stringValue.Trim();
        }

        return string.IsNullOrEmpty(stringValue) ? null : stringValue;
    }

    private (bool Success, decimal Value) ExtractDecimalValue(object? value)
    {
        if (value == null)
            return (true, 0m);

        // Handle numeric types directly
        if (value is double doubleValue)
            return (true, (decimal)doubleValue);

        if (value is decimal decimalValue)
            return (true, decimalValue);

        if (value is int intValue)
            return (true, intValue);

        // Try parsing string representation
        var stringValue = value.ToString()?.Trim();

        if (string.IsNullOrEmpty(stringValue))
            return (true, 0m);

        // Remove common currency symbols
        stringValue = stringValue
            .Replace("£", "")
            .Replace("$", "")
            .Replace("€", "")
            .Replace(",", "")
            .Trim();

        if (decimal.TryParse(stringValue, out var result))
            return (true, result);

        return (false, 0m);
    }

    private void LogImportSummary(int successCount, int skippedCount, int errorCount)
    {
        if (errorCount > 0)
        {
            _logger.LogWarning(
                "Excel import completed: {Success} products loaded, {Skipped} rows skipped, {Errors} errors",
                successCount, skippedCount, errorCount);
        }
        else
        {
            _logger.LogInformation(
                "Excel import completed successfully: {Success} products loaded, {Skipped} rows skipped",
                successCount, skippedCount);
        }
    }
}
