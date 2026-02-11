using WSDL;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;

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

try
{
    // Create SOAP client
    var client = new DataExchangeClient(
        DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,
        endpoint
    );

    using (client)
    {
        // Configure authentication
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            client.ClientCredentials.HttpDigest.ClientCredential = 
                new System.Net.NetworkCredential(username, password);
            Console.WriteLine($"Endpoint: {endpoint}");
            Console.WriteLine($"User: {username}\n");
        }

        // Call GetWebServiceInformation
        var request = new GetWebServiceInformationRequest { version = "1.0" };
        var response = await client.GetWebServiceInformationAsync(request);
        var serviceInfo = response?.GetWebServiceInformationResponse;

        if (serviceInfo != null)
        {
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
            Console.WriteLine("No data received from service.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nError: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Details: {ex.InnerException.Message}");
    }
    Environment.Exit(1);
}
