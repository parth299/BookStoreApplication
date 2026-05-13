using System.ComponentModel.DataAnnotations;
using BookStoreApplication.MVC.Validators.Author;

namespace BookStoreApplication.MVC.DTOs.Author
{
    public class AuthorRequestDTO
    {
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 100 characters")]
        [NoSpecialCharacters]  // Custom Validation Attribute
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 100 characters")]
        [NoSpecialCharacters]  // Custom Validation Attribute
        public string FirstName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Photo path must not exceed 500 characters")]
        // Note: Can be external URL (/uploads/authors/123.jpg) or external URL
        public string? Photo { get; set; }
    }
}
