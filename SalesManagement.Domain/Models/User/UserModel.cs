namespace SalesManagement.Domain.Models.User
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Role { get; set; }
        public int? RoleId { get; set; }
    }
}
