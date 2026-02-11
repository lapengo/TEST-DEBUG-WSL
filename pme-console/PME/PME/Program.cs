using WSDL;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Sockets;

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var endpoint = config["PmeService:EndpointUrl"] ?? "http://beitvmpme01.beitm.id/EWS/DataExchange.svc";
var username = config["PmeService:Username"];
var password = config["PmeService:Password"];

Console.WriteLine("PME SOAP Service Client");
Console.WriteLine("=======================\n");

// Validate endpoint URL - warn if ?singleWsdl is included
if (endpoint.Contains("?singleWsdl", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("WARNING: Endpoint URL contains '?singleWsdl'");
    Console.WriteLine("This parameter is for WSDL retrieval only, not for SOAP calls.");
    Console.WriteLine("Removing '?singleWsdl' from endpoint URL...\n");
    Console.ResetColor();
    
    endpoint = endpoint.Replace("?singleWsdl", "", StringComparison.OrdinalIgnoreCase);
}

// Perform network diagnostics
await PerformNetworkDiagnostics(endpoint);

try
{
    // Create SOAP client
    var client = new DataExchangeClient(
        DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,
        endpoint
    );

    using (client)
    {
        // Configure timeouts
        client.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(30);
        client.Endpoint.Binding.CloseTimeout = TimeSpan.FromSeconds(30);
        client.Endpoint.Binding.SendTimeout = TimeSpan.FromSeconds(60);
        client.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromSeconds(60);
        
        // Configure authentication
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            client.ClientCredentials.HttpDigest.ClientCredential = 
                new System.Net.NetworkCredential(username, password);
            Console.WriteLine($"Endpoint: {endpoint}");
            Console.WriteLine($"User: {username}\n");
        }

        Console.WriteLine("Calling GetWebServiceInformation...");
        Console.WriteLine("Please wait, this may take a few seconds...\n");
        
        // Call GetWebServiceInformation with retry logic
        var serviceInfo = await CallWithRetryAsync(async () =>
        {
            var request = new GetWebServiceInformationRequest { version = "1.0" };
            var response = await client.GetWebServiceInformationAsync(request);
            return response?.GetWebServiceInformationResponse;
        }, maxRetries: 3, delaySeconds: 2);

        if (serviceInfo != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Successfully retrieved service information!");
            Console.ResetColor();
            Console.WriteLine();
            
            Console.WriteLine("Web Service Information");
            Console.WriteLine("=======================");
            Console.WriteLine($"Version: {serviceInfo.version}");

            if (serviceInfo.GetWebServiceInformationVersion != null)
            {
                Console.WriteLine($"\nVersion Details:");
                Console.WriteLine($"  Major: {serviceInfo.GetWebServiceInformationVersion.MajorVersion}");
                Console.WriteLine($"  Minor: {serviceInfo.GetWebServiceInformationVersion.MinorVersion}");
                Console.WriteLine($"  Namespace: {serviceInfo.GetWebServiceInformationVersion.UsedNameSpace}");
            }

            if (serviceInfo.GetWebServiceInformationSupportedOperations != null)
            {
                Console.WriteLine($"\nSupported Operations ({serviceInfo.GetWebServiceInformationSupportedOperations.Length}):");
                foreach (var op in serviceInfo.GetWebServiceInformationSupportedOperations)
                {
                    Console.WriteLine($"  - {op}");
                }
            }

            if (serviceInfo.GetWebServiceInformationSupportedProfiles != null)
            {
                Console.WriteLine($"\nSupported Profiles ({serviceInfo.GetWebServiceInformationSupportedProfiles.Length}):");
                foreach (var profile in serviceInfo.GetWebServiceInformationSupportedProfiles)
                {
                    Console.WriteLine($"  - {profile}");
                }
            }

            if (serviceInfo.SystemInfo != null)
            {
                Console.WriteLine($"\nSystem Information:");
                Console.WriteLine($"  ID: {serviceInfo.SystemInfo.Id}");
                Console.WriteLine($"  Name: {serviceInfo.SystemInfo.Name}");
                Console.WriteLine($"  Version: {serviceInfo.SystemInfo.Version}");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠ No data received from service.");
            Console.ResetColor();
        }
    }
    
    Console.WriteLine("\n--- Execution completed successfully ---");
}
catch (EndpointNotFoundException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║         ENDPOINT CONNECTION ERROR                              ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
    Console.ResetColor();
    
    Console.WriteLine($"\nError: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Details: {ex.InnerException.Message}");
    }
    
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\n📋 TROUBLESHOOTING STEPS:");
    Console.ResetColor();
    
    Console.WriteLine("\n1. VERIFY NETWORK CONNECTIVITY:");
    Console.WriteLine($"   • Can you ping {ExtractHostname(endpoint)}?");
    Console.WriteLine("   • Are you connected to the required network/VPN?");
    Console.WriteLine("   • Is there a firewall blocking the connection?");
    
    Console.WriteLine("\n2. CHECK ENDPOINT CONFIGURATION:");
    Console.WriteLine($"   • Current endpoint: {endpoint}");
    Console.WriteLine("   • Verify this is the correct URL in appsettings.json");
    Console.WriteLine("   • Ensure URL does NOT include ?singleWsdl");
    
    Console.WriteLine("\n3. VERIFY SERVICE STATUS:");
    Console.WriteLine("   • Is the PME server running?");
    Console.WriteLine("   • Is the DataExchange service deployed?");
    Console.WriteLine("   • Check with your system administrator");
    
    Console.WriteLine("\n4. CONFIGURATION FILE:");
    Console.WriteLine("   • Location: appsettings.json");
    Console.WriteLine("   • Format:");
    Console.WriteLine("     {");
    Console.WriteLine("       \"PmeService\": {");
    Console.WriteLine("         \"EndpointUrl\": \"http://your-server/EWS/DataExchange.svc\",");
    Console.WriteLine("         \"Username\": \"your-username\",");
    Console.WriteLine("         \"Password\": \"your-password\"");
    Console.WriteLine("       }");
    Console.WriteLine("     }");
    
    Console.WriteLine("\n--- Execution completed with errors ---");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║         UNEXPECTED ERROR                                       ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
    Console.ResetColor();
    
    Console.WriteLine($"\nError: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Details: {ex.InnerException.Message}");
    }
    Console.WriteLine($"\nException Type: {ex.GetType().Name}");
    Console.WriteLine("\n--- Execution completed with errors ---");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

// Helper function to perform network diagnostics
static async Task PerformNetworkDiagnostics(string endpointUrl)
{
    try
    {
        var uri = new Uri(endpointUrl);
        var hostname = uri.Host;
        
        Console.WriteLine("Network Diagnostics:");
        Console.WriteLine("--------------------");
        
        // DNS Resolution
        try
        {
            var addresses = await Dns.GetHostAddressesAsync(hostname);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ DNS Resolution: SUCCESS");
            Console.ResetColor();
            Console.WriteLine($"  Host: {hostname}");
            Console.WriteLine($"  IP Address(es): {string.Join(", ", addresses.Select(a => a.ToString()))}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ DNS Resolution: FAILED");
            Console.ResetColor();
            Console.WriteLine($"  Host: {hostname}");
            Console.WriteLine($"  Error: {ex.Message}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ⚠ The hostname cannot be resolved. Check network/VPN connection.");
            Console.ResetColor();
        }
        
        // Port connectivity check
        var port = uri.Port > 0 ? uri.Port : (uri.Scheme == "https" ? 443 : 80);
        try
        {
            using var tcpClient = new TcpClient();
            var connectTask = tcpClient.ConnectAsync(hostname, port);
            if (await Task.WhenAny(connectTask, Task.Delay(5000)) == connectTask)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Port Connectivity: SUCCESS");
                Console.ResetColor();
                Console.WriteLine($"  Port {port} is reachable");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Port Connectivity: TIMEOUT");
                Console.ResetColor();
                Console.WriteLine($"  Port {port} connection timed out");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  ⚠ Server may be offline or firewall is blocking connection.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ Port Connectivity: FAILED");
            Console.ResetColor();
            Console.WriteLine($"  Port: {port}");
            Console.WriteLine($"  Error: {ex.Message}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ⚠ Cannot connect to server. Check if server is running and accessible.");
            Console.ResetColor();
        }
        
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠ Network diagnostics error: {ex.Message}\n");
        Console.ResetColor();
    }
}

static string ExtractHostname(string url)
{
    try
    {
        return new Uri(url).Host;
    }
    catch
    {
        return url;
    }
}

// Helper function to retry SOAP calls
static async Task<T?> CallWithRetryAsync<T>(Func<Task<T?>> operation, int maxRetries = 3, int delaySeconds = 2) where T : class
{
    int attempt = 0;
    while (attempt < maxRetries)
    {
        attempt++;
        try
        {
            Console.WriteLine($"Attempt {attempt} of {maxRetries}...");
            var result = await operation();
            return result;
        }
        catch (EndpointNotFoundException ex) when (attempt < maxRetries)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ Attempt {attempt} failed: {ex.Message}");
            Console.ResetColor();
            
            if (attempt < maxRetries)
            {
                Console.WriteLine($"Retrying in {delaySeconds} seconds...\n");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }
        catch (TimeoutException ex) when (attempt < maxRetries)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ Attempt {attempt} timed out: {ex.Message}");
            Console.ResetColor();
            
            if (attempt < maxRetries)
            {
                Console.WriteLine($"Retrying in {delaySeconds} seconds...\n");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }
        catch (Exception ex) when (attempt < maxRetries)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ Attempt {attempt} failed: {ex.GetType().Name} - {ex.Message}");
            Console.ResetColor();
            
            if (attempt < maxRetries)
            {
                Console.WriteLine($"Retrying in {delaySeconds} seconds...\n");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }
    }
    
    // All retries failed, throw the last exception
    throw new Exception($"All {maxRetries} attempts failed. Please check the error messages above.");
}
