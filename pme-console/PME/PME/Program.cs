using WSDL;

Console.WriteLine("PME SOAP Service Client");
Console.WriteLine("=======================\n");

try
{
    var serviceInfo = await GetWebServiceInformationAsync();
    
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
catch (Exception ex)
{
    Console.WriteLine($"Error calling web service: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
    }
}

static async Task<GetWebServiceInformationResponse?> GetWebServiceInformationAsync()
{
    using (var client = new DataExchangeClient())
    {
        var request = new GetWebServiceInformationRequest
        {
            version = "1.0"
        };
        
        var response = await client.GetWebServiceInformationAsync(request);
        return response?.GetWebServiceInformationResponse;
    }
}
