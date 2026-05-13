using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Book_Publisher;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class ReviewsController : BaseMvcController
{
    private readonly IBookReviewService _reviews;
    private readonly IReviewerService _reviewers;
    private readonly IBookService _books;

    public ReviewsController(IBookReviewService reviews, IReviewerService reviewers, IBookService books)
    {
        _reviews = reviews;
        _reviewers = reviewers;
        _books = books;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        => View(await _reviews.GetAllReviewsAsync(page, pageSize));

    [AllowAnonymous]
    public async Task<IActionResult> Book(string isbn)
    {
        ViewBag.Isbn = isbn;
        return View(await _reviews.GetBookReviewsByIsbnAsync(isbn));
    }

    [Authorize(Roles = "Admin,Manager,StoreOwner")]
    public async Task<IActionResult> Reviewer(int reviewerId)
    {
        ViewBag.ReviewerId = reviewerId;
        return View(await _reviews.GetBookReviewsByReviewerIdAsync(reviewerId));
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            TempData["Error"] = "Review writing is reserved for guest visitors in this project flow. Logged-in users can browse reviews only.";
            return RedirectToAction(nameof(Index));
        }

        await LoadReviewDropdowns(_books, _reviewers);
        return View(new CreateReviewRequestDto());
    }

    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewRequestDto dto)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            TempData["Error"] = "Only guest visitors can post reviews. Please logout to submit a guest review.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await LoadReviewDropdowns(_books, _reviewers);
            return View(dto);
        }

        await _reviews.CreateReviewAsync(dto);
        TempData["Success"] = "Thanks! Your guest review was submitted.";
        return RedirectToAction(nameof(Index));
    }
}
