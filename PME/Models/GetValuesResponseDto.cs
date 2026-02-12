namespace PME.Models
{
    public class GetValuesResponseDto
    {
        public List<ValueDto> Values { get; set; } = new();
        public List<string> ErrorResults { get; set; } = new();
        public string? ResponseVersion { get; set; }
    }

    public class ValueDto
    {
        public string? Id { get; set; }
        public string? Value { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Quality { get; set; }
    }
}
