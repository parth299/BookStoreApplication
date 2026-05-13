using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookStoreApplication.MVC.Models;
using BookStoreApplication.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var request = new LoginRequestDto
        {
            UserName = model.UserName,
            Password = model.Password
        };

        var result = await _userService.LoginAsync(request);

        if (result != null && result.Success && result.Data != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result.Data.Token);

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value 
                        ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim("JWToken", result.Data.Token ?? ""),
                new Claim("UserId", userId ?? "")
            };

            // Extract roles from JWT
            var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", result?.Message ?? "Invalid login attempt.");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var request = new RegisterRequestDto
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber,
            Password = model.Password,
            RoleNumber = model.RoleNumber
        };

        var result = await _userService.RegisterAsync(request);

        if (result?.Success == true)
        {
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        ModelState.AddModelError("", result?.Message ?? "Registration failed.");
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var userId = User.FindFirst("UserId")?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Profile", new { id = int.Parse(userId) });
        }
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet("Profile/{id}")]
    public async Task<IActionResult> Profile(int id)
    {
        var loggedInUserId = User.FindFirst("UserId")?.Value;
        
        // Security check: only allow if user is viewing their own profile OR is an admin
        if (loggedInUserId != id.ToString() && !User.IsInRole("Admin"))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var result = await _userService.GetUserProfileAsync(id);
        if (result != null && result.Success && result.Data != null)
        {
            var model = new ProfileViewModel
            {
                UserId = result.Data.UserId,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                UserName = result.Data.UserName,
                PhoneNumber = result.Data.PhoneNumber,
                RoleName = User.FindFirst(ClaimTypes.Role)?.Value // This should ideally come from the user data if viewing another user, but for now we'll stick to current role or fetch role
            };
            return View(model);
        }
        
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost("Profile/{id}")]
    public async Task<IActionResult> Profile(int id, ProfileViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var loggedInUserId = User.FindFirst("UserId")?.Value;
        if (loggedInUserId != id.ToString() && !User.IsInRole("Admin"))
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        var request = new UserDto
        {
            UserId = id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userService.UpdateUserAsync(request);

        if (result != null && result.Success)
        {
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Profile), new { id = id });
        }

        ModelState.AddModelError("", result?.Message ?? "Update failed.");
        return View(model);
    }
}