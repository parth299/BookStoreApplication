using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase;
using BookStoreApplication.Web.Services.Inventory_Cart_Purchase;
using FluentAssertions;
using Moq;

namespace BookStoreApplication.Tests.Services
{
    public class PurchaseLogServiceTests
    {
        private readonly Mock<IShoppingCartRepository> _cartRepoMock;
        private readonly Mock<IPurchaseLogRepository> _purchaseRepoMock;
        private readonly Mock<IInventoryRepository> _inventoryRepoMock;
        private readonly Mock<IReservationService> _reservationMock;
        private readonly Mock<IUserIsExistsRepository> _userRepoMock;
        private readonly PurchaseLogService _service;

        public PurchaseLogServiceTests()
        {
            _cartRepoMock = new Mock<IShoppingCartRepository>();

            _purchaseRepoMock = new Mock<IPurchaseLogRepository>();

            _inventoryRepoMock = new Mock<IInventoryRepository>();

            _reservationMock = new Mock<IReservationService>();

            _userRepoMock = new Mock<IUserIsExistsRepository>();

            _service = new PurchaseLogService(
                _cartRepoMock.Object,
                _inventoryRepoMock.Object,
                _purchaseRepoMock.Object,
                _reservationMock.Object,
                _userRepoMock.Object);
        }

        [Fact]
        public async Task Checkout_Should_Throw_When_Cart_Empty()
        {
            _userRepoMock
                .Setup(x => x.UserExistsAsync(1))
                .ReturnsAsync(true);

            _cartRepoMock
                .Setup(x => x.GetCartAsync(1))
                .ReturnsAsync(new List<Shoppingcart>());

            Func<Task> action =
                async () =>
                    await _service.CheckoutAsync(
                        new PurchaseDto
                        {
                            UserId = 1
                        });

            await action.Should()
                .ThrowAsync<BadRequestException>();
        }
    }
}