using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers;

public class ReviewerController : Controller
{
    private readonly IReviewerService _reviewerService;

    public ReviewerController(IReviewerService reviewerService)
    {
        _reviewerService = reviewerService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var reviewers = await _reviewerService.GetAllReviewersAsync();
            return View(reviewers);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View(new List<ReviewerDto>());
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateReviewerRequestDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReviewerRequestDto request)
    {
        if (!ModelState.IsValid)
            return View(request);

        try
        {
            await _reviewerService.CreateReviewerAsync(request);
            TempData["SuccessMessage"] = "Reviewer created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }
}
