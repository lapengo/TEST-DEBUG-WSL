namespace PME.Models;

/// <summary>
/// Response DTO untuk GetEnums
/// </summary>
public class GetEnumsResponseDto
{
    /// <summary>
    /// List of enums dengan values
    /// </summary>
    public List<EnumDto> Enums { get; set; } = new List<EnumDto>();

    /// <summary>
    /// Error results jika ada
    /// </summary>
    public List<string> ErrorResults { get; set; } = new List<string>();

    /// <summary>
    /// Versi response dari service
    /// </summary>
    public string? ResponseVersion { get; set; }
}

/// <summary>
/// DTO untuk single enum dengan values
/// </summary>
public class EnumDto
{
    /// <summary>
    /// Enum ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Enum name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// List of enum values (value-text pairs)
    /// </summary>
    public List<EnumValueDto> Values { get; set; } = new List<EnumValueDto>();
}

/// <summary>
/// DTO untuk enum value (value-text pair)
/// </summary>
public class EnumValueDto
{
    /// <summary>
    /// Enum value
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Enum text/display name
    /// </summary>
    public string? Text { get; set; }
}
