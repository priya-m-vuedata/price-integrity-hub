namespace PriceIntegrityHub.Scraper.Configuration;

/// <summary>
/// Configuration options for the web scraper.
/// These settings can be configured in appsettings.json under the "Scraper" section.
/// </summary>
public class ScraperOptions
{
    /// <summary>
    /// Configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "Scraper";

    /// <summary>
    /// The target URL to scrape products from.
    /// Default: "http://books.toscrape.com/"
    /// </summary>
    public string TargetUrl { get; set; } = "http://books.toscrape.com/";

    /// <summary>
    /// Time to wait for page to load in milliseconds.
    /// Default: 2000ms
    /// </summary>
    public int PageLoadDelayMs { get; set; } = 2000;

    /// <summary>
    /// Implicit wait timeout for element searches in seconds.
    /// Default: 10 seconds
    /// </summary>
    public int ImplicitWaitSeconds { get; set; } = 10;

    /// <summary>
    /// Whether to run browser in headless mode (no UI).
    /// Default: true
    /// </summary>
    public bool Headless { get; set; } = true;

    /// <summary>
    /// CSS selector for product container elements.
    /// Default: "article.product_pod"
    /// </summary>
    public string ProductSelector { get; set; } = "article.product_pod";

    /// <summary>
    /// CSS selector for product name within product container.
    /// Default: "h3 a"
    /// </summary>
    public string NameSelector { get; set; } = "h3 a";

    /// <summary>
    /// CSS selector for product price within product container.
    /// Default: "p.price_color"
    /// </summary>
    public string PriceSelector { get; set; } = "p.price_color";

    /// <summary>
    /// Currency symbol to remove from price text.
    /// Default: "£"
    /// </summary>
    public string CurrencySymbol { get; set; } = "£";
}
