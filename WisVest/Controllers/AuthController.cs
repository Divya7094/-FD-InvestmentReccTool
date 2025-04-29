using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WisVestAPI.Models;
using WisVestAPI.Services;
 
namespace WisVestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PasswordHasher<User> _passwordHasher;
 
        public AuthController(UserService userService)
        {
            _userService = userService;
            _passwordHasher = new PasswordHasher<User>();
        }
 
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest request)
        {
if (_userService.UserExists(request.Email))
            {
                return BadRequest(new { message = "User already exists" });
            }
 
            var user = new User
            {
Email = request.Email
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
 
            _userService.AddUser(user);
 
            return Ok(new { message = "Registration successful" });
        }
    }
 
    public class UserRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}