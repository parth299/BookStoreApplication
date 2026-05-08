using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO request)
        {
            var response = await _authService.RegisterAsync(request);

            if(response is null)
            {
                return BadRequest("User not registered");
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO request)
        {
            var response = await _authService.LoginAsync(request);

            if(response is null)
            {
                return BadRequest("Register the user");
            }
            if(response == "Provide username and password")
            {
                return BadRequest("Provide username and password");
            }
            if(response == "Incorrect Password")
            {
                return BadRequest("Incorrect password");
            }

            return Ok("Login Success");
        }
    }
}