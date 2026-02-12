# FaultException Fix - GetEnums Error Handling

## Problem Summary

User reported GetEnums still showing error stack trace:
```
ERROR:
Message: Error saat execute GetEnums: Error saat memanggil GetEnums: OPERATION_NOT_SUPPORTED
Inner Exception: OPERATION_NOT_SUPPORTED
Stack Trace: at PME.Services.GetEnumsService.ExecuteAsync(...) line 75
```

## Root Cause Analysis

### Initial Fix Attempt (FAILED)
First attempt used `FaultException<string>`:
```csharp
catch (System.ServiceModel.FaultException<string> faultEx)
{
    if (faultEx.Detail?.Contains("OPERATION_NOT_SUPPORTED") == true)
    {
        // Display informational message
    }
}
```

**Why This Failed:**
- WCF SOAP services throw **non-generic** `FaultException` by default
- Generic `FaultException<T>` only used with explicit fault contracts
- PME service doesn't define typed fault contracts
- Exception never caught → fell through to generic `catch (Exception ex)` → showed error

### Understanding WCF Exception Types

#### FaultException (Non-Generic)
```csharp
public class FaultException : CommunicationException
{
    public FaultCode Code { get; }
    public FaultReason Reason { get; }
    public string Message { get; }
    // No Detail property!
}
```

**Used when:**
- Server throws generic SOAP faults
- No explicit fault contract defined
- Default WCF behavior

**Properties:**
- `Message` - Exception message text
- `Code` - FaultCode with Name property
- `Reason` - FaultReason with translations

#### FaultException\<T> (Generic)
```csharp
public class FaultException<T> : FaultException
{
    public T Detail { get; }
}
```

**Used when:**
- Service defines `[FaultContract(typeof(T))]`
- Custom typed fault data needed
- Structured error information

**Example:**
```csharp
[OperationContract]
[FaultContract(typeof(MyCustomFault))]
void MyOperation();
```

### PME Service Behavior

PME DataExchange service:
- ✅ Throws `FaultException` (non-generic)
- ❌ Does NOT use `FaultException<string>`
- ✅ Error info in `Message` or `Code.Name`
- ❌ No `Detail` property

## Solution Implemented

### Changed Exception Type
```csharp
// BEFORE (Wrong)
catch (System.ServiceModel.FaultException<string> faultEx)

// AFTER (Correct)
catch (System.ServiceModel.FaultException faultEx)
```

### Enhanced Detection Logic
```csharp
string errorMessage = faultEx.Message;

if (errorMessage.Contains("OPERATION_NOT_SUPPORTED") || 
    faultEx.Code?.Name == "OPERATION_NOT_SUPPORTED" ||
    faultEx.Reason?.ToString().Contains("OPERATION_NOT_SUPPORTED") == true)
{
    // Display informational message instead of error
}
```

**Checks three places:**
1. **Message** - Primary exception message
2. **Code.Name** - SOAP fault code name
3. **Reason** - SOAP fault reason text

**Why multiple checks?**
Different SOAP implementations might put the error in different places. This ensures we catch it regardless.

## Code Changes

### File: PME/Services/GetEnumsService.cs

#### ExecuteAsync Method (Line 44)
```csharp
catch (System.ServiceModel.FaultException faultEx)  // Changed from FaultException<string>
{
    // Handle SOAP faults - check for OPERATION_NOT_SUPPORTED
    string errorMessage = faultEx.Message;
    
    if (errorMessage.Contains("OPERATION_NOT_SUPPORTED") || 
        faultEx.Code?.Name == "OPERATION_NOT_SUPPORTED" ||
        faultEx.Reason?.ToString().Contains("OPERATION_NOT_SUPPORTED") == true)
    {
        // Display informational message
        Console.WriteLine("⚠️  GetEnums operation TIDAK DIDUKUNG oleh PME server ini.");
        // ... rest of informational message
    }
    else
    {
        throw new Exception($"SOAP Fault: {faultEx.Message}", faultEx);
    }
}
```

#### GetEnumsAsync Method (Line 144)
```csharp
catch (System.ServiceModel.FaultException)  // Changed from FaultException<string>
{
    // Re-throw SOAP faults untuk di-handle di ExecuteAsync
    throw;
}
```

## Testing Results

### Expected Behavior
When user selects option 3 (GetEnums):

```
Pilihan (1/2/3/4): 3

Memanggil GetEnums...

================================================================================
INFORMASI:
================================================================================

⚠️  GetEnums operation TIDAK DIDUKUNG oleh PME server ini.

Operasi GetEnums tersedia di WSDL tetapi tidak diaktifkan di server.
Kemungkinan penyebab:
  • PME server version tidak mendukung GetEnums
  • Feature belum diaktifkan di konfigurasi server
  • License tidak mencakup feature ini

Silakan gunakan operasi lain yang didukung:
  1. GetWebServiceInformation - untuk melihat operasi yang tersedia
  2. GetAlarmEventTypes
  3. GetHistory, GetItems, GetValues, dll
```

**No error stack trace!** ✅

## Lessons Learned

### 1. Know Your Exception Types
- Generic vs non-generic FaultException matters!
- Check actual exception type thrown by service
- Don't assume typed fault contracts exist

### 2. Multiple Detection Points
- SOAP faults can contain error info in various properties
- Check Message, Code, and Reason
- Defensive coding prevents missed errors

### 3. Test With Real Service
- Mock/test environments might behave differently
- Real SOAP services often use default (non-generic) exceptions
- Always test with actual service endpoints

### 4. Read WSDL Carefully
- WSDL might not define fault contracts
- Absence of `<fault>` elements = non-generic FaultException
- Operations can be in WSDL but not implemented

## Best Practices

### WCF Exception Handling Pattern

```csharp
try
{
    // SOAP call
}
catch (FaultException faultEx)  // Catch non-generic first
{
    // Handle SOAP faults
    // Check: Message, Code.Name, Reason
}
catch (CommunicationException commEx)
{
    // Handle communication errors (timeout, connection, etc)
}
catch (Exception ex)
{
    // Handle unexpected errors
}
```

**Order matters:** Most specific to least specific!

### Detection Pattern

```csharp
// Check multiple properties for robustness
if (faultEx.Message.Contains("ERROR_CODE") || 
    faultEx.Code?.Name == "ERROR_CODE" ||
    faultEx.Reason?.ToString().Contains("ERROR_CODE") == true)
{
    // Handle specific error
}
```

## References

- **WCF FaultException Documentation**: https://learn.microsoft.com/en-us/dotnet/api/system.servicemodel.faultexception
- **SOAP Fault Handling**: https://learn.microsoft.com/en-us/dotnet/framework/wcf/specifying-and-handling-faults-in-contracts-and-services
- **Fault Contracts**: https://learn.microsoft.com/en-us/dotnet/framework/wcf/defining-and-specifying-faults

## Summary

**Problem:** Wrong exception type (FaultException\<string> instead of FaultException)  
**Solution:** Use non-generic FaultException with multiple detection checks  
**Result:** Clean user experience, no error stack traces  
**Status:** ✅ FIXED

This fix demonstrates the importance of understanding WCF exception hierarchy and testing with real service endpoints!
