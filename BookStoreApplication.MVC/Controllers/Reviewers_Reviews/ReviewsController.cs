using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers.Reviewers_Reviews
{
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class ReviewsController : Controller
    {
        private readonly IBookReviewService _bookReviewService;

        public ReviewsController(IBookReviewService bookReviewService)
        {
            _bookReviewService = bookReviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            var result = await _bookReviewService.GetAllReviewsAsync(page, pageSize);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Book(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                return RedirectToAction(nameof(Index));

            ViewBag.Isbn = isbn;
            var result = await _bookReviewService.GetBookReviewsByIsbnAsync(isbn);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchBook(string isbn)
        {
            return RedirectToAction(nameof(Book), new { isbn });
        }

        [HttpGet]
        public async Task<IActionResult> Reviewer(int reviewerId)
        {
            ViewBag.ReviewerId = reviewerId;
            var result = await _bookReviewService.GetBookReviewsByReviewerIdAsync(reviewerId);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchReviewer(int reviewerId)
        {
            return RedirectToAction(nameof(Reviewer), new { reviewerId });
        }

        [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
        [HttpGet]
        public IActionResult Create(string? isbn = null)
        {
            var model = new CreateReviewRequestDto();
            if (!string.IsNullOrWhiteSpace(isbn))
                model.Isbn = isbn;

            return View(model);
        }

        [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewRequestDto request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _bookReviewService.CreateReviewAsync(request);
            return RedirectToAction(nameof(Book), new { isbn = request.Isbn });
        }

        [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
        [HttpGet]
        public IActionResult Edit(string isbn, int reviewerId)
        {
            if (string.IsNullOrWhiteSpace(isbn) || reviewerId <= 0)
                return RedirectToAction(nameof(Index));

            ViewBag.Isbn = isbn;
            ViewBag.ReviewerId = reviewerId;
            return View(new UpdateReviewRequestDto());
        }

        [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string isbn, int reviewerId, UpdateReviewRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Isbn = isbn;
                ViewBag.ReviewerId = reviewerId;
                return View(request);
            }

            var result = await _bookReviewService.UpdateReviewAsync(isbn, reviewerId, request);
            if (result == null)
                return NotFound("Review not found for this ISBN and ReviewerId.");

            return RedirectToAction(nameof(Book), new { isbn });
        }
    }
}
