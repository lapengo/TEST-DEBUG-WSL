# PME SOAP Service Client

This console application demonstrates how to call the PME SOAP web service to retrieve web service information.

## Prerequisites

- .NET 10.0 SDK
- Access to the PME server at `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`

## Usage

The application implements a method `GetWebServiceInformationAsync()` that:
1. Creates a SOAP client connection to the PME DataExchange service
2. Sends a `GetWebServiceInformationRequest` 
3. Returns and displays the service information including:
   - Web service version
   - Supported operations
   - Supported profiles
   - System information

### Running the Application

```bash
cd pme-console/PME
dotnet run --project PME.csproj
```

### Expected Output

When successfully connected to the PME server, the application will display:
- Web Service Version (Major and Minor)
- Namespace being used
- List of supported operations
- List of supported profiles
- System information (ID, Name, Version)

## Implementation Details

The main method `GetWebServiceInformationAsync()` uses the auto-generated WSDL service reference located in `Connected Services/WSDL/Reference.cs`.

The method signature:
```csharp
static async Task<GetWebServiceInformationResponse?> GetWebServiceInformationAsync()
```

This method:
- Uses the `DataExchangeClient` class from the WSDL service reference
- Creates a `GetWebServiceInformationRequest` with version "1.0"
- Calls the async SOAP method
- Returns the parsed response data

## Notes

- The endpoint is configured in the auto-generated code at `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`
- The application includes comprehensive error handling for network and SOAP faults
- Build artifacts are excluded from source control via `.gitignore`
