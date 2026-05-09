using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Middleware;
using BookStoreApplication.Web.Repositories.ReviewAndRatings;
using BookStoreApplication.Web.Services.ReviewsAndRatings;
using BookStoreApplication.Web.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookDB")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookReviewRepository, BookReviewRepository>();
builder.Services.AddScoped<IBookReviewService, BookReviewService>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
builder.Services.AddScoped<IReviewerService, ReviewerService>();

builder.Services.AddScoped<IValidator<CreateReviewRequestDto>, CreateReviewRequestDtoValidator>();
builder.Services.AddScoped<IValidator<CreateReviewerRequestDto>, CreateReviewerRequestDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
