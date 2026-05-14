using FluentValidation;
using BookStoreApplication.MVC.DTOs.Category;

namespace BookStoreApplication.MVC.Validators.Category
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CatDescription).NotEmpty().MaximumLength(255);
        }
    }
}
