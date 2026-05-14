using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreApplication.MVC.ViewModels;

public class FormSelectLists
{
    public List<SelectListItem> Books { get; set; } = [];
    public List<SelectListItem> Publishers { get; set; } = [];
    public List<SelectListItem> Categories { get; set; } = [];
    public List<SelectListItem> Reviewers { get; set; } = [];
    public List<SelectListItem> Conditions { get; set; } = [];
}
