using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Middleware;
using BookStoreApplication.Web.Repositories.Implementations;
using BookStoreApplication.Web.Repositories.Interfaces;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// ======================================================
// SERILOG
// ======================================================

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval:
        RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// ======================================================
// DB CONTEXT
// ======================================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDB")));


// ======================================================
// AUTOMAPPER
// ======================================================

builder.Services.AddAutoMapper(
    AppDomain.CurrentDomain
        .GetAssemblies());


// ======================================================
// MEMORY CACHE
// ======================================================

builder.Services.AddMemoryCache();


// ======================================================
// REPOSITORIES
// ======================================================

builder.Services.AddScoped<
    IInventoryRepository,
    InventoryRepository>();

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IShoppingCartRepository,
    ShoppingCartRepository>();

builder.Services.AddScoped<
    IPurchaseLogRepository,
    PurchaseLogRepository>();


// ======================================================
// SERVICES
// ======================================================

builder.Services.AddScoped<
    IInventoryService,
    InventoryService>();

builder.Services.AddScoped<
    IShoppingCartService,
    ShoppingCartService>();

builder.Services.AddScoped<
    IPurchaseLogService,
    PurchaseLogService>();

builder.Services.AddSingleton<
    IReservationService,
    ReservationService>();


// ======================================================
// FLUENT VALIDATION
// ======================================================

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        InventoryValidator>();


// ======================================================
// JWT AUTH
// ======================================================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults
            .AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

builder.Services.AddAuthorization();


// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// ======================================================
// BUILD
// ======================================================

var app = builder.Build();


// ======================================================
// MIDDLEWARE
// ======================================================

app.UseMiddleware<
    ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

