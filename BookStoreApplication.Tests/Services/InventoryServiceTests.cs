using AutoMapper;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;
using BookStoreApplication.Web.Services;
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

            _service = new InventoryService( _repositoryMock.Object,  _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Inventory()
        {
            var dto = new InventoryDto
            {
                ISBN = "9781234567890",
                ConditionRank = 3,
                Purchased = 0
            };

            _repositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Inventory>()))
                .Callback<Inventory>(
                    x => x.InventoryId = 1)
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

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
            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Inventory?)null);

            Func<Task> action =
                async () => await _service.GetByIdAsync(1);

            await action.Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Inventory()
        {
            var entity = new Inventory
            {
                InventoryId = 1,
                Isbn = "123",
                Ranks = 2
            };

            var dto = new InventoryDto
            {
                ConditionRank = 5
            };

            _repositoryMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(entity);

            await _service.UpdateAsync(1, dto);

            entity.Ranks.Should().Be(5);

            _repositoryMock.Verify(
                x => x.Update(entity),
                Times.Once);
        }
    }
}
