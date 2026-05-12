using AutoMapper;
using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase;
using BookStoreApplication.Web.Services.Inventory_Cart_Purchase;
using FluentAssertions;
using Moq;

namespace BookStoreApplication.Tests.Services
{
    public class InventoryServiceTests
    {
        private readonly Mock<IInventoryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly InventoryService _service;

        public InventoryServiceTests()
        {
            _repositoryMock = new Mock<IInventoryRepository>();

            _mapperMock = new Mock<IMapper>();

            _service = new InventoryService(
                _repositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Inventory()
        {
            // Arrange
            var dto = new InventoryDto
            {
                ISBN = "9781234567890",
                ConditionRank = 3,
                Purchased = 0
            };

            var entity = new Inventory
            {
                InventoryId = 1,
                Isbn = dto.ISBN,
                Ranks = dto.ConditionRank,
                Purchased = dto.Purchased
            };

            _mapperMock
                .Setup(x => x.Map<Inventory>(dto))
                .Returns(entity);

            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Inventory>()))
                .Callback<Inventory>(x => x.InventoryId = 1)
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().Be(1);

            _repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Inventory>()),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveAsync(),
                Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_When_NotFound()
        {
            // Arrange
            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Inventory?)null);

            // Act
            Func<Task> action =
                async () => await _service.GetByIdAsync(1);

            // Assert
            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Inventory()
        {
            // Arrange
            var entity = new Inventory
            {
                InventoryId = 1,
                Isbn = "123",
                Ranks = 2
            };

            var dto = new InventoryDto
            {
                ISBN = "123",
                ConditionRank = 5,
                Purchased = 0
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            _repositoryMock
                .Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(1, dto);

            // Assert
            entity.Ranks.Should().Be(5);

            _repositoryMock.Verify(
                x => x.Update(entity),
                Times.Once);

            _repositoryMock.Verify(
                x => x.SaveAsync(),
                Times.Once);
        }
    }
}