namespace PriceIntegrityHub.Core.Models;

/// <summary>
/// Represents a product with basic price information.
/// </summary>
public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
}
