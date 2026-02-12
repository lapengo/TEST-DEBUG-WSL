namespace PME.Models;

/// <summary>
/// Request DTO untuk GetEnums
/// </summary>
public class GetEnumsRequestDto
{
    /// <summary>
    /// List of enum IDs to retrieve (optional)
    /// </summary>
    public List<string>? EnumIds { get; set; }

    /// <summary>
    /// Versi API yang diminta (opsional)
    /// </summary>
    public string? Version { get; set; }
}
