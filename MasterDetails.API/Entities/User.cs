using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace MasterDetails.API.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, StringLength(50)]
        public string? FirstName { get; set; } = default!;

        [Required, StringLength(50)]
        public string? LastName { get; set; } = default!;

        [Required, StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [JsonIgnore]
        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = default!;

        [StringLength(20)]
        public string? Phone { get; set; } = string.Empty;

        public string? RefeshToken { get; set; } = string.Empty;

        public DateTime? RefreshTokenExpiryTme { get; set; }


        [Required]
        [Column("RoleID")]
        public int RoleID { get; set; }

        public virtual Role? Role { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

       


    }
}
