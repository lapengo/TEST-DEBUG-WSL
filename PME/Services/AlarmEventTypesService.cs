using PME.Models;
using PME.Helpers;

namespace PME.Services;

/// <summary>
/// Service class untuk handle GetAlarmEventTypes logic
/// </summary>
public class AlarmEventTypesService
{
    private readonly DataExchangeService _dataExchangeService;

    public AlarmEventTypesService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    /// <summary>
    /// Execute GetAlarmEventTypes dan display hasilnya
    /// </summary>
    public async Task ExecuteAsync(string? version = null)
    {
        try
        {
            // Buat request
            var request = new AlarmEventTypesRequestDto
            {
                Version = version
            };

            Console.WriteLine("Memanggil GetAlarmEventTypes...");
            
            // Call service
            var response = await GetAlarmEventTypesAsync(request);

            Console.WriteLine();
            
            // Display hasil
            DisplayHelper.DisplayAlarmEventTypes(response);

            ConsoleHelper.PrintSuccess("Berhasil mendapatkan alarm event types!");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetAlarmEventTypes: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get alarm event types dari SOAP service
    /// </summary>
    private async Task<AlarmEventTypesResponseDto> GetAlarmEventTypesAsync(AlarmEventTypesRequestDto? request = null)
    {
        try
        {
            // Buat SOAP request
            var soapRequest = new wsdl.GetAlarmEventTypesRequest1
            {
                GetAlarmEventTypesRequest = new wsdl.GetAlarmEventTypesRequest
                {
                    version = request?.Version
                }
            };

            // Call SOAP service
            var soapResponse = await _dataExchangeService.GetClient().GetAlarmEventTypesAsync(soapRequest);

            // Map ke DTO
            var dto = new AlarmEventTypesResponseDto
            {
                ResponseVersion = soapResponse.GetAlarmEventTypesResponse.version,
                Types = soapResponse.GetAlarmEventTypesResponse.GetAlarmEventTypesTypes?.ToList() ?? new List<string>()
            };

            return dto;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat memanggil GetAlarmEventTypes: {ex.Message}", ex);
        }
    }
}
