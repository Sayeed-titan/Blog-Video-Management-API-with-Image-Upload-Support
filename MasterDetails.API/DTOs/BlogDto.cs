namespace MasterDetails.API.DTOs
{
    public class BlogDto
    {
        public int? BlogID { get; set; }  // nullable for new inserts
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }

        public List<BlogVideoDto> BlogVideos { get; set; } = new();
    }
}
