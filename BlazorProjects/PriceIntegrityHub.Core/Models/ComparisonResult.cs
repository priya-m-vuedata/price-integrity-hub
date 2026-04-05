namespace PriceIntegrityHub.Core.Models;

/// <summary>
/// Represents the result of comparing a product's price between Excel and website.
/// </summary>
public class ComparisonResult
{
    public string ProductName { get; set; } = string.Empty;
    public decimal ExcelPrice { get; set; }
    public decimal WebsitePrice { get; set; }
    public decimal Difference => WebsitePrice - ExcelPrice;
    public decimal DifferencePercent => ExcelPrice != 0 ? (Difference / ExcelPrice) * 100 : 0;
    public ComparisonStatus Status { get; set; }
    public DateTime ComparedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Status of a price comparison result.
/// </summary>
public enum ComparisonStatus
{
    Matched,
    NotMatched,
    New,
    Missing
}
