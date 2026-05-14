using BookStoreApplication.MVC.Services.Book_Publisher;
using BookStoreApplication.MVC.Services.Author_Category;
using BookStoreApplication.MVC.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreApplication.MVC.Controllers;

public abstract class BaseMvcController : Controller
{
    protected async Task LoadBookDropdowns(IBookService books, ICategoryService categories, IPublisherService publishers)
    {
        ViewBag.Books = (await books.GetAllBooksAsync()).Select(b => new SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn)).ToList();
        ViewBag.Categories = (await categories.GetAllAsync()).Select(c => new SelectListItem(c.CatDescription, c.CatId.ToString())).ToList();
        ViewBag.Publishers = (await publishers.GetAllAsync()).Select(p => new SelectListItem(p.Name, p.PublisherId.ToString())).ToList();
    }

    protected async Task LoadReviewDropdowns(IBookService books, IReviewerService reviewers)
    {
        ViewBag.Books = (await books.GetAllBooksAsync()).Select(b => new SelectListItem($"{b.Title} ({b.Isbn})", b.Isbn)).ToList();
        ViewBag.Reviewers = (await reviewers.GetAllReviewersAsync()).Select(r => new SelectListItem($"{r.Name} ({r.ReviewerId})", r.ReviewerId.ToString())).ToList();
    }

    protected void LoadConditionDropdown()
    {
        ViewBag.Conditions = Enumerable.Range(1, 5).Select(i => new SelectListItem($"Condition {i}", i.ToString())).ToList();
        ViewBag.PurchasedOptions = new List<SelectListItem> { new("Available", "0"), new("Purchased", "1") };
    }
}
