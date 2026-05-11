using BookStoreApplication.Web.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.Inventory
{
    public class InventoryValidator: AbstractValidator<InventoryDto>
    {
        public InventoryValidator()
        {
            RuleFor(x => x.ISBN)
                .NotEmpty()
                .MaximumLength(13)
                .Matches(@"^[0-9\-]+$")
                .WithMessage("ISBN can contain only digits and hyphens");

            RuleFor(x => x.ConditionRank)
                .InclusiveBetween(1, 6);

            RuleFor(x => x.Purchased)
                .Must(x =>x == 0 || x == 1)
                .When(x => x.Purchased.HasValue)
                .WithMessage(
                    "Purchased must be 0 or 1");
        }
    }
}
