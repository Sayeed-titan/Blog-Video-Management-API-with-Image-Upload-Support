namespace MasterDetails.API.DTOs
{
    public class BlogVideoDto
    {
        public int? BlogVideoID { get; set; }  // nullable for new inserts
        public string VideoUrl { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
