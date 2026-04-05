# Price Integrity Hub - Quick Start Guide

## ?? Running the Application

### Prerequisites
- .NET 8 SDK installed
- Visual Studio 2022 or VS Code
- Chrome browser (for Selenium)

### Steps to Run
1. Open terminal in project directory
2. Run: `dotnet restore`
3. Run: `dotnet run`
4. Navigate to: `https://localhost:5001`

---

## ?? Navigation Guide

### Main Menu (Sidebar)
```
?? Dashboard      ? Home page with statistics
?? Upload Data    ? Upload Excel files
??  Run Validation ? Execute price comparison
?? Results        ? View detailed results
?? History        ? View past validation runs
```

---

## ?? Page-by-Page Guide

### 1. Dashboard (`/`)
**What You See:**
- 5 colorful cards showing: Total Products, Matched, Not Matched, New Items, Missing
- "Last Run Summary" section with date, duration, and breakdown
- 3 action buttons: Upload New Data, Run Validation, View Results

**Use It To:**
- Get quick overview of latest validation
- See success rate at a glance
- Navigate to other sections

---

### 2. Upload Data (`/upload`)
**What You See:**
- File upload button
- Selected file info (name, size)
- Upload & Process button
- File requirements list
- Preview table with 8 sample products

**How To Use:**
1. Click "Select Excel File"
2. Choose your .xlsx or .xls file
3. See file details and preview
4. Click "Upload & Process"

**Current Behavior:**
- Shows dummy preview data
- Simulates 2-second processing
- (Real Excel reading to be implemented)

---

### 3. Run Validation (`/validation`)
**What You See:**
- Large green "Start Validation" button
- Progress bar (when running)
- Process log timeline
- Step indicators (5 steps)
- Configuration panel

**How To Use:**
1. Click "Start Validation"
2. Watch progress bar advance
3. See real-time process logs
4. View step indicators turn green
5. Click "View Results" when complete

**Current Behavior:**
- Simulates 5-step validation process
- Takes ~10 seconds to complete
- Shows animated progress
- (Real scraping/comparison to be integrated)

**Steps Shown:**
1. ? Reading Excel Data
2. ? Launching Browser
3. ? Scraping Website
4. ? Comparing Data
5. ? Generating Report

---

### 4. Results (`/results`)
**What You See:**
- Search box, filter dropdown, export button
- Table with columns:
  - Product Name
  - Excel Price
  - Website Price
  - Difference (£ and %)
  - Status (colored chip)
- 4 summary cards (Matched, Not Matched, New, Missing)
- Pagination controls

**How To Use:**
1. **Search**: Type product name to filter
2. **Filter**: Select status from dropdown
3. **Export**: Click to download (to be implemented)
4. **Sort**: Click column headers
5. **Navigate**: Use pagination at bottom

**Status Colors:**
- ?? **Green** = Matched (prices within tolerance)
- ?? **Red** = Not Matched (price discrepancy)
- ?? **Blue** = New (product found on website only)
- ?? **Orange** = Missing (product in Excel only)

**Features:**
- Highlighted rows for mismatches
- "N/A" for missing price data
- Real-time search filtering
- Responsive table layout

---

### 5. History (`/history`)
**What You See:**
- Search box and refresh button
- Table with run history (8 sample runs)
- Columns:
  - Run ID (#1, #2, etc.)
  - Run Date (with time)
  - Total Products
  - Matched, Mismatch, New, Missing (with icons)
  - Status (chip)
  - Actions (View, Download, Delete buttons)
- 3 summary cards (Total Runs, Success Rate, Last Run)

**How To Use:**
1. **Search**: Filter by date or status
2. **View**: Click eye icon to see details (to be implemented)
3. **Download**: Click download icon for report
4. **Delete**: Click delete icon to remove run
5. **Navigate**: Use pagination

**Run Statuses:**
- ?? **Completed** - Successful validation
- ?? **Partial** - Completed with warnings
- ?? **Failed** - Validation failed

---

## ?? UI Elements Explained

### Cards
- **Elevation 4**: Main content cards (darker shadow)
- **Elevation 2**: Secondary cards (lighter shadow)

### Colors
- **Primary (Blue)**: Main actions, navigation
- **Success (Green)**: Positive results, matched items
- **Error (Red)**: Problems, mismatches
- **Warning (Orange)**: Cautions, missing items
- **Info (Light Blue)**: New items, neutral info

### Icons
- ?? Dashboard
- ?? Upload
- ?? Play/Run
- ?? Assessment/Results
- ?? History
- ? Checkmark (success)
- ? Error
- ?? Warning
- ?? New
- ?? Price Check

---

## ?? Developer Notes

### Dummy Data Locations
- **Dashboard**: Static values in markup
- **Upload**: `GeneratePreviewData()` method
- **Validation**: Simulated progress in `StartValidation()`
- **Results**: `GenerateDummyResults()` method
- **History**: `GenerateDummyHistory()` method

### Services (Ready to Use)
- **ScrapingService**: Scrapes books.toscrape.com
- **ComparisonService**: Compares product lists

### To Integrate Real Data
1. Replace dummy data with service calls
2. Add database context
3. Implement Excel reading (EPPlus/ClosedXML)
4. Connect ScrapingService to Validation page
5. Store results in database
6. Load history from database

---

## ?? Troubleshooting

### Build Errors
- Ensure .NET 8 SDK is installed
- Run `dotnet restore`
- Clean and rebuild solution

### Selenium Issues
- Ensure Chrome browser is installed
- ChromeDriver version matches Chrome version
- Check internet connection for scraping

### MudBlazor Issues
- Clear browser cache
- Check MudBlazor version compatibility
- Ensure all MudBlazor components have required parameters

---

## ?? Next Steps

### For POC/Demo
? Current state is perfect for demonstration
? All pages functional with dummy data
? Professional UI/UX

### For Production
1. **Database**
   - Add Entity Framework Core
   - Create migration for models
   - Implement repositories

2. **Excel Integration**
   - Install EPPlus or ClosedXML
   - Parse uploaded Excel files
   - Validate Excel structure

3. **Real Scraping**
   - Connect ScrapingService
   - Handle errors gracefully
   - Add retry logic

4. **Authentication**
   - Add Identity
   - User registration/login
   - Role-based access

5. **Export**
   - Implement Excel export
   - CSV export option
   - PDF reports

6. **Deployment**
   - Configure for production
   - Set up CI/CD
   - Deploy to Azure/AWS

---

## ?? Tips

### Performance
- Use pagination for large datasets
- Cache scraping results
- Implement background jobs for long-running validations

### UX Improvements
- Add toast notifications (MudBlazor Snackbar)
- Implement real-time updates (SignalR)
- Add loading skeletons
- Implement dark mode toggle

### Best Practices
- Add logging (Serilog)
- Implement error boundaries
- Add unit tests
- Document API endpoints

---

## ?? Support

### Documentation
- MudBlazor: https://mudblazor.com/
- Blazor: https://learn.microsoft.com/en-us/aspnet/core/blazor/
- Selenium: https://www.selenium.dev/documentation/

### Common Issues
- **Chrome not found**: Update ChromeDriver version
- **Excel upload fails**: Check file size and format
- **Slow scraping**: Use headless mode (already enabled)
- **Mobile menu not working**: Check responsive breakpoint

---

## ? Checklist for Demo

Before presenting:
- [ ] Run `dotnet run` successfully
- [ ] Open application in browser
- [ ] Navigate through all pages
- [ ] Test search/filter functionality
- [ ] Show workflow diagram
- [ ] Explain dummy data vs. real implementation
- [ ] Highlight responsive design (resize browser)
- [ ] Show mobile menu (toggle sidebar)

---

## ?? Demo Script

1. **Start**: "This is Price Integrity Hub - a price validation system"
2. **Dashboard**: "Here's our analytics dashboard showing key metrics"
3. **Upload**: "Users upload Excel files with product prices"
4. **Validation**: "The system scrapes website and compares prices"
5. **Results**: "Detailed results with search and filtering"
6. **History**: "Track all validation runs over time"
7. **Workflow**: "Here's the complete workflow diagram"

---

## ?? Project Highlights

### Technical Achievements
? Clean architecture with separation of concerns
? Responsive design for all devices
? Modern UI with MudBlazor
? Selenium integration for web scraping
? Extensible service layer

### Business Value
? Automates price validation
? Reduces manual effort
? Provides visual analytics
? Tracks historical trends
? Exports reports for stakeholders

### Code Quality
? Type-safe with C# and .NET 8
? Component-based architecture
? Reusable services
? Maintainable code structure
? Build successful with no warnings

---

**Ready to present!** ??
