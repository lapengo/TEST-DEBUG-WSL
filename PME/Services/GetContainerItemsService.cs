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
                ConsoleHelper.PrintHeader("GetContainerItems - Hierarchical Traversal");

                // Always start from root (ID = "0") for hierarchical traversal
                Console.WriteLine("🔍 Memulai traversal hierarki dari root container (ID: 0)...");
                Console.WriteLine();

                var rootItem = await FetchContainerHierarchyAsync(version, "0", 0);

                // Create response with hierarchical structure
                var response = new GetContainerItemsResponseDto
                {
                    ResponseVersion = version
                };

                if (rootItem != null)
                {
                    response.Items.Add(rootItem);
                }

                DisplayHelper.DisplayContainerItemsHierarchical(response);

                // Display summary
                Console.WriteLine();
                ConsoleHelper.PrintSeparator();
                int totalItems = CountTotalItems(rootItem);
                Console.WriteLine($"✓ Total items ditemukan: {totalItems}");
                ConsoleHelper.PrintSeparator();
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

        /// <summary>
        /// Recursively fetch container hierarchy
        /// </summary>
        private async Task<ContainerItemDto?> FetchContainerHierarchyAsync(string version, string containerId, int level)
        {
            try
            {
                Console.WriteLine($"{"  ".PadLeft(level * 2)}📂 Fetching container: {containerId} (level {level})");

                var request = new GetContainerItemsRequestDto
                {
                    ContainerId = containerId,
                    Version = version,
                    Recursive = false
                };

                var response = await GetContainerItemsAsync(request);

                // If no items returned, return null
                if (response.Items == null || !response.Items.Any())
                {
                    return null;
                }

                // Get the first item (should be the container itself)
                var containerItem = response.Items.First();
                containerItem.Level = level;

                // Now fetch children for this container
                // The response might contain child items directly, or we need to fetch them
                // According to the requirement, we need to extract IDs from ContainerItems and fetch each one
                
                var childContainerIds = new List<string>();
                
                // Check if there are child containers in the current response
                // We need to look at the actual SOAP response structure
                var soapResponse = await GetRawContainerItemsAsync(request);
                
                if (soapResponse?.GetContainerItemsResponse?.GetContainerItemsItems != null)
                {
                    foreach (var item in soapResponse.GetContainerItemsResponse.GetContainerItemsItems)
                    {
                        // Check if this item has child ContainerItems
                        if (item.Items?.ContainerItems != null && item.Items.ContainerItems.Length > 0)
                        {
                            foreach (var childContainer in item.Items.ContainerItems)
                            {
                                if (!string.IsNullOrEmpty(childContainer.Id))
                                {
                                    childContainerIds.Add(childContainer.Id);
                                }
                            }
                        }
                    }
                }

                // Recursively fetch each child container
                foreach (var childId in childContainerIds)
                {
                    var childItem = await FetchContainerHierarchyAsync(version, childId, level + 1);
                    if (childItem != null)
                    {
                        containerItem.Children.Add(childItem);
                    }
                }

                return containerItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{"  ".PadLeft(level * 2)}⚠️  Error fetching {containerId}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Count total items in hierarchy
        /// </summary>
        private int CountTotalItems(ContainerItemDto? item)
        {
            if (item == null) return 0;
            
            int count = 1;
            foreach (var child in item.Children)
            {
                count += CountTotalItems(child);
            }
            return count;
        }

        /// <summary>
        /// Get raw SOAP response to extract child container IDs
        /// </summary>
        private async Task<wsdl.GetContainerItemsResponse1?> GetRawContainerItemsAsync(GetContainerItemsRequestDto request)
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

                return await client.GetContainerItemsAsync(new wsdl.GetContainerItemsRequest1(soapRequest));
            }
            catch
            {
                return null;
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
