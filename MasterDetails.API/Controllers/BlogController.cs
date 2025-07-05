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


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] BlogUploadDto dto)
        {
            // 1. Handle Cover Image
            string coverImageUrl = string.Empty;

            if (dto.CoverImage != null)
            {
                var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(dto.CoverImage.FileName).ToLower();
                if (!allowedTypes.Contains(ext)) return BadRequest("Invalid image format");

                if (dto.CoverImage.Length > 2 * 1024 * 1024)
                    return BadRequest("Image must be ≤ 2MB");

                var uploads = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var filename = Guid.NewGuid() + ext;
                var filePath = Path.Combine(uploads, filename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await dto.CoverImage.CopyToAsync(stream);

                coverImageUrl = "/uploads/" + filename;
            }

            // 2. Handle Author
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }

            // 3. Handle Tags
            var blogTags = new List<BlogTag>();
            foreach (var tagName in dto.TagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }

                blogTags.Add(new BlogTag { TagID = tag.TagID });
            }

            // 4. Create Blog
            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorID = author.AuthorID,
                IsPublished = dto.IsPublished,
                CoverImageUrl = coverImageUrl,
                CreatedAt = DateTime.Now,
                BlogTags = blogTags,
                BlogVideos = dto.BlogVideos.Select(v => _mapper.Map<BlogVideo>(v)).ToList()
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<BlogDto>(blog));
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BlogUploadDto dto)
        {
            var blog = await _context.Blogs
                .Include(b => b.Author)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null) return NotFound();

            blog.Title = dto.Title;
            blog.Content = dto.Content;
            blog.IsPublished = dto.IsPublished;

            // Cover image
            if (dto.CoverImage != null)
            {
                var ext = Path.GetExtension(dto.CoverImage.FileName).ToLower();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!allowed.Contains(ext)) return BadRequest("Invalid format");

                var uploads = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var filename = Guid.NewGuid() + ext;
                var path = Path.Combine(uploads, filename);

                using (var stream = new FileStream(path, FileMode.Create))
                    await dto.CoverImage.CopyToAsync(stream);

                blog.CoverImageUrl = "/uploads/" + filename;
            }

            // Author
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == dto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = dto.AuthorName };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }
            blog.AuthorID = author.AuthorID;

            // Tags (reset)
            blog.BlogTags.Clear();
            foreach (var tagName in dto.TagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName)
                          ?? new Tag { Name = tagName };

                _context.Tags.Attach(tag);
                blog.BlogTags.Add(new BlogTag { TagID = tag.TagID });
            }

            // BlogVideos (clear/add again for simplicity)
            blog.BlogVideos.Clear();
            foreach (var videoDto in dto.BlogVideos)
            {
                blog.BlogVideos.Add(_mapper.Map<BlogVideo>(videoDto));
            }

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<BlogDto>(blog));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetAll()
        {
            var blogs = await _context.Blogs
                .Include(b => b.Author)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.BlogVideos)
                .ToListAsync();

            return Ok(_mapper.Map<List<BlogDto>>(blogs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetById(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Author)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.BlogVideos)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null) return NotFound();

            return Ok(_mapper.Map<BlogDto>(blog));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.BlogVideos)
                .Include(b => b.BlogTags)
                .FirstOrDefaultAsync(b => b.BlogID == id);

            if (blog == null) return NotFound();

            // Remove child collections
            _context.BlogVideos.RemoveRange(blog.BlogVideos);
            _context.BlogTags.RemoveRange(blog.BlogTags);
            _context.Blogs.Remove(blog);

            if (!string.IsNullOrEmpty(blog.CoverImageUrl))
            {
                var filePath = Path.Combine("wwwroot", blog.CoverImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = $"Blog with ID {id} has been successfully deleted." });
        }




    }

}
