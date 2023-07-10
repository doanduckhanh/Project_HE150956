namespace ProjectManagementAPI.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
    }
}
