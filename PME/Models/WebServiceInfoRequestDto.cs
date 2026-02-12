namespace PME.Models;

/// <summary>
/// DTO untuk request GetWebServiceInformation
/// </summary>
public class WebServiceInfoRequestDto
{
    /// <summary>
    /// Versi API yang diminta (opsional)
    /// </summary>
    public string? Version { get; set; }
}
