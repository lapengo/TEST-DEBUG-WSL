using PME.Models;
using PME.Helpers;

namespace PME.Services
{
    public class GetValuesService
    {
        private readonly DataExchangeService _dataExchangeService;

        public GetValuesService(DataExchangeService dataExchangeService)
        {
            _dataExchangeService = dataExchangeService;
        }

        public async Task ExecuteAsync(string version, List<string> itemIds)
        {
            try
            {
                ConsoleHelper.PrintHeader("GetValues");

                var request = new GetValuesRequestDto
                {
                    ItemIds = itemIds,
                    Version = version
                };

                var response = await GetValuesAsync(request);

                DisplayHelper.DisplayValues(response);
            }
            catch (System.ServiceModel.FaultException faultEx)
            {
                string errorMessage = faultEx.Message;

                if (errorMessage.Contains("OPERATION_NOT_SUPPORTED") ||
                    faultEx.Code?.Name == "OPERATION_NOT_SUPPORTED" ||
                    faultEx.Reason?.ToString().Contains("OPERATION_NOT_SUPPORTED") == true)
                {
                    ConsoleHelper.PrintSectionHeader("INFORMASI");
                    Console.WriteLine();
                    Console.WriteLine("⚠️  GetValues operation TIDAK DIDUKUNG oleh PME server ini.");
                    Console.WriteLine();
                    Console.WriteLine("Silakan gunakan GetWebServiceInformation untuk melihat operasi yang tersedia.");
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception($"Error saat memanggil GetValues: {errorMessage}", faultEx);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saat execute GetValues: {ex.Message}", ex);
            }
        }

        private async Task<GetValuesResponseDto> GetValuesAsync(GetValuesRequestDto request)
        {
            try
            {
                var client = _dataExchangeService.GetClient();

                var soapRequest = new wsdl.GetValuesRequest
                {
                    GetValuesIds = request.ItemIds?.ToArray(),
                    version = request.Version
                };

                var soapResponse = await client.GetValuesAsync(new wsdl.GetValuesRequest1(soapRequest));

                var response = new GetValuesResponseDto
                {
                    ResponseVersion = soapResponse.GetValuesResponse.version
                };

                // Map Values
                if (soapResponse.GetValuesResponse.GetValuesValues != null)
                {
                    foreach (var value in soapResponse.GetValuesResponse.GetValuesValues)
                    {
                        response.Values.Add(new ValueDto
                        {
                            Id = value.Id,
                            Value = value.Value,
                            Timestamp = value.Timestamp,
                            Quality = value.Quality
                        });
                    }
                }

                // Map Error Results
                if (soapResponse.GetValuesResponse.GetValuesErrorResults != null)
                {
                    foreach (var error in soapResponse.GetValuesResponse.GetValuesErrorResults)
                    {
                        response.ErrorResults.Add($"{error.Id}: {error.Error}");
                    }
                }

                return response;
            }
            catch (System.ServiceModel.FaultException faultEx)
            {
                throw new Exception($"Error saat memanggil GetValues: {faultEx.Message}", faultEx);
            }
        }
    }
}
