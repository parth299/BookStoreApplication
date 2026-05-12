using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetUsersByRoleId(int roleId)
        {
            var response = await _userService.GetUsersByRoleId(roleId);

            if(response is null)
            {
                return BadRequest("Cannot fetch users of given role");
            }

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsersAsync();
            
            if(response is null)
            {
                return BadRequest("Cannot fetch users");
            }

            return Ok(response);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangePassword(int id, string updated_password)
        {
            var response = await _userService.ChangePasswordAsync(id, updated_password);

            if(response is null)
            {
                return BadRequest("User does not exists");
            }
            
            return Ok("User Password Updated");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserProfileAsync(id);
            
            if(response is null)
            {
                return BadRequest("Cannot fetch user profile");
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO request)
        {
            var response = await _userService.RegisterAsync(request);

            if(response is null)
            {
                return BadRequest("User not registered");
            }

            return Ok("User registered successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO request)
        {
            var response = await _userService.LoginAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO request)
        {
            var response = await _userService.UpdateUserAsync(request);

            if(response is null)
            {
                return BadRequest("Update user failed");
            }

            return Ok(request);
        }
    }
}
