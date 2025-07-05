namespace MasterDetails.API.DTOs
{
    public class BlogDto
    {
        public int BlogID { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public List<string> TagNames { get; set; } = new();

        public string CoverImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }

        public List<BlogVideoDto> BlogVideos { get; set; } = new();
    }

}
