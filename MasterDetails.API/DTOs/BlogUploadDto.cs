namespace MasterDetails.API.DTOs
{
    public class BlogUploadDto
    {
        public int? BlogID { get; set; } // For PUT
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Tags { get; set; }
        public bool IsPublished { get; set; }

        public List<BlogVideoDto> BlogVideos { get; set; } = new();

        public IFormFile? CoverImage { get; set; } // file
    }

}
