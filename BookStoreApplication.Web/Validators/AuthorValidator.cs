using FluentValidation;
using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Validators
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