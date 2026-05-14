using BookStoreApplication.MVC.DTOs.Book_Publisher;
using BookStoreApplication.MVC.Services.Book_Publisher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.MVC.Controllers;
[Authorize(Roles = "Admin,Manager,StoreOwner")]
public class PublishersController : Controller
{
    private readonly IPublisherService _service; public PublishersController(IPublisherService service) => _service = service;
    public async Task<IActionResult> Index(string? name) => View(string.IsNullOrWhiteSpace(name) ? await _service.GetAllAsync() : await _service.GetByNameAsync(name));
    public async Task<IActionResult> Details(int id) => View(await _service.GetDetailsByIdAsync(id));
    [HttpGet] public IActionResult Create() => View(new PublisherCreateDto());
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Create(PublisherCreateDto dto) { if (!ModelState.IsValid) return View(dto); await _service.CreateAsync(dto); return RedirectToAction(nameof(Index)); }
    [HttpGet] public async Task<IActionResult> Edit(int id) { var p = await _service.GetDetailsByIdAsync(id); return View(new PublisherUpdateDto { Name = p.Name, City = p.City, StateCode = p.StateCode }); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id, PublisherUpdateDto dto) { if (!ModelState.IsValid) return View(dto); await _service.UpdateAsync(id, dto); return RedirectToAction(nameof(Index)); }
}
