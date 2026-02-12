using PME.Helpers;

namespace PME.Services;

public class GetAlarmEventsService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetAlarmEventsService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetAlarmEvents");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetAlarmEventsRequest
            {
                version = version
            };

            var soapResponse = await client.GetAlarmEventsAsync(new wsdl.GetAlarmEventsRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetAlarmEventsResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetAlarmEventsResponse.version}");
                Console.WriteLine();
            }

            // Display alarm events if available
            if (soapResponse.GetAlarmEventsResponse.GetAlarmEventsAlarmEvents != null)
            {
                var events = soapResponse.GetAlarmEventsResponse.GetAlarmEventsAlarmEvents;
                ConsoleHelper.PrintSectionHeader($"ALARM EVENTS ({events.Length} events):");
                Console.WriteLine();

                foreach (var evt in events)
                {
                    Console.WriteLine($"  Event tersedia");
                    ConsoleHelper.PrintKeyValue("Type", evt.Type, indent: 4);
                    ConsoleHelper.PrintKeyValue("Message", evt.Message, indent: 4);
                    ConsoleHelper.PrintKeyValue("State", evt.State, indent: 4);
                    Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.PrintSectionHeader("ALARM EVENTS: Tidak ada data");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetAlarmEvents!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetAlarmEvents: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetAlarmEvents operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetAlarmEvents: {errorMessage}", faultEx);
        }
    }
}
