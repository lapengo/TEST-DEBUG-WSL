using PME.Helpers;

namespace PME.Services;

public class RenewService
{
    private readonly DataExchangeService _dataExchangeService;

    public RenewService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("Renew");

            var client = _dataExchangeService.GetClient();

            var renew = new wsdl.Renew();
            var soapRequest = new wsdl.RenewRequest(renew);

            var soapResponse = await client.RenewAsync(soapRequest);

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            Console.WriteLine("Renew operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil Renew!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute Renew: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  Renew operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil Renew: {errorMessage}", faultEx);
        }
    }
}
