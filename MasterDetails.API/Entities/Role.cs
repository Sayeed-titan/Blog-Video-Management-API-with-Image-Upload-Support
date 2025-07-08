using System.ComponentModel.DataAnnotations;

namespace MasterDetails.API.Entities
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [StringLength(50)]
        public string RoleName { get; set; } = default!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
