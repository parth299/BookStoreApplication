using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.MVC.Controllers.Reviewers_Reviews
{
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class BookRatingsController : Controller
    {
        private readonly IBookReviewService _bookReviewService;

        public BookRatingsController(IBookReviewService bookReviewService)
        {
            _bookReviewService = bookReviewService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                ModelState.AddModelError(nameof(isbn), "ISBN is required.");
                return View();
            }

            ViewBag.Isbn = isbn;
            var result = await _bookReviewService.GetAverageRatingByIsbnAsync(isbn);
            return View(result);
        }
    }
}
