using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Interfaces;

/// <summary>
/// Interface for comparing product prices between different sources.
/// </summary>
public interface IPriceComparisonService
{
    /// <summary>
    /// Compares products from Excel with products from the website.
    /// </summary>
    /// <param name="excelProducts">Products loaded from Excel.</param>
    /// <param name="websiteProducts">Products scraped from the website.</param>
    /// <returns>A list of comparison results.</returns>
    List<ComparisonResult> CompareProducts(List<Product> excelProducts, List<Product> websiteProducts);

    /// <summary>
    /// Gets a summary of comparison results.
    /// </summary>
    /// <param name="results">The comparison results.</param>
    /// <returns>Summary statistics.</returns>
    ComparisonSummary GetSummary(List<ComparisonResult> results);
}
