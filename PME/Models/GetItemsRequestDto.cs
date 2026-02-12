namespace PME.Models
{
    public class GetItemsRequestDto
    {
        public List<string>? ItemIds { get; set; }
        public string? Version { get; set; }
        public bool IncludeMetadata { get; set; }
    }
}
