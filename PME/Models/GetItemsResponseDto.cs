namespace PME.Models
{
    public class GetItemsResponseDto
    {
        public List<ValueItemDto> ValueItems { get; set; } = new();
        public List<HistoryItemDto> HistoryItems { get; set; } = new();
        public List<AlarmItemDto> AlarmItems { get; set; } = new();
        public List<string> ErrorResults { get; set; } = new();
        public string? ResponseVersion { get; set; }
    }

    public class ValueItemDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public string? Unit { get; set; }
        public string? Writeable { get; set; }
        public string? State { get; set; }
    }

    public class HistoryItemDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Unit { get; set; }
    }

    public class AlarmItemDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
