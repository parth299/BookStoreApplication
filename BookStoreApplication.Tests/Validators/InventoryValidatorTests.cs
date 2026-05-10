using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Validators;
using FluentValidation.TestHelper;

namespace BookStoreApplication.Tests.Validators
{
    public class InventoryValidatorTests
    {
        private readonly InventoryValidator _validator;

        public InventoryValidatorTests()
        {
            _validator = new InventoryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Rank_Invalid()
        {
            var dto = new InventoryDto
            {
                ISBN = "123",
                ConditionRank = 10
            };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(
                x => x.ConditionRank);
        }

        [Fact]
        public void Should_Not_Have_Error()
        {
            var dto = new InventoryDto
            {
                InventoryId = 1,
                ISBN = "1-111-11111-4",
                ConditionRank = 3,
                Purchased = 0
            };

            var result = _validator.TestValidate(dto);

            foreach (var error in result.Errors)
            {
                Console.WriteLine(
                    $"{error.PropertyName} : {error.ErrorMessage}");
            }

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
