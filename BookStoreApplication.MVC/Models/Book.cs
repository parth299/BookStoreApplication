namespace BookStoreApplication.MVC.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<Bookreview> Bookreviews { get; set; } = new List<Bookreview>();
}
