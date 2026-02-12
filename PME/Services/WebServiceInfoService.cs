using PME.Models;
using PME.Helpers;

namespace PME.Services;

/// <summary>
/// Service class untuk handle GetWebServiceInformation logic
/// </summary>
public class WebServiceInfoService
{
    private readonly DataExchangeService _dataExchangeService;

    public WebServiceInfoService(DataExchangeService dataExchangeService)
    {
        _dataExchangeService = dataExchangeService;
    }

    /// <summary>
    /// Execute GetWebServiceInformation dan display hasilnya
    /// </summary>
    public async Task ExecuteAsync(string version)
    {
        try
        {
            // Buat request
            var request = new WebServiceInfoRequestDto
            {
                Version = version
            };

            Console.WriteLine("Memanggil GetWebServiceInformation...");
            
            // Call service
            var response = await _dataExchangeService.GetWebServiceInformationAsync(request);

            Console.WriteLine();
            
            // Display hasil
            DisplayHelper.DisplayWebServiceInfo(response);

            ConsoleHelper.PrintSuccess("Berhasil mendapatkan informasi web service!");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat execute GetWebServiceInformation: {ex.Message}", ex);
        }
    }
}
