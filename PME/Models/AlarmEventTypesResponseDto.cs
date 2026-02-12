namespace PME.Models;

/// <summary>
/// Response DTO untuk GetAlarmEventTypes
/// </summary>
public class AlarmEventTypesResponseDto
{
    /// <summary>
    /// List alarm event types yang didukung
    /// </summary>
    public List<string> Types { get; set; } = new List<string>();

    /// <summary>
    /// Versi response dari service
    /// </summary>
    public string? ResponseVersion { get; set; }
}
