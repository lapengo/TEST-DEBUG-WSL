namespace PME.Models;

/// <summary>
/// Request DTO untuk GetAlarmEventTypes
/// </summary>
public class AlarmEventTypesRequestDto
{
    /// <summary>
    /// Versi API yang diminta (opsional)
    /// </summary>
    public string? Version { get; set; }
}
