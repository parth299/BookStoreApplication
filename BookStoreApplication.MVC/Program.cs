using BookStoreApplication.MVC.Data;
using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Repositories.ReviewAndRatings;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using BookStoreApplication.MVC.Validators.RatingAndReviewers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDB")));

builder.Services.AddScoped<IBookReviewRepository, BookReviewRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
builder.Services.AddScoped<IBookReviewService, BookReviewService>();
builder.Services.AddScoped<IReviewerService, ReviewerService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CreateReviewRequestDto>, CreateReviewRequestDtoValidator>();
builder.Services.AddScoped<IValidator<CreateReviewerRequestDto>, CreateReviewerRequestDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateReviewRequestDto>, UpdateReviewRequestDtoValidator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Review}/{action=Index}/{id?}");

app.Run();
