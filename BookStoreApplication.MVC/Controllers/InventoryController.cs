using BookStoreApplication.MVC.DTOs.Inventory;
using BookStoreApplication.MVC.Services.Book_Publisher;
using BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;
[Authorize(Roles = "Admin,Manager,StoreOwner")]
public class InventoryController : BaseMvcController
{
    private readonly IInventoryService _inventory; private readonly IBookService _books;
    public InventoryController(IInventoryService inventory, IBookService books) { _inventory = inventory; _books = books; }
    public async Task<IActionResult> Index(string? isbn) { ViewBag.Books = (await _books.GetAllBooksAsync()).Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn)).ToList(); return View(await _inventory.GetAllAsync(isbn)); }
    public async Task<IActionResult> Available() => View(await _inventory.GetAvailableInventoryAsync());
    public async Task<IActionResult> LowStock() => View(await _inventory.GetLowStockAsync());
    [HttpGet] public async Task<IActionResult> Create() { ViewBag.Books = (await _books.GetAllBooksAsync()).Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn)).ToList(); LoadConditionDropdown(); return View(new CreateInventoryDto()); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(CreateInventoryDto dto) { if (!ModelState.IsValid) { ViewBag.Books = (await _books.GetAllBooksAsync()).Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn)).ToList(); LoadConditionDropdown(); return View(dto); } await _inventory.CreateAsync(dto); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id) { LoadConditionDropdown(); return View(await _inventory.GetByIdAsync(id)); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, InventoryDto dto) { if (!ModelState.IsValid) { LoadConditionDropdown(); return View(dto); } await _inventory.UpdateAsync(id, dto); return RedirectToAction(nameof(Index)); }
}
