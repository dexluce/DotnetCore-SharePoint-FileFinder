namespace Org.OpenAPITools.Models
{
    public class Metadata
    {
        public required string Name { get; set; }
        public string? Title { get; set; }
        public List<CategoryEnum> Categories { get; set; } = [];
        public DateTime PublishDate { get; set; }
        public bool Approved { get; set; } = false;
        public bool HcpOnly { get; set; } = false;
        public List<LanguageEnum> Languages { get; set; } = [];
        public int? OrderNr { get; set; }
        public DateTime Modified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
