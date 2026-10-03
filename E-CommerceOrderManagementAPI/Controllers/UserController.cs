using E_CommerceOrderManagementAPI.Models.DTOs;
using E_CommerceOrderManagementAPI.Models.Entities;
using E_CommerceOrderManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_CommerceOrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserReopsitory _userRepo;
        private ITokenService _tokenService;
        private IEmailService _emailService;

        public UserController(IUserReopsitory userReopsitory, ITokenService tokenService, IEmailService emailService)
        {
            _userRepo = userReopsitory;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userRepo.GetUserListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost("UserRegisteration")]
        public async Task<IActionResult> register(UserDTOs.UserRegisteration userRegisteration)
        {
            var user = await _userRepo.UserRegisterationAsync(userRegisteration);
            if (user == null) return BadRequest();

            var subject = "Welcome our App";
            var body = $"{userRegisteration.Username}<br>Thanks 😊 For Registering</br>";
            await _emailService.SendEmailAsync(userRegisteration.Email, subject, body);

            return Ok("Registerted Successfully: Email Send!😊");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login(UserDTOs.UserLogin login)
        {
            var user = await _userRepo.UserLoginAsync(login);
            if (user == "Invalid Email or Password") return Unauthorized("Unauthorized");

            return Ok(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> update(UserDTOs.UserRegisteration userRegisteration, int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) return BadRequest();

            var update = new User
            {
                Username = userRegisteration.Username,
                Email = userRegisteration.Email,
                Password = userRegisteration.Password,
                PhoneNumber = userRegisteration.PhoneNumber,
                Role = userRegisteration.Role,
            };

            return Ok(update);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var user = await _userRepo.DeleteUserAsync(id);
            if (user == false) return NotFound();

            return Ok("User Deleted!!");
        }

    }
}
