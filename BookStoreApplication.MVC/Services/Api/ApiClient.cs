using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using BookStoreApplication.MVC.Configuration;

namespace BookStoreApplication.MVC.Services.Api;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiClient(HttpClient http, IHttpContextAccessor httpContextAccessor, IOptions<ApiSettings> settings)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
        var baseUrl = settings.Value.BaseUrl;
        if (!string.IsNullOrWhiteSpace(baseUrl) && _http.BaseAddress == null)
        {
            _http.BaseAddress = new Uri(baseUrl);
        }
    }

    private void AttachToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var token = httpContext?.Session.GetString("JWToken")
            ?? httpContext?.User.FindFirst("JWToken")?.Value;
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            AttachToken();
            using var response = await _http.GetAsync(url);
            return await ReadContentAsync<T>(response);
        }
        catch
        {
            return default;
        }
    }

    public async Task<List<T>> GetListAsync<T>(string url)
    {
        var result = await GetAsync<List<T>>(url);
        return result ?? new List<T>();
    }

    public async Task<T?> PostAsync<T>(string url, object body)
    {
        try
        {
            AttachToken();
            using var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
            return await ReadContentAsync<T>(response);
        }
        catch
        {
            return default;
        }
    }

    public async Task<T?> PutAsync<T>(string url, object body)
    {
        try
        {
            AttachToken();
            using var response = await _http.PutAsJsonAsync(url, body, JsonOptions);
            return await ReadContentAsync<T>(response);
        }
        catch
        {
            return default;
        }
    }

    public async Task<bool> DeleteAsync(string url)
    {
        try
        {
            AttachToken();
            using var response = await _http.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PatchAsync(string url, object body)
    {
        try
        {
            AttachToken();
            var json = JsonSerializer.Serialize(body, JsonOptions);
            using var request = new HttpRequestMessage(HttpMethod.Patch, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            using var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<T?> ReadContentAsync<T>(HttpResponseMessage response)
    {
        var text = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(text)) return default;

        using var doc = JsonDocument.Parse(text);
        var root = doc.RootElement;

        // API usually returns { success, message, data, errors, statusCode }
        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("data", out var data))
        {
            if (typeof(T).Name.StartsWith("ApiResponse"))
            {
                return JsonSerializer.Deserialize<T>(text, JsonOptions);
            }

            if (data.ValueKind == JsonValueKind.Null) return default;
            return JsonSerializer.Deserialize<T>(data.GetRawText(), JsonOptions);
        }

        return JsonSerializer.Deserialize<T>(text, JsonOptions);
    }
}
