using BookStoreApplication.MVC.Models;
using BookStoreApplication.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class UserAdminController : Controller
{
    private readonly IUserService _userService;

    public UserAdminController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _userService.GetUsersAsync();
        var model = new UserListViewModel();
        
        if (result != null && result.Success && result.Data != null)
        {
            model.Users = result.Data;
        }
        else
        {
            TempData["ErrorMessage"] = result?.Message ?? "Failed to fetch users.";
        }

        return View(model);
    }
}
