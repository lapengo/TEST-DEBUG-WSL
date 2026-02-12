using PME.Helpers;

namespace PME.Services;

public class GetNotificationService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetNotificationService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetNotification");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetNotificationRequest
            {
                version = version
            };

            var soapResponse = await client.GetNotificationAsync(new wsdl.GetNotificationRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetNotificationResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetNotificationResponse.version}");
                Console.WriteLine();
            }

            // Display notification data if available
            Console.WriteLine("GetNotification operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetNotification!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetNotification: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetNotification operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetNotification: {errorMessage}", faultEx);
        }
    }
}
