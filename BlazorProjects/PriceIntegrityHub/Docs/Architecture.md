# PriceIntegrityHub - Architecture Documentation

## Overview

PriceIntegrityHub is a Blazor Server application for validating product prices between Excel files and web sources. The application follows **Clean Architecture** principles with clear separation of concerns.

## Solution Structure

```
PriceIntegrityHub.sln
?
??? PriceIntegrityHub/                 # UI Layer (Blazor Server)
?   ??? Components/Pages/              # Razor components
?   ??? wwwroot/                        # Static assets
?   ??? Program.cs                      # Application entry point
?   ??? appsettings.json               # Centralized configuration
?
??? PriceIntegrityHub.Core/            # Business Logic Layer
?   ??? Configuration/                  # Options classes
?   ??? Interfaces/                     # Service contracts
?   ??? Models/                         # Domain entities
?   ??? Services/                       # Business logic
?
??? PriceIntegrityHub.Data/            # Data Access Layer
?   ??? Configuration/                  # Options classes
?   ??? Excel/                          # Excel read/write
?   ??? Storage/                        # Data persistence
?
??? PriceIntegrityHub.Scraper/         # External Services Layer
    ??? Configuration/                  # Options classes
    ??? SeleniumWebScraper.cs          # Web scraping
```

## Architecture Diagram

```
???????????????????????????????????????????????????????????????????????
?                         UI LAYER                                     ?
?                    PriceIntegrityHub                                ?
?  ????????????? ????????????? ????????????? ?????????????           ?
?  ?   Home    ? ?  Upload   ? ?Validation ? ?  Results  ?           ?
?  ?   .razor  ? ?   .razor  ? ?   .razor  ? ?   .razor  ?           ?
?  ????????????? ????????????? ????????????? ?????????????           ?
?                              ?                                       ?
?                    Dependency Injection                              ?
???????????????????????????????????????????????????????????????????????
                               ?
         ?????????????????????????????????????????????
         ?                     ?                     ?
         ?                     ?                     ?
???????????????????   ???????????????????   ???????????????????
?   CORE LAYER    ?   ?   DATA LAYER    ?   ?  SCRAPER LAYER  ?
?                 ?   ?                 ?   ?                 ?
?  Interfaces:    ?   ?  Implements:    ?   ?  Implements:    ?
?  • IDataStore   ?????  • IDataStore   ?   ?  • IWebScraper  ?
?  • IExcelReader ?????  • IExcelReader ?   ?                 ?
?  • IWebScraper  ?????  • IResultExp.  ?   ?  Uses:          ?
?  • IResultExp.  ?   ?                 ?   ?  • Selenium     ?
?  • IPriceComp.  ?   ?  Uses:          ?   ?  • ChromeDriver ?
?                 ?   ?  • EPPlus       ?   ?                 ?
?  Services:      ?   ?                 ?   ?                 ?
?  • PriceComp.   ?   ?                 ?   ?                 ?
?                 ?   ?                 ?   ?                 ?
?  Models:        ?   ?                 ?   ?                 ?
?  • Product      ?   ?                 ?   ?                 ?
?  • Comparison   ?   ?                 ?   ?                 ?
?  • ValidationRun?   ?                 ?   ?                 ?
???????????????????   ???????????????????   ???????????????????
```

## Layer Responsibilities

### UI Layer (`PriceIntegrityHub`)
- **Purpose**: User interface and application hosting
- **Technology**: Blazor Server with MudBlazor components
- **Responsibilities**:
  - Render pages and handle user interactions
  - Configure dependency injection
  - Host application configuration

### Core Layer (`PriceIntegrityHub.Core`)
- **Purpose**: Business logic and domain contracts
- **Dependencies**: None (innermost layer)
- **Responsibilities**:
  - Define interfaces (contracts) for all services
  - Contain domain models (Product, ComparisonResult, etc.)
  - Implement business logic (PriceComparisonService)
  - Define configuration options

### Data Layer (`PriceIntegrityHub.Data`)
- **Purpose**: Data persistence and file operations
- **Dependencies**: Core
- **Responsibilities**:
  - Read/write Excel files (EPPlus)
  - Store validation data (InMemoryDataStore)
  - Export comparison results

### Scraper Layer (`PriceIntegrityHub.Scraper`)
- **Purpose**: External web scraping
- **Dependencies**: Core
- **Responsibilities**:
  - Navigate and scrape websites (Selenium)
  - Extract product data from web pages

## Application Flow

```
1. UPLOAD          2. VALIDATE           3. COMPARE           4. DISPLAY
???????????       ???????????????       ???????????????       ???????????
?  Excel  ?????????   Scrape    ?????????   Compare   ????????? Results ?
?  File   ?       ?   Website   ?       ?   Prices    ?       ?  Page   ?
???????????       ???????????????       ???????????????       ???????????
     ?                   ?                     ?                   ?
     ?                   ?                     ?                   ?
IExcelReader      IWebScraper         IPriceComparison        IDataStore
     ?                   ?                     ?                   ?
     ?                   ?                     ?                   ?
ExcelReader      SeleniumWebScraper   PriceComparisonSvc    InMemoryStore
```

## Configuration

All services are configured via `appsettings.json`:

```json
{
  "Comparison": {
    "TolerancePercentage": 1.0,
    "CaseInsensitiveMatching": true
  },
  "Scraper": {
    "TargetUrl": "http://books.toscrape.com/",
    "PageLoadDelayMs": 2000,
    "Headless": true
  },
  "Excel": {
    "DataStartRow": 2,
    "Columns": { "ProductName": 1, "Price": 2 }
  },
  "DataStore": {
    "MaxHistoryCount": 100,
    "MaxResultsCount": 10000
  }
}
```

## Design Principles Applied

| Principle | Implementation |
|-----------|---------------|
| **Separation of Concerns** | Each layer has a single responsibility |
| **Dependency Inversion** | High-level modules depend on abstractions (interfaces in Core) |
| **Open/Closed** | Behavior changes via configuration, not code |
| **Interface Segregation** | Focused interfaces (IExcelReader, IWebScraper, etc.) |
| **Single Responsibility** | Each service does one thing well |

## Key Design Decisions

### 1. Interface-Based Design
All services implement interfaces defined in the Core layer:
- Enables easy testing with mocks
- Allows swapping implementations (e.g., database instead of in-memory)
- Decouples layers from concrete implementations

### 2. Options Pattern
Each service uses the Options pattern for configuration:
- `ComparisonOptions` - Price tolerance settings
- `ScraperOptions` - URL, selectors, timeouts
- `ExcelOptions` - Column mappings, validation rules
- `DataStoreOptions` - Capacity limits, threading

### 3. Thread Safety
`InMemoryDataStore` uses dedicated locks for thread safety:
- Required for Blazor Server (multiple concurrent users)
- Prevents race conditions on shared state

### 4. Centralized Configuration
All settings in `appsettings.json`:
- Environment-specific overrides supported
- No hardcoded values in services
- Easy to modify without recompilation

## Future Enhancements

### Production Ready
- [ ] Replace `InMemoryDataStore` with SQL Server/PostgreSQL
- [ ] Add authentication/authorization
- [ ] Implement caching with Redis
- [ ] Add health checks and monitoring

### Testing
- [ ] Unit tests for Core services
- [ ] Integration tests for Data layer
- [ ] E2E tests for Scraper layer

### Features
- [ ] Multiple website support
- [ ] Scheduled validation runs
- [ ] Email notifications for mismatches
- [ ] Dashboard analytics

## Dependency Graph

```
PriceIntegrityHub (UI)
    ?
    ???? PriceIntegrityHub.Core
    ?
    ???? PriceIntegrityHub.Data
    ?         ?
    ?         ???? PriceIntegrityHub.Core
    ?
    ???? PriceIntegrityHub.Scraper
              ?
              ???? PriceIntegrityHub.Core
```

**Note**: Core has no dependencies on other projects. Data and Scraper depend only on Core. UI depends on all layers.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Chrome browser (for Selenium)

### Running the Application
```bash
cd PriceIntegrityHub
dotnet run
```

### Running Tests
```bash
dotnet test
```

## Technology Stack

| Component | Technology |
|-----------|------------|
| UI Framework | Blazor Server |
| UI Components | MudBlazor |
| Excel Processing | EPPlus |
| Web Scraping | Selenium WebDriver |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Configuration | Microsoft.Extensions.Options |
| Logging | Microsoft.Extensions.Logging |
