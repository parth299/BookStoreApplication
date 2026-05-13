using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers.Reviewers_Reviews
{
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class ReviewersMvcController : Controller
    {
        private readonly IReviewerService _reviewerService;

        public ReviewersMvcController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _reviewerService.GetAllReviewersAsync();
            return View(result);
        }

        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateReviewerRequestDto());
        }

        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewerRequestDto request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _reviewerService.CreateReviewerAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
