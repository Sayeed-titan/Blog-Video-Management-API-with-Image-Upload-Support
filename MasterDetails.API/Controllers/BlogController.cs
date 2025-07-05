using AutoMapper;
using MasterDetails.API.Data;
using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;

        public BlogController(BlogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/blog
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _context.Blogs
                .Include(b => b.BlogVideos)
                .ToListAsync();

            var dtoList = _mapper.Map<List<BlogDto>>(blogs);

            return Ok(dtoList);
        }

        // GET api/blog/{id} - get blog with videos
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null) return NotFound();

            var dto = _mapper.Map<BlogDto>(blog);
            return Ok(dto);
        }

        // POST api/blog - create new blog + videos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogDto dto)
        {
            var blog = _mapper.Map<Blog>(dto);

            blog.CreatedAt = DateTime.Now; // ensure CreatedAt is set

            // Make sure new videos have no IDs
            foreach (var video in blog.BlogVideos)
            {
                video.BlogVideoID = 0;
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<BlogDto>(blog);

            return CreatedAtAction(nameof(Get), new { id = blog.BlogID }, resultDto);
        }

        // PUT api/blog/{id} - update blog + videos
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogDto dto)
        {
            if (id != dto.BlogID)
                return BadRequest("Blog ID mismatch");

            var existingBlog = await _context.Blogs
                .Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (existingBlog == null) return NotFound();

            // Update blog scalar properties
            existingBlog.Title = dto.Title;
            existingBlog.Content = dto.Content;
            existingBlog.Author = dto.Author;
            existingBlog.CoverImageUrl = dto.CoverImageUrl;
            existingBlog.Tags = dto.Tags;
            existingBlog.IsPublished = dto.IsPublished;
            // CreatedAt usually not changed on update

            // Update videos master-detail:
            // 1. Delete removed videos
            var dtoVideoIds = dto.BlogVideos.Where(v => v.BlogVideoID.HasValue).Select(v => v.BlogVideoID!.Value).ToList();
            var videosToRemove = existingBlog.BlogVideos.Where(v => !dtoVideoIds.Contains(v.BlogVideoID)).ToList();
            _context.BlogVideos.RemoveRange(videosToRemove);

            // 2. Update existing and add new videos
            foreach (var videoDto in dto.BlogVideos)
            {
                if (videoDto.BlogVideoID.HasValue)
                {
                    // Update existing video
                    var existingVideo = existingBlog.BlogVideos.FirstOrDefault(v => v.BlogVideoID == videoDto.BlogVideoID.Value);
                    if (existingVideo != null)
                    {
                        existingVideo.VideoUrl = videoDto.VideoUrl;
                        existingVideo.Caption = videoDto.Caption;
                        existingVideo.DisplayOrder = videoDto.DisplayOrder;
                    }
                }
                else
                {
                    // New video
                    var newVideo = _mapper.Map<BlogVideo>(videoDto);
                    existingBlog.BlogVideos.Add(newVideo);
                }
            }

            await _context.SaveChangesAsync();
            var updatedDto = _mapper.Map<BlogDto>(existingBlog);
            return Ok(updatedDto); // returns 200 OK + updated resource JSON
        }

        // DELETE api/blog/{id} - delete blog + videos (cascade)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Blog deleted successfully" });
        }


        [HttpPost("upload")]
        public async Task<IActionResult> CreateWithImage([FromForm] BlogUploadDto dto)
        {
            string coverImageUrl = string.Empty;

            if (dto.CoverImage != null)
            {

                //  Check file type
                var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(dto.CoverImage.FileName).ToLower();

                if (!allowedTypes.Contains(extension))
                    return BadRequest("Only JPG, JPEG, PNG, or WEBP images are allowed.");

                //  Check file size (e.g., max 2MB)
                if (dto.CoverImage.Length > 2 * 1024 * 1024)
                    return BadRequest("Image size should not exceed 2MB.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.CoverImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CoverImage.CopyToAsync(stream);
                }

                // You can use a relative URL or full URL
                coverImageUrl = "/uploads/" + fileName;
            }

            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                Tags = dto.Tags,
                CoverImageUrl = coverImageUrl,
                CreatedAt = DateTime.Now,
                IsPublished = dto.IsPublished,
                BlogVideos = dto.BlogVideos.Select(v => new BlogVideo
                {
                    VideoUrl = v.VideoUrl,
                    Caption = v.Caption,
                    DisplayOrder = v.DisplayOrder
                }).ToList()
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return Ok(blog);
        }

        [HttpPut("upload/{id}")]
        public async Task<IActionResult> UpdateWithImage(int id, [FromForm] BlogUploadDto dto)
        {
            var blog = await _context.Blogs
                .Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null) return NotFound();

            // Update scalar fields
            blog.Title = dto.Title;
            blog.Content = dto.Content;
            blog.Author = dto.Author;
            blog.Tags = dto.Tags;
            blog.IsPublished = dto.IsPublished;

            // Handle new cover image
            if (dto.CoverImage != null)
            {
                // Delete old image file (optional)
                if (!string.IsNullOrEmpty(blog.CoverImageUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.CoverImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                //  Check file type
                var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(dto.CoverImage.FileName).ToLower();

                if (!allowedTypes.Contains(extension))
                    return BadRequest("Only JPG, JPEG, PNG, or WEBP images are allowed.");

                //  Check file size (e.g., max 2MB)
                if (dto.CoverImage.Length > 2 * 1024 * 1024)
                    return BadRequest("Image size should not exceed 2MB.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.CoverImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.CoverImage.CopyToAsync(stream);
                }

                blog.CoverImageUrl = "/uploads/" + fileName; // or use full URL if needed
            }

            // Sync BlogVideos (delete, update, insert)
            var dtoVideoIds = dto.BlogVideos.Where(v => v.BlogVideoID.HasValue).Select(v => v.BlogVideoID.Value).ToList();
            var videosToRemove = blog.BlogVideos.Where(v => !dtoVideoIds.Contains(v.BlogVideoID)).ToList();
            _context.BlogVideos.RemoveRange(videosToRemove);

            foreach (var videoDto in dto.BlogVideos)
            {
                if (videoDto.BlogVideoID.HasValue)
                {
                    var existingVideo = blog.BlogVideos.FirstOrDefault(v => v.BlogVideoID == videoDto.BlogVideoID);
                    if (existingVideo != null)
                    {
                        existingVideo.VideoUrl = videoDto.VideoUrl;
                        existingVideo.Caption = videoDto.Caption;
                        existingVideo.DisplayOrder = videoDto.DisplayOrder;
                    }
                }
                else
                {
                    blog.BlogVideos.Add(new BlogVideo
                    {
                        VideoUrl = videoDto.VideoUrl,
                        Caption = videoDto.Caption,
                        DisplayOrder = videoDto.DisplayOrder
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(blog);
        }


    }

}
