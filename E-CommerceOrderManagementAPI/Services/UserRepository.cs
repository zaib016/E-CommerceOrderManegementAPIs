using E_CommerceOrderManagementAPI.Data;
using E_CommerceOrderManagementAPI.Models.DTOs;
using E_CommerceOrderManagementAPI.Models.Entities;
using E_CommerceOrderManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace E_CommerceOrderManagementAPI.Services
{
    public class UserRepository : IUserReopsitory, ITokenService, IEmailService
    {
        private ApplicationDbContext _dbContext;
        private IConfiguration _config;
        private ICacheRepository _cache;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration, ICacheRepository cacheRepository)
        {
            _dbContext = dbContext;
            _config = configuration;
            _cache = cacheRepository;
        }

        public string CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Role, user.Role),
               new Claim("username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    expires: DateTime.Today.AddDays(1),
                    claims: claims,
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null) return false;

            _dbContext.Remove(user);
            _cache.Remove("User_List");
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(i => i.UserId == id);
        }

        public async Task<User?> GetUserEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<User>> GetUserListAsync()
        {
            string cacheKey = $"{typeof(User).Name}_List";
            var cachedData = _cache.Get<List<User>>(cacheKey);

            if(cachedData != null)
            {
                return cachedData;
            }

            var user = _dbContext.Users.ToList();
            _cache.Set(cacheKey, user, 5);
            return user;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var FromEmail = _config["EmailSitting:FromEmail"];
            var Password = _config["EmailSitting:Password"];

            var mail = new MailMessage();
            mail.From = new MailAddress(FromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using(var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(FromEmail, Password);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(mail);
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _cache.Remove("User_List");
            _dbContext.Users.Update(user); 
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<string> UserLoginAsync(UserDTOs.UserLogin userLogin)
        {
            var user = await GetUserEmailAsync(userLogin.Email);
            if (user == null) return "Invalid Email or Password ";

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(userLogin.Password , user.Password);
            if (!isPasswordValid) return "Invalid Email or Password";

            return CreateTokenAsync(user);

        }

        public async Task<string> UserRegisterationAsync(UserDTOs.UserRegisteration userRegisteration)
        {
            var exists = await GetUserEmailAsync(userRegisteration.Email);
            if (exists != null) return "User Already Exists";

            var user = new User
            {
                Username = userRegisteration.Username,
                Email = userRegisteration.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userRegisteration.Password),
                PhoneNumber = userRegisteration.PhoneNumber,
                Role = userRegisteration.Role,
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();
            return ("User Register Successfully");
        }
    }
}
