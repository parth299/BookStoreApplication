using System.ComponentModel.DataAnnotations;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.DTOs.Category;

namespace BookStoreApplication.MVC.Models;

public class BookCatalogIndexViewModel
{
    public IReadOnlyList<BookResponseDto> Books { get; set; } = [];
    public IReadOnlyList<PublisherResponseDto> Publishers { get; set; } = [];
    public IReadOnlyList<CategoryResponseDto> Categories { get; set; } = [];
    public BookFilterViewModel Filter { get; set; } = new();
    public bool CanManageCatalog { get; set; }
    public string? Notice { get; set; }
}

public class BookFilterViewModel
{
    [StringLength(70)]
    public string? Title { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Category must be greater than 0.")]
    public int? Category { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Publisher is required.")]
    public int? PublisherId { get; set; }

}

public class BookFormViewModel
{
    [Required]
    [RegularExpression(@"^[0-9]{1}-[0-9]{3}-[0-9]{5}-[0-9]{1}$", ErrorMessage = "ISBN format must be like 1-111-11111-1.")]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    [StringLength(70)]
    public string Title { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Category must be greater than 0.")]
    public int? Category { get; set; }

    [StringLength(30)]
    public string? Edition { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Publisher is required.")]
    public int PublisherId { get; set; }

    public IReadOnlyList<PublisherResponseDto> Publishers { get; set; } = [];
    public IReadOnlyList<CategoryResponseDto> Categories { get; set; } = [];
}

public class BookDetailViewModel
{
    public BookResponseDto Book { get; set; } = new();
    public string CategoryName { get; set; } = "Not assigned";
    public bool CanManageCatalog { get; set; }
}

public class PublisherIndexViewModel
{
    public IReadOnlyList<PublisherResponseDto> Publishers { get; set; } = [];
    public bool CanManageCatalog { get; set; }
    public string? SearchName { get; set; }
}

public class PublisherFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Publisher id must be greater than 0.")]
    public int PublisherId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(30)]
    public string? City { get; set; }

    [StringLength(2, MinimumLength = 2, ErrorMessage = "State code must be exactly 2 characters.")]
    public string? StateCode { get; set; }
}
