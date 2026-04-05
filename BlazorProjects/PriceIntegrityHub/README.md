# Price Integrity Hub

A Blazor Server application built with .NET 8 and MudBlazor for managing price data validation workflows.

## Features

- **Modern UI**: Built with MudBlazor components for a clean, professional interface
- **Responsive Layout**: MudLayout with MudAppBar and MudDrawer for navigation
- **Interactive Pages**: All pages use InteractiveServer render mode for real-time updates

## Pages

### Dashboard (/)
- Overview of validation statistics
- Key metrics displayed in cards
- Recent activity section

### Upload Data (/upload)
- File upload interface for CSV and Excel files
- Support for multiple file uploads
- File validation and guidelines

### Run Validation (/validation)
- Dataset selection
- Configurable validation rules:
  - Price Range Validation
  - Duplicate Detection
  - Cross-Reference Consistency
  - Format Validation
- Real-time validation progress

### Results (/results)
- Summary statistics (Passed, Warnings, Errors)
- Detailed validation results table
- Export functionality

### History (/history)
- Searchable table of past validation runs
- Filtering capabilities
- View and download previous results

## Technologies Used

- **.NET 8**: Latest version of .NET
- **Blazor Server**: Interactive server-side rendering
- **MudBlazor 9.2.0**: Material Design component library
- **Razor Components**: Component-based architecture

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code

### Running the Application

```bash
cd PriceIntegrityHub
dotnet run
```

The application will be available at `https://localhost:5001` (or the port specified in launchSettings.json)

### Building the Application

```bash
dotnet build
```

## Project Structure

```
PriceIntegrityHub/
??? Components/
?   ??? Layout/
?   ?   ??? MainLayout.razor      # Main layout with MudBlazor components
?   ?   ??? NavMenu.razor          # Navigation menu with MudNavMenu
?   ??? Pages/
?   ?   ??? Home.razor             # Dashboard page
?   ?   ??? Upload.razor           # File upload page
?   ?   ??? Validation.razor       # Validation configuration page
?   ?   ??? Results.razor          # Results display page
?   ?   ??? History.razor          # Validation history page
?   ??? App.razor                  # Root component with MudBlazor CSS/JS
?   ??? _Imports.razor             # Global using directives
??? Program.cs                     # Application configuration
??? PriceIntegrityHub.csproj      # Project file
```

## Configuration

MudBlazor is configured in `Program.cs`:
```csharp
builder.Services.AddMudServices();
```

## License

This project is created for learning purposes.
