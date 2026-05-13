using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class ReviewController : Controller
{
    private readonly IBookReviewService _bookReviewService;

    public ReviewController(IBookReviewService bookReviewService)
    {
        _bookReviewService = bookReviewService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        try
        {
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var reviews = await _bookReviewService.GetAllReviewsAsync(page, pageSize);
            return View(reviews);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View(new List<BookReviewDto>());
        }
    }

    [HttpGet]
    public IActionResult Create(string? isbn)
    {
        return View(new CreateReviewRequestDto { Isbn = isbn ?? string.Empty });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewRequestDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _bookReviewService.CreateReviewAsync(request);
            TempData["SuccessMessage"] = "Review created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Book(string isbn)
    {
        try
        {
            ViewBag.Isbn = isbn;
            var reviews = await _bookReviewService.GetBookReviewsByIsbnAsync(isbn);
            return View(reviews);
        }
        catch (Exception ex)
        {
            ViewBag.Isbn = isbn;
            ViewBag.ErrorMessage = ex.Message;
            return View(new List<BookReviewDto>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Reviewer(int reviewerId)
    {
        try
        {
            ViewBag.ReviewerId = reviewerId;
            var reviews = await _bookReviewService.GetBookReviewsByReviewerIdAsync(reviewerId);
            return View(reviews);
        }
        catch (Exception ex)
        {
            ViewBag.ReviewerId = reviewerId;
            ViewBag.ErrorMessage = ex.Message;
            return View(new List<BookReviewDto>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> AverageRating(string isbn)
    {
        try
        {
            var rating = await _bookReviewService.GetAverageRatingByIsbnAsync(isbn);
            return View(rating);
        }
        catch (Exception ex)
        {
            ViewBag.Isbn = isbn;
            ViewBag.ErrorMessage = ex.Message;
            return View(null);
        }
    }

    [HttpGet]
    public IActionResult Edit(string isbn, int reviewerId, int? rating, string? comments)
    {
        ViewBag.Isbn = isbn;
        ViewBag.ReviewerId = reviewerId;
        return View(new UpdateReviewRequestDto
        {
            Rating = rating ?? 1,
            Comments = comments
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string isbn, int reviewerId, UpdateReviewRequestDto request)
    {
        ViewBag.Isbn = isbn;
        ViewBag.ReviewerId = reviewerId;

        if (!ModelState.IsValid)
            return View(request);

        try
        {
            var result = await _bookReviewService.UpdateReviewAsync(isbn, reviewerId, request);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Review not found for this ISBN and ReviewerId.");
                return View(request);
            }

            TempData["SuccessMessage"] = "Review updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }
}
