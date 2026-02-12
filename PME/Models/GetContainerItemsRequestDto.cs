namespace PME.Models
{
    public class GetContainerItemsRequestDto
    {
        public string? ContainerId { get; set; }
        public string? Version { get; set; }
        public bool Recursive { get; set; }
    }
}
