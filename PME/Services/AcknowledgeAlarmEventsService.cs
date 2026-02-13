using PME.Helpers;

namespace PME.Services;

public class AcknowledgeAlarmEventsService
{
    private readonly DataExchangeService _dataExchangeService;

    public AcknowledgeAlarmEventsService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("AcknowledgeAlarmEvents");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.AcknowledgeAlarmEventsRequest
            {
                version = version
            };

            var soapResponse = await client.AcknowledgeAlarmEventsAsync(new wsdl.AcknowledgeAlarmEventsRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.AcknowledgeAlarmEventsResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.AcknowledgeAlarmEventsResponse.version}");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil AcknowledgeAlarmEvents!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute AcknowledgeAlarmEvents: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  AcknowledgeAlarmEvents operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil AcknowledgeAlarmEvents: {errorMessage}", faultEx);
        }
    }
}
