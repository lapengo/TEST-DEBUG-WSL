# PME SOAP Service Client

This console application demonstrates how to call the PME SOAP web service to retrieve web service information.

## Prerequisites

- .NET 10.0 SDK
- Access to the PME server (default: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl`)
- Valid credentials for authentication (username and password)

## Quick Start

**The application uses `appsettings.json` for configuration:**

1. Copy the example file:
   ```bash
   cd pme-console/PME/PME
   cp appsettings.example.json appsettings.json
   ```

2. Edit `appsettings.json` with your credentials:
   ```json
   {
     "PmeService": {
       "EndpointUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl",
       "Username": "supervisor",
       "Password": "P@ssw0rdpme"
     }
   }
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

**That's it!** The application will automatically load the configuration from `appsettings.json`.

## Configuration

The application uses `appsettings.json` for configuration. This file contains:

- **EndpointUrl**: The SOAP service WSDL endpoint URL (with ?singleWsdl parameter)
- **Username**: Username for Digest authentication
- **Password**: Password for Digest authentication

### appsettings.json Format

```json
{
  "PmeService": {
    "EndpointUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl",
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

### Command-Line Override

You can override the endpoint URL using a command-line argument:

```bash
dotnet run -- http://custom-server.com/EWS/DataExchange.svc
```

**Priority Order:** Command-line argument > appsettings.json

## Usage

The application implements a method `GetWebServiceInformationAsync()` that:
1. Creates a SOAP client connection to the PME DataExchange service with Digest authentication
2. Sends a `GetWebServiceInformationRequest` 
3. Returns and displays the service information including:
   - Web service version
   - Supported operations
   - Supported profiles
   - System information

### Authentication

The service uses **Digest authentication** (MD5 or SHA-256). The application automatically configures the SOAP client with the credentials from `appsettings.json`.

### Running the Application

```bash
cd pme-console/PME/PME
dotnet run
```

### Expected Output

When successfully connected to the PME server, the application will display:
- Configuration source (appsettings.json)
- Username being used
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
- Instructions on how to configure via appsettings.json

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

Configuration options:
  - Edit appsettings.json to change endpoint URL and credentials
  - Command line: dotnet run -- <endpoint-url>
```

## Security Considerations

- **Never commit credentials to source control**
- The `appsettings.json` file is excluded from git via `.gitignore` to protect credentials
- Use `appsettings.example.json` as a template
- **CRITICAL SECURITY WARNING**: The default endpoint uses HTTP (not HTTPS), which means credentials and all data are transmitted in plaintext over the network. This is a significant security risk.
  - For production use, ensure the service is configured to use HTTPS
  - If HTTPS is not available, ensure the service is only accessible on a secure/private network
  - Consider the security implications before transmitting credentials over HTTP
- The application uses **Digest authentication** which provides better security than Basic authentication
- Rotate passwords regularly
- Use strong, unique passwords

## Implementation Details

The main method `GetWebServiceInformationAsync()` uses the auto-generated WSDL service reference located in `Connected Services/WSDL/Reference.cs`.

The method signature:
```csharp
static async Task<GetWebServiceInformationResponse?> GetWebServiceInformationAsync(
    string endpointUrl, 
    (string Username, string Password)? credentials)
```

This method:
- Uses the `DataExchangeClient` class from the WSDL service reference
- Accepts the endpoint URL from configuration
- Accepts credentials from appsettings.json
- Configures **Digest authentication** on the SOAP client
- Creates a `GetWebServiceInformationRequest` with version "1.0"
- Calls the async SOAP method
- Returns the parsed response data

## Troubleshooting

### Connection Issues
If you encounter `EndpointNotFoundException`:
1. Verify network connectivity to the target server
2. Check if the service is running on the expected port
3. Ensure firewall rules allow outbound connections
4. Verify the endpoint URL in appsettings.json is correct
5. Try using a custom endpoint via command-line argument

### SSL/TLS Issues
If you encounter certificate validation errors:
- Ensure the server certificate is valid
- Check that your system trusts the certificate authority

### Authentication Issues
If you encounter authentication errors:
- Verify the username and password in appsettings.json are correct
- The service requires Digest authentication (MD5 or SHA-256)
- Check that the credentials have the necessary permissions
- Contact your system administrator for the correct credentials

## Notes

- Configuration is read from `appsettings.json`
- Command-line argument can override the endpoint URL
- The application uses Digest authentication for secure credential transmission
- The application includes comprehensive error handling for network and SOAP faults
- Build artifacts are excluded from source control via `.gitignore`
- The application exits with code 1 on errors for easier integration in scripts/CI
- **Security**: Default configuration uses HTTP - see Security Considerations section for important warnings
