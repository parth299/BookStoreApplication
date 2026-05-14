using BookStoreApplication.MVC.DTOs.Inventory;
using BookStoreApplication.MVC.Services.Book_Publisher;
using BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IShoppingCartService _cart;
    private readonly IBookService _books;
    private readonly IPurchaseLogService _purchaseLogs;

    public CartController(IShoppingCartService cart, IBookService books, IPurchaseLogService purchaseLogs)
    {
        _cart = cart;
        _books = books;
        _purchaseLogs = purchaseLogs;
    }

    public async Task<IActionResult> Index(int userId = 1)
    {
        ViewBag.UserId = userId;
        return View(await _cart.GetCartAsync(userId));
    }

    [HttpGet]
    public async Task<IActionResult> Add(int userId = 1)
    {
        ViewBag.Books = (await _books.GetAllBooksAsync())
            .Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn))
            .ToList();
        return View(new CartItemDto { UserId = userId, Quantity = 1 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(CartItemDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Books = (await _books.GetAllBooksAsync())
                .Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn))
                .ToList();
            return View(dto);
        }

        await _cart.AddAsync(dto);
        TempData["Success"] = "Book added to cart.";
        return RedirectToAction(nameof(Index), new { userId = dto.UserId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int userId, string isbn)
    {
        await _cart.RemoveAsync(userId, isbn);
        TempData["Success"] = "Cart item removed.";
        return RedirectToAction(nameof(Index), new { userId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear(int userId)
    {
        await _cart.ClearAsync(userId);
        TempData["Success"] = "Cart cleared.";
        return RedirectToAction(nameof(Index), new { userId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(int userId = 1)
    {
        await _purchaseLogs.CheckoutAsync(new PurchaseDto { UserId = userId });
        TempData["Success"] = "Purchase completed and saved to purchase logs.";
        return RedirectToAction(nameof(Index), new { userId });
    }
}
