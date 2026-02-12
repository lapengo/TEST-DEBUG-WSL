using PME.Helpers;

namespace PME.Services;

public class GetAlarmHistoryService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetAlarmHistoryService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetAlarmHistory");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetAlarmHistoryRequest
            {
                version = version
            };

            var soapResponse = await client.GetAlarmHistoryAsync(new wsdl.GetAlarmHistoryRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetAlarmHistoryResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetAlarmHistoryResponse.version}");
                Console.WriteLine();
            }

            // Display alarm history if available
            Console.WriteLine("GetAlarmHistory operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetAlarmHistory!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetAlarmHistory: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetAlarmHistory operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetAlarmHistory: {errorMessage}", faultEx);
        }
    }
}
