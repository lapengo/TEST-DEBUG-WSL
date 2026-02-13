using PME.Helpers;

namespace PME.Services;

public class GetHistoricalDataAggregationService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetHistoricalDataAggregationService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    public async Task ExecuteAsync(string version)
    {
        try
        {
            ConsoleHelper.PrintHeader("GetHistoricalDataAggregation");

            var client = _dataExchangeService.GetClient();

            var soapRequest = new wsdl.GetHistoricalDataAggregationRequest
            {
                version = version
            };

            var soapResponse = await client.GetHistoricalDataAggregationAsync(new wsdl.GetHistoricalDataAggregationRequest1(soapRequest));

            ConsoleHelper.PrintSeparator();
            Console.WriteLine("HASIL RESPONSE:");
            ConsoleHelper.PrintSeparator();
            Console.WriteLine();

            if (!string.IsNullOrEmpty(soapResponse.GetHistoricalDataAggregationResponse.version))
            {
                Console.WriteLine($"Response Version: {soapResponse.GetHistoricalDataAggregationResponse.version}");
                Console.WriteLine();
            }

            // Display aggregation data if available
            Console.WriteLine("GetHistoricalDataAggregation operation dipanggil dengan sukses");
            Console.WriteLine();

            ConsoleHelper.PrintSuccess("Berhasil memanggil GetHistoricalDataAggregation!");
        }
        catch (System.ServiceModel.FaultException faultEx)
        {
            HandleFaultException(faultEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetHistoricalDataAggregation: {ex.Message}", ex);
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
            Console.WriteLine("⚠️  GetHistoricalDataAggregation operation TIDAK DIDUKUNG oleh PME server ini.");
            Console.WriteLine();
        }
        else
        {
            throw new Exception($"Error saat memanggil GetHistoricalDataAggregation: {errorMessage}", faultEx);
        }
    }
}
