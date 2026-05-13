using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookStoreApplication.MVC.DTOs.User;
using BookStoreApplication.MVC.Services.User;
using BookStoreApplication.MVC.Models.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _users; public AccountController(IUserService users) => _users = users;
    [HttpGet] public IActionResult Login() => View(new LoginUserDTO());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginUserDTO model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _users.LoginAsync(model);
        if (!result.Success || result.Data == null) { ModelState.AddModelError("", result.Message ?? "Invalid login."); return View(model); }
        var token = result.Data.Token ?? string.Empty;
        HttpContext.Session.SetString("JWToken", token);
        var claims = new List<Claim> { new(ClaimTypes.Name, model.UserName), new("JWToken", token) };
        if (!string.IsNullOrWhiteSpace(token))
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            claims.AddRange(jwt.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type.EndsWith("/role") || c.Type.Equals("role", StringComparison.OrdinalIgnoreCase))
                .Select(c => new Claim(ClaimTypes.Role, c.Value)));
            var nameClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type.EndsWith("/name") || c.Type == "unique_name");
            if (nameClaim != null) claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
        }
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return RedirectToAction("Index", "Home");
    }
    [HttpGet] public IActionResult Register() => View(new RegisterUserDTO());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Register(RegisterUserDTO model)
    {
        if (!ModelState.IsValid) return View(model);
        model.RoleNumber = (int)UserRole.User;
        var result = await _users.RegisterAsync(model);
        if (!result.Success) { ModelState.AddModelError("", result.Message ?? "Registration failed."); return View(model); }
        return RedirectToAction(nameof(Login));
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Logout() { await HttpContext.SignOutAsync(); return RedirectToAction("Index", "Home"); }
    public IActionResult AccessDenied() => View();
}
