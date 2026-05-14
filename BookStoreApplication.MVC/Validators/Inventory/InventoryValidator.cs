using BookStoreApplication.MVC.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.MVC.Validators.Inventory
{
    public class InventoryValidator : AbstractValidator<InventoryDto>
    {
        public InventoryValidator()
        {
            RuleFor(x => x.ISBN)
     .NotEmpty()
     .MaximumLength(13)
     .Matches(@"^[0-9]{1}-[0-9]{3}-[0-9]{5}-[0-9]{1}$")
     .WithMessage("ISBN format must be like 1-111-11111-1");

            RuleFor(x => x.ConditionRank)
                .InclusiveBetween(1, 6);

            RuleFor(x => x.Purchased)
                .Must(x => x == 0 || x == 1)
                .When(x => x.Purchased.HasValue)
                .WithMessage(
                    "Purchased must be 0 or 1");
        }
    }
}
