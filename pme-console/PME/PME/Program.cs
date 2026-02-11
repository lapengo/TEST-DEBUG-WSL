using WSDL;
using System.ServiceModel;

DotNetEnv.Env.Load();

Console.WriteLine("PME SOAP Service Client");
Console.WriteLine("=======================\n");

// Get endpoint URL from command-line args, environment variable, or use default
string endpointUrl = GetEndpointUrl(args);

// Get credentials from environment variables
var credentials = GetCredentials();

Console.WriteLine($"Using endpoint: {endpointUrl}");
if (credentials.HasValue)
{
    Console.WriteLine($"Using authentication: {credentials.Value.Username}");
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
    Console.WriteLine("  - Command line: dotnet run -- <endpoint-url>");
    Console.WriteLine("  - Environment variable: PME_ENDPOINT_URL=<endpoint-url>");
    Console.WriteLine("  - Credentials: PME_USERNAME and PME_PASSWORD environment variables");
    
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

static string GetEndpointUrl(string[] args)
{
    // First priority: command-line argument
    if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
    {
        return args[0];
    }
    
    // Second priority: environment variable
    string? envEndpoint = Environment.GetEnvironmentVariable("PME_ENDPOINT_URL");
    if (!string.IsNullOrWhiteSpace(envEndpoint))
    {
        return envEndpoint;
    }
    
    // Use default endpoint from WSDL
    return "http://beitvmpme01.beitm.id/EWS/DataExchange.svc";
}

static (string Username, string Password)? GetCredentials()
{
    string? username = "supervisor";
    string? password = "P@ssw0rdpme";
    
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
        // Configure credentials if provided
        if (credentials.HasValue)
        {
            client.ClientCredentials.UserName.UserName = credentials.Value.Username;
            client.ClientCredentials.UserName.Password = credentials.Value.Password;
        }
        
        var request = new GetWebServiceInformationRequest
        {
            version = "1.0"
        };
        
        var response = await client.GetWebServiceInformationAsync(request);
        return response?.GetWebServiceInformationResponse;
    }
}
