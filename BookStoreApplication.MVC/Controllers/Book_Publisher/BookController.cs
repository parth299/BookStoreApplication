using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookStoreApplication.MVC.Models;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.DTOs.Category;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers.Book_Publisher;

public class BookController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BookController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index([FromQuery] string? title)
    {
        var books = string.IsNullOrWhiteSpace(title)
            ? await GetDataAsync<IReadOnlyList<BookResponseDto>>("books")
            : await GetDataAsync<IReadOnlyList<BookResponseDto>>($"books/search?title={Uri.EscapeDataString(title)}");

        var model = new BookCatalogIndexViewModel
        {
            Books = books ?? [],
            Publishers = await GetPublishersAsync(),
            Categories = await GetCategoriesAsync(),
            Filter = new BookFilterViewModel { Title = title },
            CanManageCatalog = CanManageCatalog(),
            Notice = TempData["CatalogMessage"] as string
        };

        return View(model);
    }

    public async Task<IActionResult> Detail(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            return BadRequest();
        }

        var book = await GetDataAsync<BookResponseDto>($"books/{Uri.EscapeDataString(isbn)}");
        if (book is null)
        {
            return NotFound();
        }

        return View(new BookDetailViewModel
        {
            Book = book,
            CategoryName = await GetCategoryNameAsync(book.Category),
            CanManageCatalog = CanManageCatalog()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return View(new BookFormViewModel
        {
            Isbn = GenerateIsbn(),
            Publishers = await GetPublishersAsync(),
            Categories = await GetCategoriesAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookFormViewModel model)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (!ModelState.IsValid)
        {
            await PopulateBookFormListsAsync(model);
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.Isbn))
        {
            model.Isbn = GenerateIsbn();
        }

        var response = await CreateClient().PostAsJsonAsync("books", new BookCreateDto
        {
            Isbn = model.Isbn.Trim(),
            Title = model.Title.Trim(),
            Description = model.Description,
            Category = model.Category,
            Edition = model.Edition,
            PublisherId = model.PublisherId
        });

        if (!await AddApiErrorsAsync(response))
        {
            await PopulateBookFormListsAsync(model);
            return View(model);
        }

        TempData["CatalogMessage"] = "Book created successfully.";
        return RedirectToAction(nameof(Detail), new { isbn = model.Isbn });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string isbn)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (string.IsNullOrWhiteSpace(isbn))
        {
            return BadRequest();
        }

        var book = await GetDataAsync<BookResponseDto>($"books/{Uri.EscapeDataString(isbn)}");
        if (book is null)
        {
            return NotFound();
        }

        return View(new BookFormViewModel
        {
            Isbn = book.Isbn,
            Title = book.Title,
            Description = book.Description,
            Category = book.Category,
            Edition = book.Edition,
            PublisherId = book.PublisherId,
            Publishers = await GetPublishersAsync(),
            Categories = await GetCategoriesAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string isbn, BookFormViewModel model)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        if (string.IsNullOrWhiteSpace(isbn))
        {
            return BadRequest();
        }

        ModelState.Remove(nameof(BookFormViewModel.Isbn));
        if (!ModelState.IsValid)
        {
            model.Isbn = isbn;
            await PopulateBookFormListsAsync(model);
            return View(model);
        }

        var response = await CreateClient().PutAsJsonAsync($"books/{Uri.EscapeDataString(isbn)}", new BookUpdateDto
        {
            Title = model.Title.Trim(),
            Description = model.Description,
            Category = model.Category,
            Edition = model.Edition,
            PublisherId = model.PublisherId
        });

        if (!await AddApiErrorsAsync(response))
        {
            model.Isbn = isbn;
            await PopulateBookFormListsAsync(model);
            return View(model);
        }

        TempData["CatalogMessage"] = "Book updated successfully.";
        return RedirectToAction(nameof(Detail), new { isbn });
    }

    [HttpGet]
    public async Task<IActionResult> Filter([FromQuery] BookFilterViewModel filter)
    {
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query.Add($"Title={Uri.EscapeDataString(filter.Title)}");
        }
        if (filter.Category.HasValue)
        {
            query.Add($"Category={filter.Category.Value}");
        }
        if (filter.PublisherId.HasValue)
        {
            query.Add($"PublisherId={filter.PublisherId.Value}");
        }
        var endpoint = query.Count == 0 ? "books" : $"books/filter?{string.Join("&", query)}";
        var model = new BookCatalogIndexViewModel
        {
            Books = await GetDataAsync<IReadOnlyList<BookResponseDto>>(endpoint) ?? [],
            Publishers = await GetPublishersAsync(),
            Categories = await GetCategoriesAsync(),
            Filter = filter,
            CanManageCatalog = CanManageCatalog()
        };

        return View(model);
    }

    private async Task<IReadOnlyList<PublisherResponseDto>> GetPublishersAsync()
    {
        return await GetDataAsync<IReadOnlyList<PublisherResponseDto>>("publishers") ?? [];
    }

    private async Task<IReadOnlyList<CategoryResponseDto>> GetCategoriesAsync()
    {
        var categories = await GetDataAsync<IEnumerable<CategoryResponseDto>>("api/Category");
        return categories?.ToList() ?? [];
    }

    private async Task<string> GetCategoryNameAsync(int? categoryId)
    {
        if (!categoryId.HasValue)
        {
            return "Not assigned";
        }

        var categories = await GetCategoriesAsync();
        return categories.FirstOrDefault(category => category.CatId == categoryId.Value)?.CatDescription ?? "Not assigned";
    }

    private async Task PopulateBookFormListsAsync(BookFormViewModel model)
    {
        model.Publishers = await GetPublishersAsync();
        model.Categories = await GetCategoriesAsync();
    }

    private async Task<T?> GetDataAsync<T>(string endpoint)
    {
        var response = await CreateClient().GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        return payload is { Success: true } ? payload.Data : default;
    }

    private async Task<bool> AddApiErrorsAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        var payload = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
        if (payload?.Errors?.Count > 0)
        {
            foreach (var error in payload.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, payload?.Message ?? "The API rejected the request.");
        }

        return false;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("BookStoreApi");
        if (Request.Headers.TryGetValue("Authorization", out var authorization))
        {
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authorization!);
        }

        return client;
    }

    private bool CanManageCatalog()
    {
        //return User.IsInRole("Admin") || User.IsInRole("StoreOwner");
        return true;
    }

    private static string GenerateIsbn()
    {
        var value = Math.Abs(DateTimeOffset.UtcNow.Ticks % 1_000_000_000);
        var prefix = (value / 100_000_000) % 10;
        var group = (value / 100_000) % 1_000;
        var title = value % 100_000;
        var check = (prefix + group + title) % 10;

        return $"{prefix:0}-{group:000}-{title:00000}-{check:0}";
    }

}
