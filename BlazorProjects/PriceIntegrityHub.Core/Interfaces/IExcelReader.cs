using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Interfaces;

/// <summary>
/// Interface for reading product data from Excel files.
/// </summary>
public interface IExcelReader
{
    /// <summary>
    /// Reads product data from an Excel file stream.
    /// </summary>
    /// <param name="fileStream">The Excel file stream.</param>
    /// <returns>A list of products parsed from the Excel file.</returns>
    Task<List<Product>> ReadExcelFileAsync(Stream fileStream);
}
