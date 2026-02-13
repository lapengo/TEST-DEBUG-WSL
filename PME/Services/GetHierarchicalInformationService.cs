using PME.Helpers;

namespace PME.Services;

public class GetHierarchicalInformationService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetHierarchicalInformationService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetHierarchicalInformation");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetHierarchicalInformationRequest
            {
                version = version
            };

            var soapResponse = await client.GetHierarchicalInformationAsync(new wsdl.GetHierarchicalInformationRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetHierarchicalInformationResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetHierarchicalInformationResponse.version}");
                Console.WriteLine();
            }

            // Display hierarchical info if available
            Console.WriteLine("GetHierarchicalInformation operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetHierarchicalInformation!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetHierarchicalInformation: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetHierarchicalInformation operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetHierarchicalInformation: {errorMessage}", faultEx);
        }
    }
}
