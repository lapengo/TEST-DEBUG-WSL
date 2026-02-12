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

                // If no container ID provided, use root container ID
                if (string.IsNullOrEmpty(containerId))
                {
                    Console.WriteLine("⚠️  Tidak ada Container ID yang diberikan.");
                    Console.WriteLine("Mencoba menggunakan root container ID: \"/\"");
                    Console.WriteLine();
                    containerId = "/";
                }

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
                else if (errorMessage.Contains("MISSING_ID_LIST") ||
                         faultEx.Code?.Name == "MISSING_ID_LIST" ||
                         faultEx.Reason?.ToString().Contains("MISSING_ID_LIST") == true)
                {
                    ConsoleHelper.PrintSectionHeader("INFORMASI");
                    Console.WriteLine();
                    Console.WriteLine("⚠️  GetContainerItems memerlukan ID Container yang valid.");
                    Console.WriteLine();
                    Console.WriteLine("Cara mendapatkan Container IDs:");
                    Console.WriteLine("  1. Gunakan GetItems untuk melihat semua items dan containers");
                    Console.WriteLine("  2. Pilih Container ID dari hasil GetItems");
                    Console.WriteLine("  3. Jalankan GetContainerItems dengan Container ID tersebut");
                    Console.WriteLine();
                    Console.WriteLine("Contoh Container IDs yang mungkin:");
                    Console.WriteLine("  • \"/\" - Root container");
                    Console.WriteLine("  • \"System\" - System container");
                    Console.WriteLine("  • Container ID spesifik dari GetItems");
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
                    GetContainerItemsIds = string.IsNullOrEmpty(request.ContainerId) ? null : new[] { request.ContainerId },
                    version = request.Version,
                    metadata = false
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
                        // Check if it has sub-items (is a container)
                        bool isContainer = item.Items != null && 
                            (item.Items.ContainerItems?.Length > 0 ||
                             item.Items.ValueItems?.Length > 0 ||
                             item.Items.HistoryItems?.Length > 0 ||
                             item.Items.AlarmItems?.Length > 0);

                        response.Items.Add(new ContainerItemDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Type = item.Type,
                            IsContainer = isContainer
                        });
                    }
                }

                // Map Error Results
                if (soapResponse.GetContainerItemsResponse.GetContainerItemsErrorResults != null)
                {
                    foreach (var error in soapResponse.GetContainerItemsResponse.GetContainerItemsErrorResults)
                    {
                        response.ErrorResults.Add($"{error.Id}: {error.Message}");
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
