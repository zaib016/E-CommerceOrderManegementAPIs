namespace E_CommerceOrderManagementAPI.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required int PhoneNumber { get; set; }
        public required string Role { get; set; }
    }
}
