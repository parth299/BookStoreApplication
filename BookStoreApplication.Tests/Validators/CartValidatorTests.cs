using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Validators;
using FluentValidation.TestHelper;

namespace BookStoreApplication.Tests.Validators
{
    public class CartValidatorTests
    {
        private readonly CartValidator _validator;

        public CartValidatorTests()
        {
            _validator = new CartValidator();
        }

        [Fact]
        public void Should_Have_Error_When_User_Invalid()
        {
            var dto = new CartItemDto
            {
                UserId = 0,
                ISBN = "123",
                Quantity = 1
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(
                x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_Quantity_Invalid()
        {
            var dto = new CartItemDto
            {
                UserId = 1,
                ISBN = "123",
                Quantity = 0
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(
                x => x.Quantity);
        }
    }
}
