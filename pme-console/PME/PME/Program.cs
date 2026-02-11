using WSDL;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;

// Load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Console.WriteLine("PME SOAP Service Client");
Console.WriteLine("=======================\n");

// Get endpoint URL from command-line args or configuration
string endpointUrl = GetEndpointUrl(args, configuration);

// Get credentials from configuration
var credentials = GetCredentials(configuration);

// Display configuration source
Console.WriteLine("Configuration loaded:");
if (args.Length > 0)
{
    Console.WriteLine("  Endpoint: Command-line argument");
}
else
{
    Console.WriteLine("  Endpoint: appsettings.json");
}

if (credentials.HasValue)
{
    Console.WriteLine($"  Username: {credentials.Value.Username}");
    Console.WriteLine($"  Password: ***");
}
else
{
    Console.WriteLine("  Authentication: Not configured");
}

Console.WriteLine($"\nUsing endpoint: {endpointUrl}");
if (credentials.HasValue)
{
    Console.WriteLine($"Using authentication: {credentials.Value.Username}");
}
else
{
    Console.WriteLine("WARNING: No credentials configured. Authentication may fail.");
}
Console.WriteLine();

try
{
    var serviceInfo = await GetWebServiceInformationAsync(endpointUrl, credentials);
    
    if (serviceInfo != null)
    {
        Console.WriteLine("Web Service Information Retrieved Successfully!");
        Console.WriteLine($"Version: {serviceInfo.version}");
        
        if (serviceInfo.GetWebServiceInformationVersion != null)
        {
            Console.WriteLine($"Major Version: {serviceInfo.GetWebServiceInformationVersion.MajorVersion}");
            Console.WriteLine($"Minor Version: {serviceInfo.GetWebServiceInformationVersion.MinorVersion}");
            Console.WriteLine($"Namespace: {serviceInfo.GetWebServiceInformationVersion.UsedNameSpace}");
        }
        
        if (serviceInfo.GetWebServiceInformationSupportedOperations != null)
        {
            Console.WriteLine("\nSupported Operations:");
            foreach (var operation in serviceInfo.GetWebServiceInformationSupportedOperations)
            {
                Console.WriteLine($"  - {operation}");
            }
        }
        
        if (serviceInfo.GetWebServiceInformationSupportedProfiles != null)
        {
            Console.WriteLine("\nSupported Profiles:");
            foreach (var profile in serviceInfo.GetWebServiceInformationSupportedProfiles)
            {
                Console.WriteLine($"  - {profile}");
            }
        }
        
        if (serviceInfo.SystemInfo != null)
        {
            Console.WriteLine("\nSystem Information:");
            Console.WriteLine($"  ID: {serviceInfo.SystemInfo.Id}");
            Console.WriteLine($"  Name: {serviceInfo.SystemInfo.Name}");
            Console.WriteLine($"  Version: {serviceInfo.SystemInfo.Version}");
        }
    }
    else
    {
        Console.WriteLine("No data received from web service.");
    }
}
catch (EndpointNotFoundException ex)
{
    Console.WriteLine("ERROR: Unable to connect to the SOAP service endpoint.");
    Console.WriteLine($"Endpoint: {endpointUrl}");
    Console.WriteLine($"\nDetails: {ex.Message}");
    
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    
    Console.WriteLine("\nTroubleshooting:");
    Console.WriteLine("1. Verify the endpoint URL is correct");
    Console.WriteLine("2. Check network connectivity to the server");
    Console.WriteLine("3. Ensure the service is running on the target server");
    Console.WriteLine("4. Check firewall settings");
    Console.WriteLine("\nConfiguration options:");
    Console.WriteLine("  - Edit appsettings.json to change endpoint URL and credentials");
    Console.WriteLine("  - Command line: dotnet run -- <endpoint-url>");
    
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"Error calling web service: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
    Environment.Exit(1);
}

static string GetEndpointUrl(string[] args, IConfiguration configuration)
{
    // First priority: command-line argument
    if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
    {
        return args[0];
    }
    
    // Second priority: appsettings.json
    string? endpoint = configuration["PmeService:EndpointUrl"];
    if (!string.IsNullOrWhiteSpace(endpoint))
    {
        return endpoint;
    }
    
    // Use default endpoint from WSDL
    return "http://beitvmpme01.beitm.id/EWS/DataExchange.svc";
}

static (string Username, string Password)? GetCredentials(IConfiguration configuration)
{
    string? username = configuration["PmeService:Username"];
    string? password = configuration["PmeService:Password"];
    
    if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
    {
        return (username, password);
    }
    
    return null;
}

static async Task<GetWebServiceInformationResponse?> GetWebServiceInformationAsync(
    string endpointUrl, 
    (string Username, string Password)? credentials)
{
    // Use custom endpoint
    var client = new DataExchangeClient(
        DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,
        endpointUrl
    );
    
    using (client)
    {
        // Configure Digest authentication if credentials are provided
        if (credentials.HasValue)
        {
            // Use HttpDigest authentication for Digest authentication scheme
            client.ClientCredentials.HttpDigest.ClientCredential = 
                new System.Net.NetworkCredential(
                    credentials.Value.Username, 
                    credentials.Value.Password);
        }
        
        var request = new GetWebServiceInformationRequest
        {
            version = "1.0"
        };
        
        var response = await client.GetWebServiceInformationAsync(request);
        return response?.GetWebServiceInformationResponse;
    }
}
