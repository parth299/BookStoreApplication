using BookStoreApplication.MVC.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;
[Authorize(Roles = "Admin")]
public class UserAdminController : Controller
{
    private readonly IUserService _users; public UserAdminController(IUserService users) => _users = users;
    public async Task<IActionResult> Index() { var r = await _users.GetUsersAsync(); return View(r.Data ?? []); }
}
