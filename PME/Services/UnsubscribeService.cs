using PME.Helpers;

namespace PME.Services;

public class UnsubscribeService
{
    private readonly DataExchangeService _dataExchangeService;

    public UnsubscribeService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("Unsubscribe");

            var client = _dataExchangeService.GetClient();

            var unsubscribe = new wsdl.Unsubscribe();
            var soapRequest = new wsdl.UnsubscribeRequest(unsubscribe);

            var soapResponse = await client.UnsubscribeAsync(soapRequest);

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            Console.WriteLine("Unsubscribe operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil Unsubscribe!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute Unsubscribe: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  Unsubscribe operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil Unsubscribe: {errorMessage}", faultEx);
        }
    }
}
