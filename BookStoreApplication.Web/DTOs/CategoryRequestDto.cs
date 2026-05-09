using System.ComponentModel.DataAnnotations;
using BookStoreApplication.Web.Validators;

namespace BookStoreApplication.Web.DTOs
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category description is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 255 characters")]
        [NoSpecialCharacters]  // Custom Validation Attribute
        public string CatDescription { get; set; } = null!;
    }
}