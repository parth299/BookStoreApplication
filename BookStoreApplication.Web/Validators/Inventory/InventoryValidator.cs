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
                .MaximumLength(20);

            RuleFor(x => x.ConditionRank)
                .InclusiveBetween(1, 6);

            RuleFor(x => x.Purchased)
                .Must(x =>
                    x == 0 || x == 1)
                .WithMessage(
                    "Purchased must be 0 or 1");
        }
    }
}
