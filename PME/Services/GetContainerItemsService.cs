using PME.Models;
using PME.Helpers;

namespace PME.Services
{
    public class GetContainerItemsService
    {
        private readonly DataExchangeService _dataExchangeService;

        public GetContainerItemsService(DataExchangeService dataExchangeService)
        {
            _dataExchangeService = dataExchangeService;
        }

        public async Task ExecuteAsync(string version, string? containerId = null, bool recursive = false)
        {
            try
            {
                ConsoleHelper.PrintHeader("GetContainerItems");

                var request = new GetContainerItemsRequestDto
                {
                    ContainerId = containerId,
                    Version = version,
                    Recursive = recursive
                };

                var response = await GetContainerItemsAsync(request);

                DisplayHelper.DisplayContainerItems(response);
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
                    Console.WriteLine("⚠️  GetContainerItems operation TIDAK DIDUKUNG oleh PME server ini.");
                    Console.WriteLine();
                    Console.WriteLine("Silakan gunakan GetWebServiceInformation untuk melihat operasi yang tersedia.");
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception($"Error saat memanggil GetContainerItems: {errorMessage}", faultEx);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saat execute GetContainerItems: {ex.Message}", ex);
            }
        }

        private async Task<GetContainerItemsResponseDto> GetContainerItemsAsync(GetContainerItemsRequestDto request)
        {
            try
            {
                var client = _dataExchangeService.GetClient();

                var soapRequest = new wsdl.GetContainerItemsRequest
                {
                    GetContainerItemsId = request.ContainerId,
                    version = request.Version,
                    Recursive = request.Recursive
                };

                var soapResponse = await client.GetContainerItemsAsync(new wsdl.GetContainerItemsRequest1(soapRequest));

                var response = new GetContainerItemsResponseDto
                {
                    ResponseVersion = soapResponse.GetContainerItemsResponse.version
                };

                // Map Container Items
                if (soapResponse.GetContainerItemsResponse.GetContainerItemsItems != null)
                {
                    foreach (var item in soapResponse.GetContainerItemsResponse.GetContainerItemsItems)
                    {
                        response.Items.Add(new ContainerItemDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Type = item.Type,
                            IsContainer = item.IsContainer
                        });
                    }
                }

                // Map Error Results
                if (soapResponse.GetContainerItemsResponse.GetContainerItemsErrorResults != null)
                {
                    foreach (var error in soapResponse.GetContainerItemsResponse.GetContainerItemsErrorResults)
                    {
                        response.ErrorResults.Add($"{error.Id}: {error.Error}");
                    }
                }

                return response;
            }
            catch (System.ServiceModel.FaultException faultEx)
            {
                throw new Exception($"Error saat memanggil GetContainerItems: {faultEx.Message}", faultEx);
            }
        }
    }
}
