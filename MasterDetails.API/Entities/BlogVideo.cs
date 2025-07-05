using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MasterDetails.API.Entities
{

    public partial class BlogVideo
    {
        [Key]
        public int BlogVideoID { get; set; }

        public int BlogID { get; set; }

        [StringLength(500)]
        public string VideoUrl { get; set; } = null!;

        public int? DisplayOrder { get; set; } = 0;

        [StringLength(255)]
        public string? Caption { get; set; }

        [ForeignKey("BlogID")]
        [InverseProperty("BlogVideos")]
        [JsonIgnore]
        public virtual Blog? Blog { get; set; } = null!;
    }

}
