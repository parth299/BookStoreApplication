using BookStoreApplication.MVC.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.MVC.Validators.Inventory
{
    public class InventoryPatchValidator: AbstractValidator<InventoryPatchDto>
    {
        public InventoryPatchValidator()
        {
            RuleFor(x => x.Ranks)
                .InclusiveBetween(1, 6)
                .When(x => x.Ranks.HasValue)
                .WithMessage(
                    "Condition rank must be between 1 and 6");

            RuleFor(x => x.Purchased)
                .Must(x => x == 0 || x == 1)
                .When(x => x.Purchased.HasValue)
                .WithMessage(
                    "Purchased must be 0 or 1");
        }
    }
}
