# 📌 Price Integrity Hub

## 🔍 Overview
Price Integrity Hub is a **Blazor-based POC** designed to validate product pricing by comparing uploaded data with live website data using automation.

---

## 🎯 Objective
- Ensure pricing consistency across platforms  
- Identify mismatched or missing products  
- Automate price validation using web scraping  

---

## ⚙️ Workflow

```text
Start
  ↓
Upload Excel File
  ↓
Read Excel Data
  ↓
Start Validation
  ↓
Launch Selenium
  ↓
Scrape Website Data
  ↓
Compare Data
  ↓
Match?
 ├── Yes → Matched
 └── No → Check Exists?
            ├── No → New Product
            └── Yes → Missing / Mismatch
  ↓
Store Results
  ↓
Display Results
  ↓
End
```
## 🧠 Key Features
- Excel-based bulk input  
- Automated browser scraping using Selenium  
- Intelligent comparison logic  

### Categorization:
- ✅ Matched  
- ➕ New Product  
- ⚠️ Missing / Mismatch  

- Result storage and UI display

## 🛠 Tech Stack
- .NET Blazor  
- Selenium WebDriver  
- Excel processing (EPPlus / ClosedXML)  
- SQL (optional for storage)

## 📊 Output Categories

| Category           | Description                          |
|------------------|--------------------------------------|
| Matched          | Price matches with website           |
| New Product      | Product not found online             |
| Missing/Mismatch | Exists but price differs             |

## 🚧 Future Enhancements
- Multi-website comparison  
- Parallel scraping (performance improvement)  
- Retry & error handling  
- Dashboard analytics  
