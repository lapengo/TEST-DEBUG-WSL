using PME.Helpers;

namespace PME.Services;

public class GetHistoryService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetHistoryService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetHistory");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetHistoryRequest
            {
                version = version
            };

            var soapResponse = await client.GetHistoryAsync(new wsdl.GetHistoryRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetHistoryResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetHistoryResponse.version}");
                Console.WriteLine();
            }

            // Display history data if available
            Console.WriteLine("GetHistory operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetHistory!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetHistory: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetHistory operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetHistory: {errorMessage}", faultEx);
        }
    }
}
