using BookStoreApplication.MVC.Configuration;
using BookStoreApplication.MVC.DTOs.Author;
using BookStoreApplication.MVC.DTOs.Category;
using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services;
using BookStoreApplication.MVC.Services.Api;
using BookStoreApplication.MVC.Services.Author_Category;
using BookStoreApplication.MVC.Services.Book_Publisher;
using BookStoreApplication.MVC.Services.Interfaces;
using BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using BookStoreApplication.MVC.Services.User;
using BookStoreApplication.MVC.Validators.Author;
using BookStoreApplication.MVC.Validators.Category;
using BookStoreApplication.MVC.Validators.Inventory;
using BookStoreApplication.MVC.Validators.RatingAndReviewers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings")
);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ApiClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["ApiSettings:BaseUrl"] ?? "https://localhost:7001/";

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient("BookStoreApi", client =>
{
    var baseUrl = builder.Configuration["BookStoreApi:BaseUrl"]
                  ?? "http://localhost:5048/";

    client.BaseAddress = new Uri(
        baseUrl.EndsWith('/')
            ? baseUrl
            : $"{baseUrl}/"
    );
});

builder.Services.AddHttpClient<IUserService, UserService>(
    (sp, client) =>
    {
        var settings = sp
            .GetRequiredService<IOptions<ApiSettings>>()
            .Value;

        client.BaseAddress = new Uri(settings.BaseUrl);
    });

builder.Services.AddMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<InventoryValidator>();

builder.Services.AddScoped<IValidator<AuthorRequestDTO>, AuthorValidator>();
builder.Services.AddScoped<IValidator<CategoryRequestDto>, CategoryValidator>();
builder.Services.AddScoped<IValidator<CreateReviewRequestDto>, CreateReviewRequestDtoValidator>();
builder.Services.AddScoped<IValidator<CreateReviewerRequestDto>, CreateReviewerRequestDtoValidator>();

// API-backed MVC services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IPurchaseLogService, PurchaseLogService>();
builder.Services.AddScoped<IBookReviewService, BookReviewService>();
builder.Services.AddScoped<IReviewerService, ReviewerService>();
builder.Services.AddSingleton<IReservationService, ReservationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();