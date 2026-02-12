using PME.Helpers;

namespace PME.Services;

public class GetSystemEventTypesService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetSystemEventTypesService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetSystemEventTypes");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetSystemEventTypesRequest
            {
                version = version
            };

            var soapResponse = await client.GetSystemEventTypesAsync(new wsdl.GetSystemEventTypesRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetSystemEventTypesResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetSystemEventTypesResponse.version}");
                Console.WriteLine();
            }

            // Display system event types if available
            if (soapResponse.GetSystemEventTypesResponse.GetSystemEventTypesTypes != null)
            {
                var types = soapResponse.GetSystemEventTypesResponse.GetSystemEventTypesTypes;
                ConsoleHelper.PrintSectionHeader($"SYSTEM EVENT TYPES ({types.Length} types):");
                Console.WriteLine();
                foreach (var type in types)
                {
                    ConsoleHelper.PrintListItem(type);
                }
                Console.WriteLine();
            }
            else
            {
                ConsoleHelper.PrintSectionHeader("SYSTEM EVENT TYPES: Tidak ada data");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetSystemEventTypes!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetSystemEventTypes: {ex.Message}", ex);
        }
    }

    private void HandleFaultException(System.ServiceModel.FaultException faultEx)
    {
        string errorMessage = faultEx.Message;

        if (errorMessage.Contains("OPERATION_NOT_SUPPORTED") ||
            faultEx.Code?.Name == "OPERATION_NOT_SUPPORTED" ||
            faultEx.Reason?.ToString().Contains("OPERATION_NOT_SUPPORTED") == true)
        {
            ConsoleHelper.PrintSectionHeader("INFORMASI");
            Console.WriteLine();
            Console.WriteLine("⚠️  GetSystemEventTypes operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetSystemEventTypes: {errorMessage}", faultEx);
        }
    }
}
