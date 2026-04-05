using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PriceIntegrityHub.Core.Configuration;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Services;

/// <summary>
/// Service for comparing product prices between Excel and website data.
/// </summary>
public class PriceComparisonService : IPriceComparisonService
{
    private readonly ILogger<PriceComparisonService> _logger;
    private readonly ComparisonOptions _options;

    public PriceComparisonService(
        ILogger<PriceComparisonService> logger,
        IOptions<ComparisonOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new ComparisonOptions();
    }

    public List<ComparisonResult> CompareProducts(List<Product> excelProducts, List<Product> websiteProducts)
    {
        ArgumentNullException.ThrowIfNull(excelProducts);
        ArgumentNullException.ThrowIfNull(websiteProducts);

        var results = new List<ComparisonResult>();
        var comparisonType = _options.CaseInsensitiveMatching 
            ? StringComparison.OrdinalIgnoreCase 
            : StringComparison.Ordinal;

        _logger.LogInformation(
            "Comparing {ExcelCount} Excel products with {WebCount} website products (Tolerance: {Tolerance}%)",
            excelProducts.Count, websiteProducts.Count, _options.TolerancePercentage);

        // Check Excel products against Website products
        foreach (var excelProduct in excelProducts)
        {
            var websiteProduct = websiteProducts.FirstOrDefault(p =>
                p.Name.Equals(excelProduct.Name, comparisonType));

            if (websiteProduct != null)
            {
                var result = CreateComparisonResult(excelProduct, websiteProduct);
                results.Add(result);
            }
            else
            {
                // Product exists in Excel but not on website
                results.Add(CreateMissingResult(excelProduct));
            }
        }

        // Check for new products (exist on website but not in Excel)
        foreach (var websiteProduct in websiteProducts)
        {
            var excelProduct = excelProducts.FirstOrDefault(p =>
                p.Name.Equals(websiteProduct.Name, comparisonType));

            if (excelProduct == null)
            {
                results.Add(CreateNewProductResult(websiteProduct));
            }
        }

        LogComparisonSummary(results);

        return results;
    }

    public ComparisonSummary GetSummary(List<ComparisonResult> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var matched = results.Count(r => r.Status == ComparisonStatus.Matched);
        var notMatched = results.Count(r => r.Status == ComparisonStatus.NotMatched);
        var newItems = results.Count(r => r.Status == ComparisonStatus.New);
        var missing = results.Count(r => r.Status == ComparisonStatus.Missing);

        return new ComparisonSummary(matched, notMatched, newItems, missing);
    }

    private ComparisonResult CreateComparisonResult(Product excelProduct, Product websiteProduct)
    {
        var difference = Math.Abs(websiteProduct.Price - excelProduct.Price);
        var percentDifference = excelProduct.Price != 0
            ? (difference / excelProduct.Price) * 100
            : 0;

        var status = percentDifference <= _options.TolerancePercentage
            ? ComparisonStatus.Matched
            : ComparisonStatus.NotMatched;

        return new ComparisonResult
        {
            ProductName = excelProduct.Name,
            ExcelPrice = excelProduct.Price,
            WebsitePrice = websiteProduct.Price,
            Status = status,
            ComparedAt = DateTime.UtcNow
        };
    }

    private static ComparisonResult CreateMissingResult(Product excelProduct)
    {
        return new ComparisonResult
        {
            ProductName = excelProduct.Name,
            ExcelPrice = excelProduct.Price,
            WebsitePrice = 0,
            Status = ComparisonStatus.Missing,
            ComparedAt = DateTime.UtcNow
        };
    }

    private static ComparisonResult CreateNewProductResult(Product websiteProduct)
    {
        return new ComparisonResult
        {
            ProductName = websiteProduct.Name,
            ExcelPrice = 0,
            WebsitePrice = websiteProduct.Price,
            Status = ComparisonStatus.New,
            ComparedAt = DateTime.UtcNow
        };
    }

    private void LogComparisonSummary(List<ComparisonResult> results)
    {
        var summary = GetSummary(results);
        _logger.LogInformation(
            "Comparison completed. Total: {Total}, Matched: {Matched}, NotMatched: {NotMatched}, New: {New}, Missing: {Missing}",
            results.Count, summary.Matched, summary.NotMatched, summary.NewItems, summary.Missing);
    }
}
