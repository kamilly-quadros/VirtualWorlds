namespace VirtualWorlds.Server.DTOs
{
    public class BookJsonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public SpecificationJsonDto Specifications { get; set; } = null!;
    }
}
