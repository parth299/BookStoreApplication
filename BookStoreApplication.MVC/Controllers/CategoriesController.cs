using BookStoreApplication.MVC.DTOs.Category;
using BookStoreApplication.MVC.Services.Author_Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;
[Authorize(Roles = "Admin,Manager,StoreOwner")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _service; public CategoriesController(ICategoryService service) => _service = service;
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());
    [HttpGet] public IActionResult Create() => View(new CategoryRequestDto());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(CategoryRequestDto dto) { if (!ModelState.IsValid) return View(dto); await _service.CreateAsync(dto); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id) { var c = await _service.GetByIdAsync(id); return View(new CategoryRequestDto { CatDescription = c?.CatDescription ?? "" }); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, CategoryRequestDto dto) { if (!ModelState.IsValid) return View(dto); await _service.UpdateAsync(id, dto); return RedirectToAction(nameof(Index)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Delete(int id) { await _service.DeleteAsync(id); return RedirectToAction(nameof(Index)); }
}
