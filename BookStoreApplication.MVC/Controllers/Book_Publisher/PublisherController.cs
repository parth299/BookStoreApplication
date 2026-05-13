using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookStoreApplication.MVC.Models;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class PublisherController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PublisherController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(string? name)
    {
        var publishers = string.IsNullOrWhiteSpace(name)
            ? await GetDataAsync<IReadOnlyList<PublisherResponseDto>>("publishers")
            : (await GetDataAsync<IReadOnlyList<PublisherDetailsDto>>($"publishers/by-name/{Uri.EscapeDataString(name)}"))
                ?.Select(p => new PublisherResponseDto
                {
                    PublisherId = p.PublisherId,
                    Name = p.Name,
                    City = p.City,
                    StateCode = p.StateCode
                })
                .ToList();

        return View(new PublisherIndexViewModel
        {
            Publishers = publishers ?? [],
            CanManageCatalog = CanManageCatalog(),
            SearchName = name
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return View(new PublisherFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PublisherFormViewModel model)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        ModelState.Remove(nameof(PublisherFormViewModel.PublisherId));
        model.PublisherId = await GetNextPublisherIdAsync();
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var response = await CreateClient().PostAsJsonAsync("publishers", new PublisherCreateDto
        {
            PublisherId = model.PublisherId,
            Name = model.Name.Trim(),
            City = model.City,
            StateCode = model.StateCode?.Trim().ToUpperInvariant()
        });

        if (!await AddApiErrorsAsync(response))
        {
            return View(model);
        }

        TempData["CatalogMessage"] = "Publisher created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var publisher = await GetDataAsync<PublisherDetailsDto>($"publishers/{id}");
        if (publisher is null)
        {
            return NotFound();
        }

        return View(new PublisherFormViewModel
        {
            PublisherId = publisher.PublisherId,
            Name = publisher.Name,
            City = publisher.City,
            StateCode = publisher.StateCode
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PublisherFormViewModel model)
    {
        if (!CanManageCatalog())
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        ModelState.Remove(nameof(PublisherFormViewModel.PublisherId));
        if (!ModelState.IsValid)
        {
            model.PublisherId = id;
            return View(model);
        }

        var response = await CreateClient().PutAsJsonAsync($"publishers/{id}", new PublisherUpdateDto
        {
            Name = model.Name.Trim(),
            City = model.City,
            StateCode = model.StateCode?.Trim().ToUpperInvariant()
        });

        if (!await AddApiErrorsAsync(response))
        {
            model.PublisherId = id;
            return View(model);
        }

        TempData["CatalogMessage"] = "Publisher updated successfully.";
        return RedirectToAction(nameof(Index));
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

    private async Task<int> GetNextPublisherIdAsync()
    {
        var publishers = await GetDataAsync<IReadOnlyList<PublisherResponseDto>>("publishers") ?? [];
        return publishers.Count == 0 ? 1 : publishers.Max(publisher => publisher.PublisherId) + 1;
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

}
