using PME.Helpers;

namespace PME.Services;

public class GetUpdatedAlarmEventsService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetUpdatedAlarmEventsService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetUpdatedAlarmEvents");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetUpdatedAlarmEventsRequest
            {
                version = version
            };

            var soapResponse = await client.GetUpdatedAlarmEventsAsync(new wsdl.GetUpdatedAlarmEventsRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetUpdatedAlarmEventsResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetUpdatedAlarmEventsResponse.version}");
                Console.WriteLine();
            }

            // Display updated alarm events if available
            if (soapResponse.GetUpdatedAlarmEventsResponse.GetUpdatedAlarmEventsAlarmEvents != null)
            {
                var events = soapResponse.GetUpdatedAlarmEventsResponse.GetUpdatedAlarmEventsAlarmEvents;
                ConsoleHelper.PrintSectionHeader($"UPDATED ALARM EVENTS ({events.Length} events):");
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
                ConsoleHelper.PrintSectionHeader("UPDATED ALARM EVENTS: Tidak ada data");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetUpdatedAlarmEvents!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetUpdatedAlarmEvents: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetUpdatedAlarmEvents operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetUpdatedAlarmEvents: {errorMessage}", faultEx);
        }
    }
}
