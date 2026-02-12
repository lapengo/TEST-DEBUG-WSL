# Architecture Refactoring - Service Layers

## 🎯 Overview

Project telah di-refactor dari monolithic Program.cs menjadi clean architecture dengan separation of concerns yang jelas.

---

## 📁 New Project Structure

```
PME/
├── Helpers/                    # Helper layer - reusable utility methods
│   ├── ConsoleHelper.cs        # Console formatting & output
│   └── DisplayHelper.cs        # Display logic for different data types
│
├── Models/                     # DTOs - Data Transfer Objects
│   ├── PmeSettings.cs          # Configuration model
│   ├── WebServiceInfoRequestDto.cs
│   ├── WebServiceInfoResponseDto.cs
│   ├── AlarmEventTypesRequestDto.cs
│   └── AlarmEventTypesResponseDto.cs
│
├── Services/                   # Service layer - business logic
│   ├── DataExchangeService.cs  # SOAP client wrapper
│   ├── WebServiceInfoService.cs    # GetWebServiceInformation logic
│   └── AlarmEventTypesService.cs   # GetAlarmEventTypes logic
│
└── Program.cs                  # Entry point - clean & simple
```

---

## 🏗️ Architecture Layers

### 1. Helper Layer
**Purpose:** Reusable utility methods yang bisa dipakai di mana saja

#### ConsoleHelper.cs
```csharp
// Methods:
- PrintSeparator(int length = 80, char character = '=')
- PrintHeader(string title)
- PrintSectionHeader(string title)
- PrintKeyValue(string key, string? value, int indent = 2)
- PrintListItem(string item, int indent = 2)
- PrintSuccess(string message)
- PrintError(string message)
```

**Usage Example:**
```csharp
ConsoleHelper.PrintHeader("My Title");
ConsoleHelper.PrintKeyValue("Username", "admin");
ConsoleHelper.PrintListItem("Item 1");
ConsoleHelper.PrintSuccess("Operation completed!");
```

#### DisplayHelper.cs
```csharp
// Methods:
- DisplayWebServiceInfo(WebServiceInfoResponseDto response)
- DisplayAlarmEventTypes(AlarmEventTypesResponseDto response)
```

**Usage Example:**
```csharp
var response = await service.GetWebServiceInformationAsync();
DisplayHelper.DisplayWebServiceInfo(response);
```

---

### 2. Service Layer
**Purpose:** Business logic untuk setiap SOAP operation

#### WebServiceInfoService.cs
**Responsibility:** Handle GetWebServiceInformation operation

```csharp
public class WebServiceInfoService
{
    public async Task ExecuteAsync(string version)
    {
        // 1. Create request
        // 2. Call SOAP service
        // 3. Display results
    }
}
```

**Usage:**
```csharp
var service = new WebServiceInfoService(dataExchangeService);
await service.ExecuteAsync("2");
```

#### AlarmEventTypesService.cs
**Responsibility:** Handle GetAlarmEventTypes operation

```csharp
public class AlarmEventTypesService
{
    public async Task ExecuteAsync(string? version = null)
    {
        // 1. Create request
        // 2. Call SOAP service
        // 3. Map response to DTO
        // 4. Display results
    }
}
```

**Usage:**
```csharp
var service = new AlarmEventTypesService(dataExchangeService);
await service.ExecuteAsync("2");
```

#### DataExchangeService.cs (Updated)
**Responsibility:** SOAP client wrapper dengan authentication

**New Method:**
```csharp
public DataExchangeClient GetClient()
{
    return _client;
}
```

Allows service classes to access SOAP client directly for operations not yet wrapped.

---

### 3. Model Layer (DTOs)
**Purpose:** Data Transfer Objects untuk request & response

#### AlarmEventTypesRequestDto.cs
```csharp
public class AlarmEventTypesRequestDto
{
    public string? Version { get; set; }
}
```

#### AlarmEventTypesResponseDto.cs
```csharp
public class AlarmEventTypesResponseDto
{
    public List<string> Types { get; set; }
    public string? ResponseVersion { get; set; }
}
```

---

## 🚀 Program.cs - The Entry Point

### New Features:

**1. Interactive Menu**
```
Pilih operasi yang ingin dijalankan:
1. GetWebServiceInformation
2. GetAlarmEventTypes
3. Jalankan semua

Pilihan (1/2/3):
```

**2. Clean Code Structure**
```csharp
// Configuration
var settings = LoadConfiguration();

// Create SOAP client
using var dataExchangeService = new DataExchangeService(...);

// Menu selection
switch (choice)
{
    case "1":
        await RunGetWebServiceInfo(dataExchangeService, settings.Version);
        break;
    case "2":
        await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
        break;
    case "3":
        // Run both
        break;
}
```

**3. Helper Methods**
```csharp
static async Task RunGetWebServiceInfo(...)
{
    var service = new WebServiceInfoService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetAlarmEventTypes(...)
{
    var service = new AlarmEventTypesService(dataExchangeService);
    await service.ExecuteAsync(version);
}
```

---

## 📊 Before vs After

### Before (Monolithic):
```csharp
Program.cs (180 lines)
├─ Configuration loading
├─ SOAP client creation
├─ Service call
├─ Response mapping (inline)
├─ Display logic (inline)
│  ├─ Print separators
│  ├─ Print version info
│  ├─ Print operations
│  ├─ Print profiles
│  └─ Print system info
└─ Error handling
```

**Issues:**
- ❌ All logic in one file
- ❌ Hard to reuse code
- ❌ Hard to add new operations
- ❌ Mixed concerns (display + business logic)

---

### After (Layered):
```csharp
Program.cs (145 lines)
├─ Configuration loading
├─ Menu selection
└─ Delegate to services

Helpers/
├─ ConsoleHelper (formatting)
└─ DisplayHelper (display)

Services/
├─ WebServiceInfoService
└─ AlarmEventTypesService

Models/
├─ Request DTOs
└─ Response DTOs
```

**Benefits:**
- ✅ Separation of concerns
- ✅ Reusable components
- ✅ Easy to add new operations
- ✅ Clean, maintainable code
- ✅ Single responsibility principle

---

## 🔧 How to Add New SOAP Operation

### Step 1: Create DTOs
```csharp
// Models/MyOperationRequestDto.cs
public class MyOperationRequestDto
{
    public string? Version { get; set; }
    // Add other properties
}

// Models/MyOperationResponseDto.cs
public class MyOperationResponseDto
{
    public string? ResponseVersion { get; set; }
    // Add other properties
}
```

### Step 2: Add Display Method (if needed)
```csharp
// Helpers/DisplayHelper.cs
public static void DisplayMyOperation(MyOperationResponseDto response)
{
    ConsoleHelper.PrintSeparator();
    Console.WriteLine("HASIL RESPONSE:");
    ConsoleHelper.PrintSeparator();
    // Display logic here
}
```

### Step 3: Create Service Class
```csharp
// Services/MyOperationService.cs
public class MyOperationService
{
    private readonly DataExchangeService _dataExchangeService;

    public MyOperationService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string? version = null)
    {
        var request = new MyOperationRequestDto { Version = version };
        var response = await GetMyOperationAsync(request);
        DisplayHelper.DisplayMyOperation(response);
        ConsoleHelper.PrintSuccess("Success!");
    }

    private async Task<MyOperationResponseDto> GetMyOperationAsync(...)
    {
        // Call SOAP service via _dataExchangeService.GetClient()
        // Map response to DTO
        // Return DTO
    }
}
```

### Step 4: Add to Program.cs
```csharp
// Add menu option
Console.WriteLine("4. MyOperation");

// Add case
case "4":
    await RunMyOperation(dataExchangeService, settings.Version);
    break;

// Add helper method
static async Task RunMyOperation(...)
{
    var service = new MyOperationService(dataExchangeService);
    await service.ExecuteAsync(version);
}
```

**That's it!** New operation added with clean separation of concerns.

---

## 🎯 Design Principles Applied

1. **Single Responsibility Principle**
   - Each class has one job
   - ConsoleHelper = formatting
   - DisplayHelper = displaying data
   - Service classes = business logic for one operation

2. **Don't Repeat Yourself (DRY)**
   - Reusable helper methods
   - Common display patterns in DisplayHelper
   - Common formatting in ConsoleHelper

3. **Separation of Concerns**
   - Helpers = utilities
   - Services = business logic
   - Models = data structures
   - Program.cs = orchestration

4. **Open/Closed Principle**
   - Easy to add new operations (open for extension)
   - Don't need to modify existing code (closed for modification)

---

## 📝 Summary

**What Changed:**
- ✅ Created Helper layer (ConsoleHelper, DisplayHelper)
- ✅ Created Service layer (WebServiceInfoService, AlarmEventTypesService)
- ✅ Added GetAlarmEventTypes functionality
- ✅ Refactored Program.cs to use layers
- ✅ Clean, maintainable architecture

**Result:**
- Professional, scalable code structure
- Easy to add new SOAP operations
- Reusable components
- Clean separation of concerns
- Ready for production use

**Version:** 1.3.0 (Service Layers Architecture)
**Date:** 2026-02-12
