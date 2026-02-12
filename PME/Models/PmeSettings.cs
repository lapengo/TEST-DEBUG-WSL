namespace PME.Models;

/// <summary>
/// Model untuk konfigurasi PME DataExchange dari appsettings.json
/// </summary>
public class PmeSettings
{
    /// <summary>
    /// URL endpoint SOAP service
    /// </summary>
    public string ServiceUrl { get; set; } = string.Empty;

    /// <summary>
    /// Username untuk autentikasi
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password untuk autentikasi
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Versi API yang diminta
    /// </summary>
    public string Version { get; set; } = string.Empty;
}
