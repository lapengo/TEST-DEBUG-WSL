# PME SOAP Service Client

This console application demonstrates how to call the PME SOAP web service to retrieve web service information.

## Prerequisites

- .NET 10.0 SDK
- Access to the PME server (default: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`)
- Valid credentials for authentication (username and password)

## Quick Start (Recommended)

**The easiest way to configure the application is using a `.env` file:**

1. Copy the example file:
   ```bash
   cd pme-console/PME/PME
   cp ../../../.env.example .env
   ```

2. Edit `.env` with your credentials (or use the defaults for testing):
   ```bash
   # .env file content:
   PME_ENDPOINT_URL=http://beitvmpme01.beitm.id/EWS/DataExchange.svc
   PME_USERNAME=supervisor
   PME_PASSWORD=P@ssw0rdpme
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

**That's it!** The application will automatically load the configuration from the `.env` file. No need to manually set environment variables!

## Configuration

The application supports multiple ways to configure the SOAP endpoint and credentials:

### 1. .env File (Recommended - Easiest Method)

Create a `.env` file in the `pme-console/PME/PME` directory:

```bash
PME_ENDPOINT_URL=http://beitvmpme01.beitm.id/EWS/DataExchange.svc
PME_USERNAME=supervisor
PME_PASSWORD=P@ssw0rdpme
```

Then simply run:
```bash
cd pme-console/PME/PME
dotnet run
```

The application will automatically load these values. The `.env` file is excluded from git for security.

### 2. Environment Variables (Manual Method)

Set environment variables before running:

```bash
# Linux/macOS
export PME_USERNAME=your-username
export PME_PASSWORD='your-password'
export PME_ENDPOINT_URL=http://your-server.com/EWS/DataExchange.svc

# Windows PowerShell
$env:PME_USERNAME="your-username"
$env:PME_PASSWORD="your-password"
$env:PME_ENDPOINT_URL="http://your-server.com/EWS/DataExchange.svc"

# Windows Command Prompt
set PME_USERNAME=your-username
set PME_PASSWORD=your-password
set PME_ENDPOINT_URL=http://your-server.com/EWS/DataExchange.svc
```

### 3. Command-Line Argument (For Endpoint Only)

You can specify a custom endpoint URL as a command-line argument:

```bash
cd pme-console/PME/PME
dotnet run -- http://your-server.com/EWS/DataExchange.svc
```

**Priority Order:** Command-line argument > Environment variable > .env file > Default hardcoded value

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
- **CRITICAL SECURITY WARNING**: The default endpoint uses HTTP (not HTTPS), which means credentials and all data are transmitted in plaintext over the network. This is a significant security risk.
  - For production use, ensure the service is configured to use HTTPS
  - If HTTPS is not available, ensure the service is only accessible on a secure/private network
  - Consider the security implications before transmitting credentials over HTTP
- Use the `.env.example` file as a template and create a `.env` file with your actual credentials
- The `.env` file is automatically excluded from git commits via `.gitignore`
- Consider using a secure credential store for production deployments
- Rotate passwords regularly
- Use strong, unique passwords

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
- Contact your system administrator for the correct credentials

## Notes

- The endpoint can be configured via command-line, environment variable, or uses the default from WSDL
- The application includes comprehensive error handling for network and SOAP faults
- Build artifacts are excluded from source control via `.gitignore`
- The application exits with code 1 on errors for easier integration in scripts/CI
- **Security**: Default configuration uses HTTP - see Security Considerations section for important warnings
