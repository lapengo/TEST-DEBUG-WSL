# Fix Summary: GetItems MISSING_ID_LIST Error

## Problem Statement
When users selected option 4 (GetItems) from the menu, the application crashed with an error:
```
Error saat execute GetItems: Error saat memanggil GetItems: MISSING_ID_LIST
```

This occurred because the PME DataExchange server requires that GetItems be called with specific item IDs, but the application was calling it with null IDs.

## Root Cause
The `GetItemsService.ExecuteAsync()` method was being called without item IDs:
```csharp
await service.ExecuteAsync(version, null, false);
```

The PME server requires a non-empty list of item IDs for the GetItems operation, otherwise it returns a MISSING_ID_LIST fault.

## Solution Implemented

### 1. Enhanced Error Handling
Updated `GetItemsService.cs` to catch and handle the MISSING_ID_LIST error gracefully:

```csharp
else if (errorMessage.Contains("MISSING_ID_LIST") ||
         faultEx.Code?.Name == "MISSING_ID_LIST" ||
         faultEx.Reason?.ToString().Contains("MISSING_ID_LIST") == true)
{
    ConsoleHelper.PrintSectionHeader("INFORMASI");
    Console.WriteLine();
    Console.WriteLine("⚠️  GetItems memerlukan daftar Item IDs yang spesifik.");
    Console.WriteLine();
    Console.WriteLine("GetItems tidak dapat dipanggil tanpa Item IDs.");
    Console.WriteLine();
    Console.WriteLine("Cara mendapatkan Item IDs:");
    Console.WriteLine("  1. Gunakan GetContainerItems terlebih dahulu untuk melihat item yang tersedia");
    Console.WriteLine("  2. Catat ID dari item yang ingin Anda query");
    Console.WriteLine("  3. Untuk saat ini, GetItems di-skip karena memerlukan kustomisasi kode");
    Console.WriteLine();
    Console.WriteLine("Alternatif:");
    Console.WriteLine("  • GetContainerItems - melihat semua item dalam container (lebih berguna)");
    Console.WriteLine("  • GetValues - mendapatkan nilai dari item tertentu (jika sudah tahu ID-nya)");
    Console.WriteLine();
}
```

### 2. Documentation
Created `KNOWN_LIMITATIONS.md` explaining:
- Why GetItems requires item IDs
- How to discover item IDs using GetContainerItems
- Example code for customizing GetItems with specific IDs
- Alternative operations that work without IDs

### 3. Updated User Guide
Modified `FUNCTIONS.md` to highlight operations that require specific parameters and reference the limitations document.

## Behavior After Fix

### Before:
- Application crashed with stack trace
- No guidance on what went wrong
- User confused about how to use GetItems

### After:
- Error caught gracefully
- Clear, informative message displayed in Indonesian
- Step-by-step guidance on how to get item IDs
- Suggestions for alternative operations
- Application continues running

## Example Output
When user selects option 4, they now see:

```
================================================================================
GetItems
================================================================================

================================================================================
INFORMASI:
================================================================================

⚠️  GetItems memerlukan daftar Item IDs yang spesifik.

GetItems tidak dapat dipanggil tanpa Item IDs.

Cara mendapatkan Item IDs:
  1. Gunakan GetContainerItems terlebih dahulu untuk melihat item yang tersedia
  2. Catat ID dari item yang ingin Anda query
  3. Untuk saat ini, GetItems di-skip karena memerlukan kustomisasi kode

Alternatif:
  • GetContainerItems - melihat semua item dalam container (lebih berguna)
  • GetValues - mendapatkan nilai dari item tertentu (jika sudah tahu ID-nya)
```

## Testing
- ✅ Build successful (0 errors, 0 warnings)
- ✅ Error is caught and handled gracefully
- ✅ Informative message displayed to user
- ✅ Application continues running after error
- ✅ Documentation updated

## Future Enhancements (Optional)
If desired, the application could be enhanced to:
1. Prompt user for item IDs interactively
2. Automatically fetch item IDs from GetContainerItems first
3. Store frequently-used item IDs in configuration

However, the current fix provides clear guidance for users to customize the code themselves, which aligns with the minimal-change approach.

## Files Changed
1. `PME/Services/GetItemsService.cs` - Added MISSING_ID_LIST error handling
2. `PME/KNOWN_LIMITATIONS.md` - Created documentation (new file)
3. `PME/FUNCTIONS.md` - Updated with limitation notes

## Verification
The fix ensures that:
- ✅ No more crashes when selecting GetItems
- ✅ User understands why GetItems needs IDs
- ✅ User knows how to get IDs (use GetContainerItems)
- ✅ User knows what to do next (customize code or use alternatives)
- ✅ Consistent with error handling for other operations (e.g., OPERATION_NOT_SUPPORTED)
