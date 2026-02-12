using PME.Models;
using wsdl;

namespace PME.Services;

/// <summary>
/// Service untuk komunikasi dengan DataExchange SOAP Web Service
/// </summary>
public class DataExchangeService : IDisposable
{
    private readonly DataExchangeClient _client;
    private bool _disposed = false;

    /// <summary>
    /// Konstruktor untuk DataExchangeService
    /// </summary>
    /// <param name="serviceUrl">URL endpoint SOAP service</param>
    public DataExchangeService(string serviceUrl)
    {
        _client = new DataExchangeClient(
            DataExchangeClient.EndpointConfiguration.CustomBinding_IDataExchange,
            serviceUrl
        );
    }

    /// <summary>
    /// Mendapatkan informasi web service dari SOAP endpoint
    /// </summary>
    /// <param name="request">Request DTO berisi versi (opsional)</param>
    /// <returns>Response DTO berisi informasi web service</returns>
    public async Task<WebServiceInfoResponseDto> GetWebServiceInformationAsync(WebServiceInfoRequestDto? request = null)
    {
        try
        {
            // Buat request SOAP
            var soapRequest = new GetWebServiceInformationRequest1
            {
                GetWebServiceInformationRequest = new GetWebServiceInformationRequest
                {
                    version = request?.Version
                }
            };

            // Panggil SOAP service
            var soapResponse = await _client.GetWebServiceInformationAsync(soapRequest);
            
            // Map response SOAP ke DTO
            var responseDto = MapToDto(soapResponse);
            
            return responseDto;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saat memanggil GetWebServiceInformation: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Mapping dari SOAP response ke DTO
    /// </summary>
    private WebServiceInfoResponseDto MapToDto(GetWebServiceInformationResponse1 soapResponse)
    {
        var response = soapResponse.GetWebServiceInformationResponse;
        
        var dto = new WebServiceInfoResponseDto
        {
            ResponseVersion = response.version,
            SupportedOperations = response.GetWebServiceInformationSupportedOperations?.ToList() ?? new List<string>(),
            SupportedProfiles = response.GetWebServiceInformationSupportedProfiles?.ToList() ?? new List<string>()
        };

        // Map version info
        if (response.GetWebServiceInformationVersion != null)
        {
            dto.Version = new VersionInfo
            {
                MajorVersion = response.GetWebServiceInformationVersion.MajorVersion,
                MinorVersion = response.GetWebServiceInformationVersion.MinorVersion
            };
        }

        // Map system info
        if (response.SystemInfo != null)
        {
            dto.System = new SystemInfo
            {
                Version = response.SystemInfo.Version,
                Id = response.SystemInfo.Id,
                Name = response.SystemInfo.Name
            };
        }

        return dto;
    }

    /// <summary>
    /// Dispose resources
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    try
                    {
                        _client.Close();
                    }
                    catch
                    {
                        _client.Abort();
                    }
                }
            }
            _disposed = true;
        }
    }
}
