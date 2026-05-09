using FluentValidation;
using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CatDescription).NotEmpty().MaximumLength(255);
        }
    }
}