using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Models;
using PriceIntegrityHub.Data.Configuration;

namespace PriceIntegrityHub.Data.Storage;

/// <summary>
/// Thread-safe in-memory data store for validation data.
/// Configured via DataStoreOptions in appsettings.json.
/// For production, this could be replaced with a database implementation.
/// </summary>
public class InMemoryDataStore : IDataStore
{
    private readonly ILogger<InMemoryDataStore> _logger;
    private readonly DataStoreOptions _options;

    // Thread-safe collections with dedicated locks
    private readonly object _historyLock = new();
    private readonly object _resultsLock = new();
    private readonly object _productsLock = new();

    private readonly List<ValidationRun> _validationHistory = new();
    private readonly List<ComparisonResult> _latestResults = new();
    private List<Product> _uploadedExcelProducts = new();
    private UploadedFileInfo? _uploadedFileInfo;
    private int _nextRunId = 1;

    public InMemoryDataStore(
        ILogger<InMemoryDataStore> logger,
        IOptions<DataStoreOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new DataStoreOptions();

        _logger.LogInformation(
            "InMemoryDataStore initialized. Limits: History={MaxHistory}, Results={MaxResults}, Products={MaxProducts}",
            _options.MaxHistoryCount, _options.MaxResultsCount, _options.MaxProductsCount);
    }

    #region Uploaded Products

    public List<Product> GetUploadedProducts()
    {
        lock (_productsLock)
        {
            return _uploadedExcelProducts.ToList();
        }
    }

    public UploadedFileInfo? GetUploadedFileInfo()
    {
        lock (_productsLock)
        {
            return _uploadedFileInfo;
        }
    }

    public void SetUploadedProducts(List<Product> products, string fileName, long fileSizeKB)
    {
        ArgumentNullException.ThrowIfNull(products);

        lock (_productsLock)
        {
            if (products.Count > _options.MaxProductsCount)
            {
                _logger.LogWarning(
                    "Product count ({Count}) exceeds limit ({Limit}). Truncating.",
                    products.Count, _options.MaxProductsCount);

                _uploadedExcelProducts = products.Take(_options.MaxProductsCount).ToList();
            }
            else
            {
                _uploadedExcelProducts = products.ToList();
            }

            _uploadedFileInfo = new UploadedFileInfo(fileName, fileSizeKB, DateTime.Now);
            _logger.LogDebug("Stored {Count} uploaded products from file {FileName}", _uploadedExcelProducts.Count, fileName);
        }
    }

    public void ClearUploadedProducts()
    {
        lock (_productsLock)
        {
            var count = _uploadedExcelProducts.Count;
            _uploadedExcelProducts.Clear();
            _uploadedFileInfo = null;
            _logger.LogDebug("Cleared {Count} uploaded products", count);
        }
    }

    #endregion

    #region Comparison Results

    public List<ComparisonResult> GetLatestResults()
    {
        lock (_resultsLock)
        {
            return _latestResults.ToList();
        }
    }

    public void SaveResults(List<ComparisonResult> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        lock (_resultsLock)
        {
            _latestResults.Clear();

            if (results.Count > _options.MaxResultsCount)
            {
                _logger.LogWarning(
                    "Results count ({Count}) exceeds limit ({Limit}). Truncating.",
                    results.Count, _options.MaxResultsCount);

                _latestResults.AddRange(results.Take(_options.MaxResultsCount));
            }
            else
            {
                _latestResults.AddRange(results);
            }

            _logger.LogDebug("Saved {Count} comparison results", _latestResults.Count);
        }
    }

    #endregion

    #region Validation History

    public void AddValidationRun(ValidationRun run)
    {
        ArgumentNullException.ThrowIfNull(run);

        lock (_historyLock)
        {
            run.Id = _nextRunId++;
            run.RunDate = _options.UseUtcTime ? DateTime.UtcNow : DateTime.Now;

            _validationHistory.Insert(0, run); // Add at beginning for newest first

            // Enforce history limit
            if (_validationHistory.Count > _options.MaxHistoryCount)
            {
                var removedCount = _validationHistory.Count - _options.MaxHistoryCount;
                _validationHistory.RemoveRange(_options.MaxHistoryCount, removedCount);

                _logger.LogDebug(
                    "History limit reached. Removed {Count} oldest entries.",
                    removedCount);
            }

            _logger.LogDebug(
                "Added validation run #{Id}. History count: {Count}",
                run.Id, _validationHistory.Count);
        }
    }

    public List<ValidationRun> GetValidationHistory()
    {
        lock (_historyLock)
        {
            return _validationHistory.ToList();
        }
    }

    public ValidationRun? GetLatestRun()
    {
        lock (_historyLock)
        {
            return _validationHistory.FirstOrDefault();
        }
    }

    #endregion

    #region Statistics

    public ValidationStatistics GetStatistics()
    {
        lock (_resultsLock)
        {
            if (_latestResults.Count == 0)
            {
                return new ValidationStatistics(0, 0, 0, 0, 0);
            }

            var matched = _latestResults.Count(r => r.Status == ComparisonStatus.Matched);
            var notMatched = _latestResults.Count(r => r.Status == ComparisonStatus.NotMatched);
            var newItems = _latestResults.Count(r => r.Status == ComparisonStatus.New);
            var missing = _latestResults.Count(r => r.Status == ComparisonStatus.Missing);
            var total = _latestResults.Count;

            return new ValidationStatistics(total, matched, notMatched, newItems, missing);
        }
    }

    #endregion

    #region Maintenance

    /// <summary>
    /// Clears all data from the store.
    /// </summary>
    public void ClearAll()
    {
        lock (_productsLock)
        lock (_resultsLock)
        lock (_historyLock)
        {
            _uploadedExcelProducts.Clear();
            _latestResults.Clear();
            _validationHistory.Clear();
            _nextRunId = 1;

            _logger.LogInformation("All data cleared from InMemoryDataStore");
        }
    }

    /// <summary>
    /// Gets the current memory usage statistics.
    /// </summary>
    public DataStoreStatistics GetDataStoreStatistics()
    {
        lock (_productsLock)
        lock (_resultsLock)
        lock (_historyLock)
        {
            return new DataStoreStatistics
            {
                ProductCount = _uploadedExcelProducts.Count,
                ResultCount = _latestResults.Count,
                HistoryCount = _validationHistory.Count,
                MaxProducts = _options.MaxProductsCount,
                MaxResults = _options.MaxResultsCount,
                MaxHistory = _options.MaxHistoryCount
            };
        }
    }

    #endregion
}

/// <summary>
/// Statistics about the data store's current state.
/// </summary>
public class DataStoreStatistics
{
    public int ProductCount { get; init; }
    public int ResultCount { get; init; }
    public int HistoryCount { get; init; }
    public int MaxProducts { get; init; }
    public int MaxResults { get; init; }
    public int MaxHistory { get; init; }

    public double ProductUtilization => MaxProducts > 0 ? (double)ProductCount / MaxProducts * 100 : 0;
    public double ResultUtilization => MaxResults > 0 ? (double)ResultCount / MaxResults * 100 : 0;
    public double HistoryUtilization => MaxHistory > 0 ? (double)HistoryCount / MaxHistory * 100 : 0;
}
