namespace PME.Models
{
    public class GetContainerItemsResponseDto
    {
        public List<ContainerItemDto> Items { get; set; } = new();
        public List<string> ErrorResults { get; set; } = new();
        public string? ResponseVersion { get; set; }
    }

    public class ContainerItemDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public bool IsContainer { get; set; }
    }
}
