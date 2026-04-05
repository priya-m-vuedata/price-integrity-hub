using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Interfaces;

/// <summary>
/// Represents metadata about an uploaded file.
/// </summary>
public record UploadedFileInfo(string FileName, long FileSizeKB, DateTime UploadedAt);

/// <summary>
/// Interface for storing and retrieving validation data.
/// </summary>
public interface IDataStore
{
    /// <summary>
    /// Gets the uploaded products from Excel.
    /// </summary>
    List<Product> GetUploadedProducts();

    /// <summary>
    /// Sets the uploaded products from Excel with file metadata.
    /// </summary>
    void SetUploadedProducts(List<Product> products, string fileName, long fileSizeKB);

    /// <summary>
    /// Gets the uploaded file metadata, or null if no file is loaded.
    /// </summary>
    UploadedFileInfo? GetUploadedFileInfo();

    /// <summary>
    /// Clears the uploaded products.
    /// </summary>
    void ClearUploadedProducts();

    /// <summary>
    /// Gets the latest comparison results.
    /// </summary>
    List<ComparisonResult> GetLatestResults();

    /// <summary>
    /// Saves comparison results.
    /// </summary>
    void SaveResults(List<ComparisonResult> results);

    /// <summary>
    /// Adds a validation run to history.
    /// </summary>
    void AddValidationRun(ValidationRun run);

    /// <summary>
    /// Gets all validation history.
    /// </summary>
    List<ValidationRun> GetValidationHistory();

    /// <summary>
    /// Gets the most recent validation run.
    /// </summary>
    ValidationRun? GetLatestRun();

    /// <summary>
    /// Gets statistics from the latest results.
    /// </summary>
    ValidationStatistics GetStatistics();
}
