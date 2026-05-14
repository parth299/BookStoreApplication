using BookStoreApplication.MVC.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.MVC.Validators.Cart
{
    public class PurchaseValidator: AbstractValidator<PurchaseDto>
    {
        public PurchaseValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);
        }
    }
}
