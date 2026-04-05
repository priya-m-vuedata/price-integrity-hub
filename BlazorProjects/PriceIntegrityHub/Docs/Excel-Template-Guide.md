# Sample Excel File Template

Create an Excel file with the following format:

## Excel Structure

| Column A | Column B |
|----------|----------|
| Product Name | Price |

## Example Data

| Product Name | Price |
|-------------|-------|
| A Light in the Attic | 51.77 |
| Tipping the Velvet | 53.74 |
| Soumission | 50.10 |
| Sharp Objects | 47.82 |
| Sapiens: A Brief History of Humankind | 54.23 |
| The Requiem Red | 22.65 |
| The Dirty Little Secrets of Getting Your Dream Job | 33.34 |
| The Coming Woman: A Novel Based on the Life of the Infamous Feminist, Victoria Woodhull | 17.93 |
| The Boys in the Boat: Nine Americans and Their Epic Quest for Gold at the 1936 Berlin Olympics | 22.60 |
| The Black Maria | 52.15 |
| Starving Hearts (Triangular Trade Trilogy, #1) | 13.99 |
| Shakespeare's Sonnets | 20.66 |
| Set Me Free | 17.46 |
| Scott Pilgrim's Precious Little Life (Scott Pilgrim #1) | 52.29 |
| Rip it Up and Start Again | 35.02 |

## Important Notes

1. **First row** can be headers (will be skipped during import)
2. **Column A** must contain product names (text)
3. **Column B** must contain prices (numbers only, no currency symbols)
4. **File format**: .xlsx or .xls
5. **Maximum size**: 10 MB

## To Create:

1. Open Microsoft Excel or Google Sheets
2. Enter "Product Name" in cell A1
3. Enter "Price" in cell B1
4. Fill in your product data starting from row 2
5. Save as .xlsx file
6. Upload to the application

## What Happens:

1. Application reads Column A and B
2. First row (headers) is skipped automatically
3. Empty rows are ignored
4. Products are validated against books.toscrape.com
5. Results show matches, mismatches, new items, and missing items

## Matching Logic:

- Product names are matched **case-insensitive**
- Prices are compared with **1% tolerance**
- Website products not in Excel = **New Items**
- Excel products not on website = **Missing Items**

---

**Tip:** Use actual product names from books.toscrape.com to test matching functionality!
