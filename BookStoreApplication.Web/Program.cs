using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Middleware;
using BookStoreApplication.Web.Repositories;    
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Validators;
using BookStoreApplication.Web.Mapping;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDB")));

// Controllers with Global Filters (Validation + Exception Handling)
builder.Services.AddControllers(options =>
{
    // Add global filters - applied to ALL controller actions
    options.Filters.Add<ValidationFilter>();      // Validates ModelState automatically
    options.Filters.Add<ApiExceptionFilter>();   // Handles exceptions at action level
})
.ConfigureApiBehaviorOptions(options =>
{
    // Disable automatic 400 response so our ValidationFilter can handle it
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation - Manual registration (WORKS EVERYWHERE)
builder.Services.AddScoped<IValidator<AuthorRequestDTO>, AuthorValidator>();
builder.Services.AddScoped<IValidator<CategoryRequestDto>, CategoryValidator>();

// Register custom Action Filters for [ServiceFilter] attribute usage
builder.Services.AddScoped<LogActionFilter>();

// Repositories
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<FileUploadService>();  // File upload service

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();  // Serve files from wwwroot (uploaded photos)
app.MapControllers();

app.Run();