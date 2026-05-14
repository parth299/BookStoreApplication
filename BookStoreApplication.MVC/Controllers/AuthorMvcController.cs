using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Wrappers;

namespace BookStoreApplication.MVC.Controllers;

public class AuthorController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiUnavailableMessage =
        "BookStore API is not running. Start BookStoreApplication.Web and try again.";

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public AuthorController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("BookStoreApi");
        var token = HttpContext.Session.GetString("JwtToken");

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    private static async Task<string> GetApiErrorAsync(HttpResponseMessage response)
    {
        var body = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(body)
            ? $"API request failed: {(int)response.StatusCode} {response.ReasonPhrase}"
            : body;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? search = null)
    {
        ViewBag.Search = search;

        var client = CreateClient();
        var url = string.IsNullOrWhiteSpace(search)
            ? $"api/authors?pageNumber={pageNumber}&pageSize={pageSize}"
            : $"api/authors/search?name={Uri.EscapeDataString(search)}";

        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(url);
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = ApiUnavailableMessage;
            return View(new List<AuthorResponseDTO>());
        }

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = await GetApiErrorAsync(response);
            return View(new List<AuthorResponseDTO>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<ApiResponse<List<AuthorResponseDTO>>>(json, JsonOptions);
        return View(wrapper?.Data ?? new List<AuthorResponseDTO>());
    }

    public async Task<IActionResult> Detail(int id)
    {
        var client = CreateClient();
        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync($"api/authors/{id}");
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = ApiUnavailableMessage;
            return RedirectToAction(nameof(Index));
        }

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<ApiResponse<AuthorResponseDTO>>(json, JsonOptions);
        return View(wrapper?.Data);
    }

    public Task<IActionResult> Details(int id) => Detail(id);

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var client = CreateClient();
        var payload = JsonSerializer.Serialize(dto);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync("api/authors", content);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, ApiUnavailableMessage);
            return View(dto);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Author created successfully.";
            return RedirectToAction(nameof(Index));
        }

        var error = await GetApiErrorAsync(response);
        ModelState.AddModelError(string.Empty, $"API Error: {error}");
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var client = CreateClient();
        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync($"api/authors/{id}");
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = ApiUnavailableMessage;
            return RedirectToAction(nameof(Index));
        }

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<ApiResponse<AuthorResponseDTO>>(json, JsonOptions);
        var author = wrapper?.Data;

        if (author == null)
        {
            return NotFound();
        }

        ViewBag.AuthorId = author.AuthorId;
        return View(new AuthorRequestDTO
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
            Photo = author.Photo
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AuthorRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AuthorId = id;
            return View(dto);
        }

        var client = CreateClient();
        var payload = JsonSerializer.Serialize(dto);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        HttpResponseMessage response;
        try
        {
            response = await client.PutAsync($"api/authors/{id}", content);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, ApiUnavailableMessage);
            ViewBag.AuthorId = id;
            return View(dto);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Author updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        var error = await GetApiErrorAsync(response);
        ModelState.AddModelError(string.Empty, $"API Error: {error}");
        ViewBag.AuthorId = id;
        return View(dto);
    }

    public async Task<IActionResult> Search(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return View(new List<AuthorResponseDTO>());
        }

        var client = CreateClient();
        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync($"api/authors/search?name={Uri.EscapeDataString(name)}");
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = ApiUnavailableMessage;
            return View(new List<AuthorResponseDTO>());
        }

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = await GetApiErrorAsync(response);
            return View(new List<AuthorResponseDTO>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<ApiResponse<List<AuthorResponseDTO>>>(json, JsonOptions);
        return View(wrapper?.Data ?? new List<AuthorResponseDTO>());
    }

    [HttpPost]
    public async Task<IActionResult> UploadPhoto(int id, IFormFile photo)
    {
        if (photo == null || photo.Length == 0)
        {
            TempData["Error"] = "No photo selected.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        var client = CreateClient();
        using var form = new MultipartFormDataContent();
        await using var stream = photo.OpenReadStream();
        form.Add(new StreamContent(stream), "photo", photo.FileName);
        HttpResponseMessage response;
        try
        {
            response = await client.PostAsync($"api/authors/{id}/photo", form);
        }
        catch (HttpRequestException)
        {
            TempData["Error"] = ApiUnavailableMessage;
            return RedirectToAction(nameof(Edit), new { id });
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Photo uploaded successfully.";
        }
        else
        {
            TempData["Error"] = await GetApiErrorAsync(response);
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> SearchJson(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Json(Array.Empty<AuthorResponseDTO>());
        }

        var client = CreateClient();
        HttpResponseMessage response;

        try
        {
            response = await client.GetAsync($"api/authors/search?name={Uri.EscapeDataString(name)}");
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ApiUnavailableMessage);
        }

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, await GetApiErrorAsync(response));
        }

        var json = await response.Content.ReadAsStringAsync();
        var wrapper = JsonSerializer.Deserialize<ApiResponse<List<AuthorResponseDTO>>>(json, JsonOptions);
        return Json(wrapper?.Data ?? new List<AuthorResponseDTO>());
    }
}
