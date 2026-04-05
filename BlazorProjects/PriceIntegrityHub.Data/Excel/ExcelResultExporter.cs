using OfficeOpenXml;
using PriceIntegrityHub.Core.Interfaces;
using PriceIntegrityHub.Core.Models;

namespace PriceIntegrityHub.Data.Excel;

/// <summary>
/// Exports comparison results to Excel format.
/// </summary>
public class ExcelResultExporter : IResultExporter
{
    public ExcelResultExporter()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<byte[]> ExportToExcelAsync(List<ComparisonResult> results)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Comparison Results");

        // Headers
        worksheet.Cells[1, 1].Value = "Product Name";
        worksheet.Cells[1, 2].Value = "Excel Price";
        worksheet.Cells[1, 3].Value = "Website Price";
        worksheet.Cells[1, 4].Value = "Difference (£)";
        worksheet.Cells[1, 5].Value = "Difference (%)";
        worksheet.Cells[1, 6].Value = "Status";
        worksheet.Cells[1, 7].Value = "Compared At";

        // Style header
        using (var range = worksheet.Cells[1, 1, 1, 7])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
        }

        // Data
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var row = i + 2;

            worksheet.Cells[row, 1].Value = result.ProductName;
            worksheet.Cells[row, 2].Value = result.ExcelPrice;
            worksheet.Cells[row, 3].Value = result.WebsitePrice > 0 ? result.WebsitePrice : null;
            worksheet.Cells[row, 4].Value = result.Difference;
            worksheet.Cells[row, 5].Value = result.DifferencePercent;
            worksheet.Cells[row, 6].Value = result.Status.ToString();
            worksheet.Cells[row, 7].Value = result.ComparedAt.ToString("yyyy-MM-dd HH:mm:ss");

            // Color code rows based on status
            var rowRange = worksheet.Cells[row, 1, row, 7];
            switch (result.Status)
            {
                case ComparisonStatus.Matched:
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    break;
                case ComparisonStatus.NotMatched:
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                    break;
                case ComparisonStatus.New:
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    break;
                case ComparisonStatus.Missing:
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                    break;
            }
        }

        // Auto-fit columns
        worksheet.Cells.AutoFitColumns();

        return await package.GetAsByteArrayAsync();
    }
}
