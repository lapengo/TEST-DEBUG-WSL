using PME.Models;
using PME.Helpers;

namespace PME.Services
{
    public class GetItemsService
    {
        private readonly DataExchangeService _dataExchangeService;

        public GetItemsService(DataExchangeService dataExchangeService)
        {
            _dataExchangeService = dataExchangeService;
        }

        public async Task ExecuteAsync(string version, List<string>? itemIds = null, bool includeMetadata = false)
        {
            try
            {
                ConsoleHelper.PrintHeader("GetItems");

                var request = new GetItemsRequestDto
                {
                    ItemIds = itemIds,
                    Version = version,
                    IncludeMetadata = includeMetadata
                };

                var response = await GetItemsAsync(request);

                DisplayHelper.DisplayItems(response);
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
                    Console.WriteLine("⚠️  GetItems operation TIDAK DIDUKUNG oleh PME server ini.");
                    Console.WriteLine();
                    Console.WriteLine("Operasi GetItems tersedia di WSDL tetapi tidak diaktifkan di server.");
                    Console.WriteLine("Kemungkinan penyebab:");
                    Console.WriteLine("  • PME server version tidak mendukung GetItems");
                    Console.WriteLine("  • Feature belum diaktifkan di konfigurasi server");
                    Console.WriteLine("  • License tidak mencakup feature ini");
                    Console.WriteLine();
                    Console.WriteLine("Silakan gunakan operasi lain yang didukung:");
                    Console.WriteLine("  1. GetWebServiceInformation - untuk melihat operasi yang tersedia");
                    Console.WriteLine("  2. GetContainerItems - untuk melihat struktur container");
                    Console.WriteLine("  3. GetAlarmEventTypes");
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception($"Error saat memanggil GetItems: {errorMessage}", faultEx);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saat execute GetItems: {ex.Message}", ex);
            }
        }

        private async Task<GetItemsResponseDto> GetItemsAsync(GetItemsRequestDto request)
        {
            try
            {
                var client = _dataExchangeService.GetClient();

                var soapRequest = new wsdl.GetItemsRequest
                {
                    GetItemsIds = request.ItemIds?.ToArray(),
                    version = request.Version,
                    metadata = request.IncludeMetadata
                };

                var soapResponse = await client.GetItemsAsync(new wsdl.GetItemsRequest1(soapRequest));

                var response = new GetItemsResponseDto
                {
                    ResponseVersion = soapResponse.GetItemsResponse.version
                };

                // Map Value Items
                if (soapResponse.GetItemsResponse.GetItemsItems?.ValueItems != null)
                {
                    foreach (var item in soapResponse.GetItemsResponse.GetItemsItems.ValueItems)
                    {
                        response.ValueItems.Add(new ValueItemDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Type = item.Type,
                            Value = item.Value,
                            Unit = item.Unit,
                            Writeable = item.Writeable,
                            State = item.State
                        });
                    }
                }

                // Map History Items
                if (soapResponse.GetItemsResponse.GetItemsItems?.HistoryItems != null)
                {
                    foreach (var item in soapResponse.GetItemsResponse.GetItemsItems.HistoryItems)
                    {
                        response.HistoryItems.Add(new HistoryItemDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Type = item.Type,
                            Unit = item.Unit
                        });
                    }
                }

                // Map Alarm Items
                if (soapResponse.GetItemsResponse.GetItemsItems?.AlarmItems != null)
                {
                    foreach (var item in soapResponse.GetItemsResponse.GetItemsItems.AlarmItems)
                    {
                        response.AlarmItems.Add(new AlarmItemDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        });
                    }
                }

                // Map Error Results
                if (soapResponse.GetItemsResponse.GetItemsErrorResults != null)
                {
                    foreach (var error in soapResponse.GetItemsResponse.GetItemsErrorResults)
                    {
                        response.ErrorResults.Add($"{error.Id}: {error.Message}");
                    }
                }

                return response;
            }
            catch (System.ServiceModel.FaultException faultEx)
            {
                throw new Exception($"Error saat memanggil GetItems: {faultEx.Message}", faultEx);
            }
        }
    }
}
