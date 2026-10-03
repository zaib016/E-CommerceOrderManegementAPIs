namespace E_CommerceOrderManagementAPI.Models.DTOs
{
    public class UserDTOs
    {
        public class UserRegisteration
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required int PhoneNumber { get; set; }
            public required string Role { get; set; }
        }
        public class UserLogin 
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
        

    }
}
