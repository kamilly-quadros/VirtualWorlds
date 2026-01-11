namespace VirtualWorlds.Server.Models
{
    public class Book
    {
        public int CdBook { get; set; }
        public string NmBook { get; set; } = string.Empty;
        public double NrPriceBook { get; set; }
        public int CdSpecification { get; set; }
        public required Specification Specification { get; set; }
    }
}
