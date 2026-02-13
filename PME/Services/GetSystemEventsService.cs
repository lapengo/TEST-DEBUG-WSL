using PME.Helpers;

namespace PME.Services;

public class GetSystemEventsService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetSystemEventsService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetSystemEvents");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetSystemEventsRequest
            {
                version = version
            };

            var soapResponse = await client.GetSystemEventsAsync(new wsdl.GetSystemEventsRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetSystemEventsResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetSystemEventsResponse.version}");
                Console.WriteLine();
            }

            // Display system events if available
            if (soapResponse.GetSystemEventsResponse.GetSystemEventsSystemEvents != null)
            {
                var events = soapResponse.GetSystemEventsResponse.GetSystemEventsSystemEvents;
                ConsoleHelper.PrintSectionHeader($"SYSTEM EVENTS ({events.Length} events):");
                Console.WriteLine();
                foreach (var evt in events)
                {
                    Console.WriteLine($"  Event tersedia");
                    ConsoleHelper.PrintKeyValue("Type", evt.Type, indent: 4);
                    ConsoleHelper.PrintKeyValue("Message", evt.Message, indent: 4);
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintSectionHeader("SYSTEM EVENTS: Tidak ada data");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetSystemEvents!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetSystemEvents: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetSystemEvents operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetSystemEvents: {errorMessage}", faultEx);
        }
    }
}
