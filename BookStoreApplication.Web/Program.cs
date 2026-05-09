using BookStoreApplication.Web.Configurations;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.Mapping;
using BookStoreApplication.Web.Middleware;
using BookStoreApplication.Web.Middleware.RatingAndReviewers;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories;
using BookStoreApplication.Web.Repositories.Implementations;
using BookStoreApplication.Web.Repositories.Interfaces;
using BookStoreApplication.Web.Repositories.ReviewAndRatings;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Services.ReviewsAndRatings;
using BookStoreApplication.Web.services;
using BookStoreApplication.Web.Validators;
using BookStoreApplication.Web.Validators.RatingAndReviewers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BookDB")));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAutoMapper(
    typeof(MappingProfile));

builder.Services.AddMemoryCache();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();

    options.Filters.Add<ApiExceptionFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<
    IAuthorRepository,
    AuthorRepository>();

builder.Services.AddScoped<
    ICategoryRepository,
    CategoryRepository>();

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

builder.Services.AddScoped<
    IBookReviewRepository,
    BookReviewRepository>();

builder.Services.AddScoped<
    IReviewerRepository,
    ReviewerRepository>();

builder.Services.AddScoped<UserReporitory>();

builder.Services.AddScoped<PermRoleRepository>();

builder.Services.AddScoped<
    IAuthorService,
    AuthorService>();

builder.Services.AddScoped<
    ICategoryService,
    CategoryService>();

builder.Services.AddScoped<
    IInventoryService,
    InventoryService>();

builder.Services.AddScoped<
    IShoppingCartService,
    ShoppingCartService>();

builder.Services.AddScoped<
    IPurchaseLogService,
    PurchaseLogService>();

builder.Services.AddScoped<
    IBookReviewService,
    BookReviewService>();

builder.Services.AddScoped<
    IReviewerService,
    ReviewerService>();

builder.Services.AddSingleton<
    IReservationService,
    ReservationService>();

builder.Services.AddScoped<
    IUserService,
    UserService>();

builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();

builder.Services.AddScoped<
    FileUploadService>();

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        InventoryValidator>();

builder.Services
    .AddValidatorsFromAssemblyContaining<
        RegisterUserDTO>();

builder.Services.AddScoped<
    IValidator<AuthorRequestDTO>,
    AuthorValidator>();

builder.Services.AddScoped<
    IValidator<CategoryRequestDto>,
    CategoryValidator>();

builder.Services.AddScoped<
    IValidator<CreateReviewRequestDto>,
    CreateReviewRequestDtoValidator>();

builder.Services.AddScoped<
    IValidator<CreateReviewerRequestDto>,
    CreateReviewerRequestDtoValidator>();

builder.Services.AddScoped<LogActionFilter>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookStore",
        Version = "v1"
    });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter 'Bearer' followed by your JWT token."
        });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseExceptionHandlingMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();