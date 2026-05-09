using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.Controllers;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Wrappers;


namespace BookStoreApplication.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        // ✅ Test 13: Create valid category
        [Fact]
        public async Task Create_ValidCategory_ReturnsSuccess()
        {
            var dto = new CategoryRequestDto { CatDescription = "Fiction" };
            var response = new CategoryResponseDto { CatId = 1, CatDescription = "Fiction" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
        }

        // ✅ Test 14: Get all categories
        [Fact]
        public async Task GetAll_ReturnsAllCategories()
        {
            var categories = new List<CategoryResponseDto> 
            { 
                new() { CatId = 1, CatDescription = "Fiction" },
                new() { CatId = 2, CatDescription = "Non-Fiction" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<CategoryResponseDto>>>(okResult.Value);
            Assert.Equal(2, apiResponse.Data!.Count());
        }

        // ✅ Test 15: Update category
        [Fact]
        public async Task Update_ValidCategory_ReturnsUpdated()
        {
            var dto = new CategoryRequestDto { CatDescription = "Science Fiction" };
            var response = new CategoryResponseDto { CatId = 1, CatDescription = "Science Fiction" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

            var result = await _controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CategoryResponseDto>>(okResult.Value);
            Assert.Equal("Science Fiction", apiResponse.Data?.CatDescription);
        }

        // ❌ Test 16: Update non-existent category returns 404
        [Fact]
        public async Task Update_NonExistent_Returns404()
        {
            var dto = new CategoryRequestDto { CatDescription = "Science Fiction" };
            _mockService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((CategoryResponseDto?)null);

            var result = await _controller.Update(999, dto);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ❌ Test 17: Create duplicate category throws exception
        [Fact]
        public async Task Create_DuplicateCategory_ThrowsBadRequest()
        {
            var dto = new CategoryRequestDto { CatDescription = "Fiction" };
            _mockService.Setup(s => s.CreateAsync(dto))
                .ThrowsAsync(new BadRequestException("Category already exists"));

            await Assert.ThrowsAsync<BadRequestException>(() => _controller.Create(dto));
        }
    }
}