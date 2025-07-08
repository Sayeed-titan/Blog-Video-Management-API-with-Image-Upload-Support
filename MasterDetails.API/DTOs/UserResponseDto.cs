namespace MasterDetails.API.DTOs
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        //public string? RoleName { get; set; } 
    }
}
