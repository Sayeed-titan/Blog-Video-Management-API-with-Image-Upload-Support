using MasterDetails.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.API.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogVideo> BlogVideos { get; set; }

        public DbSet<User> Users { get; set; } 
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many Blog ↔ Tag
            modelBuilder.Entity<BlogTag>()
                .HasKey(bt => new { bt.BlogID, bt.TagID });

            modelBuilder.Entity<BlogTag>()
                .HasOne(bt => bt.Blog)
                .WithMany(b => b.BlogTags)
                .HasForeignKey(bt => bt.BlogID);

            modelBuilder.Entity<BlogTag>()
                .HasOne(bt => bt.Tag)
                .WithMany(t => t.BlogTags)
                .HasForeignKey(bt => bt.TagID);

            // Author → Blog (One to Many)
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Blogs)
                .HasForeignKey(b => b.AuthorID);
        }
    }

}

