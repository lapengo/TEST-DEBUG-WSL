# PME SOAP Service Client

This console application demonstrates how to call the PME SOAP web service to retrieve web service information.

## Prerequisites

- .NET 10.0 SDK
- Access to the PME server (default: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`)
- Valid credentials for authentication (username and password)

## Configuration

The application supports multiple ways to configure the SOAP endpoint and credentials:

### Authentication

The service requires username and password authentication. Configure credentials using environment variables:

```bash
# Linux/macOS
export PME_USERNAME=supervisor
export PME_PASSWORD='P@ssw0rdpme'

# Windows PowerShell
$env:PME_USERNAME="supervisor"
$env:PME_PASSWORD="P@ssw0rdpme"

# Windows Command Prompt
set PME_USERNAME=supervisor
set PME_PASSWORD=P@ssw0rdpme
```

### Endpoint Configuration

### 1. Default Endpoint (Hardcoded)
By default, the application uses `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`

### 2. Command-Line Argument
You can specify a custom endpoint URL as a command-line argument:

```bash
cd pme-console/PME
dotnet run --project PME.csproj -- http://your-server.com/EWS/DataExchange.svc
```

### 3. Environment Variable
Set the `PME_ENDPOINT_URL` environment variable:

```bash
# Linux/macOS
export PME_ENDPOINT_URL=http://your-server.com/EWS/DataExchange.svc
export PME_USERNAME=supervisor
export PME_PASSWORD='P@ssw0rdpme'
dotnet run --project PME.csproj

# Windows PowerShell
$env:PME_ENDPOINT_URL="http://your-server.com/EWS/DataExchange.svc"
$env:PME_USERNAME="supervisor"
$env:PME_PASSWORD="P@ssw0rdpme"
dotnet run --project PME.csproj

# Windows Command Prompt
set PME_ENDPOINT_URL=http://your-server.com/EWS/DataExchange.svc
set PME_USERNAME=supervisor
set PME_PASSWORD=P@ssw0rdpme
dotnet run --project PME.csproj
```

**Priority Order:** Command-line argument > Environment variable > Default hardcoded value

### Full Example with Authentication

```bash
# Linux/macOS
export PME_USERNAME=supervisor
export PME_PASSWORD='P@ssw0rdpme'
cd pme-console/PME
dotnet run --project PME.csproj

# Or with custom endpoint
export PME_ENDPOINT_URL=http://beitvmpme01.beitm.id/EWS/DataExchange.svc
export PME_USERNAME=supervisor
export PME_PASSWORD='P@ssw0rdpme'
dotnet run --project PME.csproj
```

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

### Error Handling

If the endpoint is not accessible, the application provides detailed error information:
- Clear error message indicating connection failure
- The endpoint URL that was attempted
- Troubleshooting steps
- Instructions on how to configure a custom endpoint

Example error output:
```
ERROR: Unable to connect to the SOAP service endpoint.
Endpoint: http://beitvmpme01.beitm.id/EWS/DataExchange.svc

Details: There was no endpoint listening at http://beitvmpme01.beitm.id/EWS/DataExchange.svc...

Troubleshooting:
1. Verify the endpoint URL is correct
2. Check network connectivity to the server
3. Ensure the service is running on the target server
4. Check firewall settings

You can specify a custom endpoint using:
  - Command line: dotnet run -- <endpoint-url>
  - Environment variable: PME_ENDPOINT_URL=<endpoint-url>
  - Credentials: PME_USERNAME and PME_PASSWORD environment variables
```

## Security Considerations

- **Never commit credentials to source control**
- Use environment variables to store sensitive credentials
- The password is transmitted over the network - ensure you're using HTTPS in production
- Consider using a secure credential store for production deployments

## Implementation Details

The main method `GetWebServiceInformationAsync()` uses the auto-generated WSDL service reference located in `Connected Services/WSDL/Reference.cs`.

The method signature:
```csharp
static async Task<GetWebServiceInformationResponse?> GetWebServiceInformationAsync(
    string? endpointUrl, 
    (string Username, string Password)? credentials)
```

This method:
- Uses the `DataExchangeClient` class from the WSDL service reference
- Accepts an optional custom endpoint URL
- Accepts optional credentials (username and password)
- Configures the client with the provided credentials
- Creates a `GetWebServiceInformationRequest` with version "1.0"
- Calls the async SOAP method
- Returns the parsed response data

## Troubleshooting

### Connection Issues
If you encounter `EndpointNotFoundException`:
1. Verify network connectivity to the target server
2. Check if the service is running on the expected port
3. Ensure firewall rules allow outbound connections
4. Verify the endpoint URL is correct
5. Try using a custom endpoint if the default is not accessible in your environment

### SSL/TLS Issues
If you encounter certificate validation errors:
- Ensure the server certificate is valid
- Check that your system trusts the certificate authority

### Authentication Issues
If you encounter authentication errors:
- Verify the username and password are correct
- Ensure `PME_USERNAME` and `PME_PASSWORD` environment variables are set
- Check that the credentials have the necessary permissions

## Default Credentials

For the default PME server at `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`:
- WSDL URL: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl`
- Default Username: `supervisor`
- Default Password: Contact your system administrator

Set these using environment variables before running the application.

## Notes

- The endpoint can be configured via command-line, environment variable, or uses the default from WSDL
- The application includes comprehensive error handling for network and SOAP faults
- Build artifacts are excluded from source control via `.gitignore`
- The application exits with code 1 on errors for easier integration in scripts/CI
