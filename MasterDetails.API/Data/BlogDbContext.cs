using MasterDetails.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.API.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogVideo> BlogVideos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Blog>()
                .HasMany(b => b.BlogVideos)
                .WithOne(v => v.Blog)
                .HasForeignKey(v => v.BlogID)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Blog>()
                .Property(b => b.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()");

            // Seed Blogs
            modelBuilder.Entity<Blog>().HasData(
                new Blog
                {
                    BlogID = 1,
                    Title = "Introduction to C# 10",
                    Content = "This blog post covers the new features in C# 10...",
                    Author = "John Doe",
                    CoverImageUrl = "https://example.com/images/csharp10.png",
                    Tags = "C#, .NET, Programming",
                    CreatedAt = DateTime.Now,
                    IsPublished = true
                },
                new Blog
                {
                    BlogID = 2,
                    Title = "Getting Started with ASP.NET Core",
                    Content = "Learn how to build web applications using ASP.NET Core...",
                    Author = "Jane Smith",
                    CoverImageUrl = "https://example.com/images/aspnetcore.png",
                    Tags = "ASP.NET Core, Web Development",
                    CreatedAt = DateTime.Now,
                    IsPublished = true
                }
            );

            // Seed BlogVideos
            modelBuilder.Entity<BlogVideo>().HasData(
                new BlogVideo
                {
                    BlogVideoID = 1,
                    BlogID = 1,
                    VideoUrl = "https://www.youtube.com/embed/dQw4w9WgXcQ",
                    DisplayOrder = 1,
                    Caption = "C# 10 New Features Overview"
                },
                new BlogVideo
                {
                    BlogVideoID = 2,
                    BlogID = 1,
                    VideoUrl = "https://www.youtube.com/embed/oHg5SJYRHA0",
                    DisplayOrder = 2,
                    Caption = "Advanced C# 10 Tips"
                },
                new BlogVideo
                {
                    BlogVideoID = 3,
                    BlogID = 2,
                    VideoUrl = "https://www.youtube.com/embed/abcdefg1234",
                    DisplayOrder = 1,
                    Caption = "ASP.NET Core Beginner Tutorial"
                }
            );
        }



    }
}

