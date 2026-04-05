namespace PriceIntegrityHub.Core.Models;

/// <summary>
/// Represents a validation run with summary statistics.
/// </summary>
public class ValidationRun
{
    public int Id { get; set; }
    public DateTime RunDate { get; set; }
    public int TotalProducts { get; set; }
    public int Matched { get; set; }
    public int Mismatch { get; set; }
    public int NewProducts { get; set; }
    public int MissingProducts { get; set; }
    public string Status { get; set; } = "Completed";
}
