using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Interfaces;

/// <summary>
/// Interface for exporting comparison results to various formats.
/// </summary>
public interface IResultExporter
{
    /// <summary>
    /// Exports comparison results to Excel format.
    /// </summary>
    /// <param name="results">The comparison results to export.</param>
    /// <returns>The Excel file as a byte array.</returns>
    Task<byte[]> ExportToExcelAsync(List<ComparisonResult> results);
}
