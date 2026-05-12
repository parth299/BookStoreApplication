using FluentValidation;
using BookStoreApplication.Web.DTOs.Category;

namespace BookStoreApplication.Web.Validators.Category
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CatDescription).NotEmpty().MaximumLength(255);
        }
    }
}
