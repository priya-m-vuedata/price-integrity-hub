using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Core.Interfaces;

/// <summary>
/// Interface for web scraping operations to retrieve product data.
/// </summary>
public interface IWebScraper
{
    /// <summary>
    /// Scrapes product data from the configured website.
    /// </summary>
    /// <returns>A list of products scraped from the website.</returns>
    Task<List<Product>> GetProductsAsync();
}
