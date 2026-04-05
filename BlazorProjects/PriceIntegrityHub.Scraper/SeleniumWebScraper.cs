using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Models;
using PriceIntegrityHub.Scraper.Configuration;

namespace PriceIntegrityHub.Scraper;

/// <summary>
/// Selenium-based web scraper for extracting product data from websites.
/// Configured via ScraperOptions in appsettings.json.
/// </summary>
public class SeleniumWebScraper : IWebScraper
{
    private readonly ILogger<SeleniumWebScraper> _logger;
    private readonly ScraperOptions _options;

    public SeleniumWebScraper(
        ILogger<SeleniumWebScraper> logger,
        IOptions<ScraperOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? new ScraperOptions();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var products = new List<Product>();

        _logger.LogInformation(
            "Starting web scrape. Target: {Url}, Headless: {Headless}",
            _options.TargetUrl, _options.Headless);

        try
        {
            using var driver = CreateWebDriver();

            NavigateToTarget(driver);
            await WaitForPageLoad();

            var productElements = FindProductElements(driver);
            products = ExtractProducts(productElements);

            _logger.LogInformation(
                "Scraping completed successfully. Extracted {Count} products",
                products.Count);
        }
        catch (WebDriverException ex)
        {
            _logger.LogError(ex, "WebDriver error while scraping {Url}", _options.TargetUrl);
            throw new InvalidOperationException($"Failed to scrape website: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while scraping products");
            throw;
        }

        return products;
    }

    private ChromeDriver CreateWebDriver()
    {
        var chromeOptions = new ChromeOptions();

        if (_options.Headless)
        {
            chromeOptions.AddArgument("--headless");
        }

        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--window-size=1920,1080");

        var driver = new ChromeDriver(chromeOptions);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_options.ImplicitWaitSeconds);

        return driver;
    }

    private void NavigateToTarget(IWebDriver driver)
    {
        _logger.LogDebug("Navigating to {Url}", _options.TargetUrl);
        driver.Navigate().GoToUrl(_options.TargetUrl);
    }

    private async Task WaitForPageLoad()
    {
        _logger.LogDebug("Waiting {Delay}ms for page to load", _options.PageLoadDelayMs);
        await Task.Delay(_options.PageLoadDelayMs);
    }

    private IReadOnlyCollection<IWebElement> FindProductElements(IWebDriver driver)
    {
        var elements = driver.FindElements(By.CssSelector(_options.ProductSelector));
        _logger.LogDebug("Found {Count} product elements using selector '{Selector}'",
            elements.Count, _options.ProductSelector);
        return elements;
    }

    private List<Product> ExtractProducts(IReadOnlyCollection<IWebElement> productElements)
    {
        var products = new List<Product>();
        var successCount = 0;
        var failCount = 0;

        foreach (var element in productElements)
        {
            try
            {
                var product = ExtractProductFromElement(element);
                if (product != null)
                {
                    products.Add(product);
                    successCount++;
                }
            }
            catch (NoSuchElementException ex)
            {
                failCount++;
                _logger.LogWarning("Required element not found in product: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                failCount++;
                _logger.LogWarning(ex, "Error extracting product data");
            }
        }

        if (failCount > 0)
        {
            _logger.LogWarning(
                "Product extraction completed with {FailCount} failures out of {Total} elements",
                failCount, productElements.Count);
        }

        return products;
    }

    private Product? ExtractProductFromElement(IWebElement productElement)
    {
        var nameElement = productElement.FindElement(By.CssSelector(_options.NameSelector));
        var priceElement = productElement.FindElement(By.CssSelector(_options.PriceSelector));

        var name = nameElement.GetDomProperty("title") 
                   ?? nameElement.GetDomAttribute("title") 
                   ?? nameElement.Text;

        var priceText = priceElement.Text
            .Replace(_options.CurrencySymbol, "")
            .Trim();

        if (string.IsNullOrEmpty(name))
        {
            _logger.LogDebug("Skipping product with empty name");
            return null;
        }

        if (!decimal.TryParse(priceText, out var price))
        {
            _logger.LogDebug("Could not parse price '{PriceText}' for product '{Name}'", priceText, name);
            return null;
        }

        return new Product
        {
            Name = name,
            Price = price,
            Status = "Active"
        };
    }
}
