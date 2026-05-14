using BookStoreApplication.MVC.DTOs.Author;
using BookStoreApplication.MVC.Services.Author_Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;
public class AuthorsController : Controller
{
    private readonly IAuthorService _service; public AuthorsController(IAuthorService service) => _service = service;
    public async Task<IActionResult> Index(string? name, string? country, int page = 1, int pageSize = 10)
    {
        if (!string.IsNullOrWhiteSpace(name)) return View(await _service.SearchByNameAsync(name));
        if (!string.IsNullOrWhiteSpace(country)) return View(await _service.GetByCountryAsync(country));
        return View(await _service.GetAllAsync(new PaginationFilter(page, pageSize)));
    }
    public async Task<IActionResult> Details(int id) => View(await _service.GetByIdWithBooksAsync(id));
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpGet] public IActionResult Create() => View(new AuthorRequestDTO());
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(AuthorRequestDTO dto) { if (!ModelState.IsValid) return View(dto); await _service.CreateAsync(dto); return RedirectToAction(nameof(Index)); }
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpGet] public async Task<IActionResult> Edit(int id) { var a = await _service.GetByIdAsync(id); return View(new AuthorRequestDTO { FirstName = a?.FirstName ?? "", LastName = a?.LastName ?? "", Photo = a?.Photo }); }
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, AuthorRequestDTO dto) { if (!ModelState.IsValid) return View(dto); await _service.UpdateAsync(id, dto); return RedirectToAction(nameof(Index)); }
}
