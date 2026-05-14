using BookStoreApplication.MVC.DTOs.Book_Publisher;
using BookStoreApplication.MVC.Services.Author_Category;
using BookStoreApplication.MVC.Services.Book_Publisher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;

public class BooksController : BaseMvcController
{
    private readonly IBookService _books; private readonly ICategoryService _categories; private readonly IPublisherService _publishers;
    public BooksController(IBookService books, ICategoryService categories, IPublisherService publishers) { _books = books; _categories = categories; _publishers = publishers; }
    public async Task<IActionResult> Index(string? title, int? category, int? publisherId)
    {
        await LoadBookDropdowns(_books, _categories, _publishers);
        var data = !string.IsNullOrWhiteSpace(title) ? await _books.SearchAsync(title) : await _books.FilterAsync(new BookFilterDto { Category = category, PublisherId = publisherId });
        return View(data);
    }
    public async Task<IActionResult> Details(string id) => View(await _books.GetByIsbnAsync(id));
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpGet] public async Task<IActionResult> Create() { await LoadBookDropdowns(_books, _categories, _publishers); return View(new BookCreateDto()); }
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(BookCreateDto dto) { if (!ModelState.IsValid) { await LoadBookDropdowns(_books, _categories, _publishers); return View(dto); } await _books.CreateAsync(dto); return RedirectToAction(nameof(Index)); }
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpGet] public async Task<IActionResult> Edit(string id) { await LoadBookDropdowns(_books, _categories, _publishers); var b = await _books.GetByIsbnAsync(id); return View(new BookUpdateDto { Title = b.Title, Description = b.Description, Category = b.Category, Edition = b.Edition, PublisherId = b.PublisherId }); }
    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(string id, BookUpdateDto dto) { if (!ModelState.IsValid) { await LoadBookDropdowns(_books, _categories, _publishers); return View(dto); } await _books.UpdateAsync(id, dto); return RedirectToAction(nameof(Index)); }
    public async Task<IActionResult> TopRated(int count = 10) => View(await _books.GetTopRatedAsync(count));
}
