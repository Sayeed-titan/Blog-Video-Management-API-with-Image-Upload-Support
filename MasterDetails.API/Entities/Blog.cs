using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.API.Entities{


    public partial class Blog
    {
        [Key]
        public int BlogID { get; set; }

        [StringLength(255)]
        public string Title { get; set; } = string.Empty!;

        public string Content { get; set; } = string.Empty!;

        public int AuthorID { get; set; }
        public Author Author { get; set; } = null!;

        [StringLength(500)]
        public string CoverImageUrl { get; set; } = string.Empty!;

        public string? Tags { get; set; } = string.Empty!;

        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        public bool IsPublished { get; set; } = false;


        [InverseProperty("Blog")]
        public virtual ICollection<BlogVideo> BlogVideos { get; set; } = new List<BlogVideo>();
        public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();

    }

}