using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;
using BookStoreApplication.Web.Services;
using FluentAssertions;
using Moq;

namespace BookStoreApplication.Tests.Services
{
    public class ShoppingCartServiceTests
    {
        private readonly Mock<IShoppingCartRepository> _cartRepoMock;
        private readonly Mock<IInventoryRepository> _inventoryRepoMock;
        private readonly Mock<IReservationService> _reservationMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly ShoppingCartService _service;

        public ShoppingCartServiceTests()
        {
            _cartRepoMock = new Mock<IShoppingCartRepository>();

            _inventoryRepoMock = new Mock<IInventoryRepository>();

            _reservationMock = new Mock<IReservationService>();

            _userRepoMock = new Mock<IUserRepository>();

            _service = new ShoppingCartService(
                _cartRepoMock.Object,
                _inventoryRepoMock.Object,
                _reservationMock.Object,
                _userRepoMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Add_To_Cart()
        {
            var dto = new CartItemDto
            {
                UserId = 1,
                ISBN = "123",
                Quantity = 1
            };

            _userRepoMock
                .Setup(x => x.UserExistsAsync(1))
                .ReturnsAsync(true);

            _inventoryRepoMock
                .Setup(x => x.GetAvailableByISBNAsync("123"))
                .ReturnsAsync(new Inventory
                {
                    InventoryId = 1,
                    Isbn = "123",
                    Purchased = 0
                });

            _reservationMock
                .Setup(x => x.IsReserved(1))
                .Returns(false);

            await _service.AddAsync(dto);

            _cartRepoMock.Verify(
                x => x.AddAsync(It.IsAny<Shoppingcart>()),
                Times.Once);
        }

        [Fact]
        public async Task AddAsync_Should_Throw_When_OutOfStock()
        {
            var dto = new CartItemDto
            {
                UserId = 1,
                ISBN = "123"
            };

            _userRepoMock
                .Setup(x => x.UserExistsAsync(1))
                .ReturnsAsync(true);

            _inventoryRepoMock
                .Setup(x => x.GetAvailableByISBNAsync("123"))
                .ReturnsAsync((Inventory?)null);

            Func<Task> action =
                async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task AddAsync_Should_Throw_When_Duplicate()
        {
            var dto = new CartItemDto
            {
                UserId = 1,
                ISBN = "123"
            };

            _userRepoMock
                .Setup(x => x.UserExistsAsync(1))
                .ReturnsAsync(true);

            _inventoryRepoMock
                .Setup(x => x.GetAvailableByISBNAsync("123"))
                .ReturnsAsync(new Inventory
                {
                    InventoryId = 1
                });

            _reservationMock
                .Setup(x => x.IsReserved(1))
                .Returns(false);

            _cartRepoMock
                .Setup(x => x.GetItemAsync(1, "123"))
                .ReturnsAsync(new Shoppingcart());

            Func<Task> action =
                async () => await _service.AddAsync(dto);

            await action.Should()
                .ThrowAsync<BadRequestException>();
        }
    }
}
