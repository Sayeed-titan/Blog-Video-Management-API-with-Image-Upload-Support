namespace MasterDetails.API.DTOs
{
    public class BlogUploadDto
    {
        public int? BlogID { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public List<string> TagNames { get; set; } = new();

        public bool IsPublished { get; set; }

        public string BlogVideos { get; set; } = string.Empty;
        public IFormFile? CoverImage { get; set; }
    }


}
