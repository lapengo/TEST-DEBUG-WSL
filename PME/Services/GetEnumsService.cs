using PME.Models;
using PME.Helpers;

namespace PME.Services;

/// <summary>
/// Service class untuk handle GetEnums logic
/// </summary>
public class GetEnumsService
{
    private readonly DataExchangeService _dataExchangeService;

    public GetEnumsService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    /// <summary>
    /// Execute GetEnums dan display hasilnya
    /// </summary>
    public async Task ExecuteAsync(string? version = null, List<string>? enumIds = null)
    {
        try
        {
            // Buat request
            var request = new GetEnumsRequestDto
            {
                Version = version,
                EnumIds = enumIds
            };

            Console.WriteLine("Memanggil GetEnums...");
            
            // Call service
            var response = await GetEnumsAsync(request);

            Console.WriteLine();
            
            // Display hasil
            DisplayHelper.DisplayEnums(response);

            ConsoleHelper.PrintSuccess("Berhasil mendapatkan enums!");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetEnums: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get enums dari SOAP service
    /// </summary>
    private async Task<GetEnumsResponseDto> GetEnumsAsync(GetEnumsRequestDto? request = null)
    {
        try
        {
            // Buat SOAP request
            var soapRequest = new wsdl.GetEnumsRequest1
            {
                GetEnumsRequest = new wsdl.GetEnumsRequest
                {
                    version = request?.Version,
                    GetEnumIds = request?.EnumIds?.ToArray()
                }
            };

            // Call SOAP service
            var soapResponse = await _dataExchangeService.GetClient().GetEnumsAsync(soapRequest);

            // Map ke DTO
            var dto = new GetEnumsResponseDto
            {
                ResponseVersion = soapResponse.GetEnumsResponse.version
            };

            // Map enums
            if (soapResponse.GetEnumsResponse.GetEnumsEnums != null)
            {
                foreach (var enumType in soapResponse.GetEnumsResponse.GetEnumsEnums)
                {
                    var enumDto = new EnumDto
                    {
                        Id = enumType.Id,
                        Name = enumType.Name
                    };

                    // Map enum values
                    if (enumType.EnumValues != null)
                    {
                        foreach (var enumValue in enumType.EnumValues)
                        {
                            enumDto.Values.Add(new EnumValueDto
                            {
                                Value = enumValue.Value,
                                Text = enumValue.Text
                            });
                        }
                    }

                    dto.Enums.Add(enumDto);
                }
            }

            // Map error results if any
            if (soapResponse.GetEnumsResponse.GetEnumsErrorResults != null)
            {
                foreach (var error in soapResponse.GetEnumsResponse.GetEnumsErrorResults)
                {
                    dto.ErrorResults.Add($"Error: {error.Message} (ID: {error.Id})");
                }
            }

            return dto;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat memanggil GetEnums: {ex.Message}", ex);
        }
    }
}
