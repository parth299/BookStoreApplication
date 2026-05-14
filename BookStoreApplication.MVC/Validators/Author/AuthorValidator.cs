using FluentValidation;
using BookStoreApplication.MVC.DTOs.Author;

namespace BookStoreApplication.MVC.Validators.Author
{
    public class AuthorValidator : AbstractValidator<AuthorRequestDTO>
    {
        public AuthorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}
