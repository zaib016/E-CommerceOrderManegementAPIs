using E_CommerceOrderManagementAPI.Models.DTOs;
using E_CommerceOrderManagementAPI.Models.Entities;

namespace E_CommerceOrderManagementAPI.Services.Interfaces
{
    public interface IUserReopsitory
    {
        Task<List<User>> GetUserListAsync();
        Task<User?> GetUserEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<string> UserRegisterationAsync(UserDTOs.UserRegisteration userRegisteration);
        Task<string> UserLoginAsync(UserDTOs.UserLogin userLogin);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        
    }
    public interface ITokenService
    {
        string CreateTokenAsync(User user);
    }
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
