using Moq;
using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Services.Author_Category;
using BookStoreApplication.Web.Controllers.Author_Category;

namespace BookStoreApplication.Tests.Controllers
{
    public class AuthorControllerTest
    {
        private readonly Mock<IAuthorService> _mockService;
        private readonly AuthorController _controller;

        public AuthorControllerTest()
        {
            _mockService = new Mock<IAuthorService>();
            _controller = new AuthorController(_mockService.Object, null!);
        }

        // ✅ Test 1: Create valid author returns success
        [Fact]
        public async Task Create_ValidAuthor_ReturnsOk()
        {
            var dto = new AuthorRequestDTO { FirstName = "John", LastName = "Doe" };
            var response = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True(((ApiResponse<AuthorResponseDTO>)okResult.Value!).Success);
        }

        // ❌ Test 2: Get non-existent author returns 404
        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((AuthorResponseDTO?)null);

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ Test 3: Get existing author returns data
        [Fact]
        public async Task GetById_Exists_ReturnsAuthor()
        {
            var response = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = (ApiResponse<AuthorResponseDTO>)okResult.Value!;
            Assert.Equal("John", apiResponse.Data?.FirstName);
        }

        // ❌ Test 4: Update non-existent author returns 404
        [Fact]
        public async Task Update_NotFound_Returns404()
        {
            var dto = new AuthorRequestDTO { FirstName = "Jane", LastName = "Doe" };
            _mockService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((AuthorResponseDTO?)null);

            var result = await _controller.Update(999, dto);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}