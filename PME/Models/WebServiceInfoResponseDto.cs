namespace PME.Models;

/// <summary>
/// DTO untuk response GetWebServiceInformation
/// </summary>
public class WebServiceInfoResponseDto
{
    /// <summary>
    /// Informasi versi web service
    /// </summary>
    public VersionInfo? Version { get; set; }

    /// <summary>
    /// Daftar operasi yang didukung oleh web service
    /// </summary>
    public List<string> SupportedOperations { get; set; } = new();

    /// <summary>
    /// Daftar profil yang didukung oleh web service
    /// </summary>
    public List<string> SupportedProfiles { get; set; } = new();

    /// <summary>
    /// Informasi sistem
    /// </summary>
    public SystemInfo? System { get; set; }

    /// <summary>
    /// Versi response
    /// </summary>
    public string? ResponseVersion { get; set; }
}

/// <summary>
/// Informasi versi web service
/// </summary>
public class VersionInfo
{
    /// <summary>
    /// Versi major
    /// </summary>
    public string? MajorVersion { get; set; }

    /// <summary>
    /// Versi minor
    /// </summary>
    public string? MinorVersion { get; set; }
}

/// <summary>
/// Informasi sistem
/// </summary>
public class SystemInfo
{
    /// <summary>
    /// Versi sistem
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// ID sistem
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Nama sistem
    /// </summary>
    public string? Name { get; set; }
}
