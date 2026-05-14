using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,RegisteredUser,Manager,StoreOwner,Admin")]
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

            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsersAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangePassword(int id, string updatedPassword)
        {
            var response = await _userService.ChangePasswordAsync(id, updatedPassword);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserProfileAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO request)
        {
            var response = await _userService.RegisterAsync(request);

            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO request)
        {
            var response = await _userService.LoginAsync(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO request)
        {
            var response = await _userService.UpdateUserAsync(request);

            return StatusCode(response.StatusCode, response);
        }
    }
}