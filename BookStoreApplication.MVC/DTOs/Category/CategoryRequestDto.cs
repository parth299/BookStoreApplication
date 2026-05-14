using System.ComponentModel.DataAnnotations;
using BookStoreApplication.MVC.Validators.Author;

namespace BookStoreApplication.MVC.DTOs.Category
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category description is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 255 characters")]
        [NoSpecialCharacters]  // Custom Validation Attribute
        public string CatDescription { get; set; } = null!;
    }
}
