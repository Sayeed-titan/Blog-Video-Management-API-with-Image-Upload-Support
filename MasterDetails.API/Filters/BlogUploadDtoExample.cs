using MasterDetails.API.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MasterDetails.API.Filters
{
    public class BlogUploadDtoExample : IExamplesProvider<BlogUploadDto>
    {
        public BlogUploadDto GetExamples()
        {
            return new BlogUploadDto
            {
                BlogID = null,
                Title = "Exploring ASP.NET Core",
                Content = "This blog explores advanced features of ASP.NET Core.",
                AuthorName = "Jane Doe",
                TagNames = new List<string> { "ASP.NET", "WebAPI", "C#" },
                IsPublished = true,
                BlogVideos = new List<BlogVideoDto>
            {
                new BlogVideoDto
                {
                    VideoUrl = "https://youtu.be/intro-to-aspnetcore",
                    Caption = "Introduction Video",
                    DisplayOrder = 1
                },
                new BlogVideoDto
                {
                    VideoUrl = "https://youtu.be/deep-dive-aspnetcore",
                    Caption = "Deep Dive",
                    DisplayOrder = 2
                }
            },
                CoverImage = null // Swagger can’t simulate file upload, but still include the key
            };
        }
    }
}
