using PME.Helpers;

namespace PME.Services;

public class ForceValuesService
{
    private readonly DataExchangeService _dataExchangeService;

    public ForceValuesService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("ForceValues");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.ForceValuesRequest
            {
                version = version
            };

            var soapResponse = await client.ForceValuesAsync(new wsdl.ForceValuesRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.ForceValuesResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.ForceValuesResponse.version}");
                Console.WriteLine();
            }

            ConsoleHelper.PrintSuccess("Berhasil memanggil ForceValues!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute ForceValues: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  ForceValues operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil ForceValues: {errorMessage}", faultEx);
        }
    }
}
