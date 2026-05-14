using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using BookStoreApplication.MVC.Wrappers;

namespace BookStoreApplication.MVC.Controllers
{
    // ─────────────────────────────────────────────────────────────
    //  MVC Category Controller
    //  Talks to your existing API: /api/category
    //  Route prefix:  /Categories
    // ─────────────────────────────────────────────────────────────
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string ApiUnavailableMessage =
            "BookStore API is not running. Start BookStoreApplication.Web and try again.";

        public CategoriesController(IHttpClientFactory httpClientFactory)
            => _httpClientFactory = httpClientFactory;

        // ── helpers ───────────────────────────────────────────────
        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("BookStoreApi");
            var token  = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private static readonly JsonSerializerOptions _json =
            new() { PropertyNameCaseInsensitive = true };

        private static async Task<string> GetApiErrorAsync(HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(body)
                ? $"API request failed: {(int)response.StatusCode} {response.ReasonPhrase}"
                : body;
        }

        // ── GET /CategoryMvc  (Index – list all) ──────────────────
        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            HttpResponseMessage resp;
            try
            {
                resp = await client.GetAsync("api/categories");
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = ApiUnavailableMessage;
                return View(new List<CategoryViewModel>());
            }

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = await GetApiErrorAsync(resp);
                return View(new List<CategoryViewModel>());
            }

            var json   = await resp.Content.ReadAsStringAsync();
            var wrapper    = JsonSerializer.Deserialize<ApiResponse<List<CategoryViewModel>>>(json, _json);
            var categories = wrapper?.Data ?? new List<CategoryViewModel>();

            return View(categories);
        }

        // ── GET /CategoryMvc/Create ───────────────────────────────
        public IActionResult Create() => View(new CategoryFormModel());

        // ── POST /CategoryMvc/Create ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client  = CreateClient();
            var payload = JsonSerializer.Serialize(new { model.CatDescription });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage resp;
            try
            {
                resp = await client.PostAsync("api/categories", content);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, ApiUnavailableMessage);
                return View(model);
            }

            if (resp.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var err = await GetApiErrorAsync(resp);
            ModelState.AddModelError(string.Empty, $"API Error: {err}");
            return View(model);
        }

        // ── GET /CategoryMvc/Edit/5 ───────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            HttpResponseMessage resp;
            try
            {
                resp = await client.GetAsync($"api/categories/{id}");
            }
            catch (HttpRequestException)
            {
                TempData["Error"] = ApiUnavailableMessage;
                return RedirectToAction(nameof(Index));
            }

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            var json    = await resp.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ApiResponse<CategoryViewModel>>(json, _json);
            var cat     = wrapper?.Data;

            var model = new CategoryFormModel
            {
                CatId          = cat!.CatId,
                CatDescription = cat.CatDescription
            };
            return View(model);
        }

        // ── POST /CategoryMvc/Edit/5 ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryFormModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client  = CreateClient();
            var payload = JsonSerializer.Serialize(new { model.CatDescription });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage resp;
            try
            {
                resp = await client.PutAsync($"api/categories/{id}", content);
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, ApiUnavailableMessage);
                return View(model);
            }

            if (resp.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            var err = await GetApiErrorAsync(resp);
            ModelState.AddModelError(string.Empty, $"API Error: {err}");
            return View(model);
        }
    }

    // ── Inline view-models ─────────────────────────────────────────
    public class CategoryViewModel
    {
        public int    CatId          { get; set; }
        public string CatDescription { get; set; } = "";
    }

    public class CategoryFormModel
    {
        public int CatId { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Description is required")]
        [System.ComponentModel.DataAnnotations.StringLength(255, MinimumLength = 2,
            ErrorMessage = "Description must be 2–255 characters")]
        public string CatDescription { get; set; } = "";
    }
}
