using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
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
    public class AuthorControllerTest
    {
        private readonly Mock<IAuthorService> _mockService;
        private readonly Mock<FileUploadService> _mockFileUploadService;
        private readonly AuthorController _controller;

        public AuthorControllerTest()
        {
            _mockService = new Mock<IAuthorService>();
            
            // Mock FileUploadService dependencies (IWebHostEnvironment + ILogger)
            var mockEnvironment = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<FileUploadService>>();
            _mockFileUploadService = new Mock<FileUploadService>(mockEnvironment.Object, mockLogger.Object);
            
            _controller = new AuthorController(_mockService.Object, _mockFileUploadService.Object);
        }

        // ✅ Test 1: Create valid author
        [Fact]
        public async Task Create_ValidAuthor_ReturnsSuccess()
        {
            var dto = new AuthorRequestDTO { FirstName = "John", LastName = "Doe" };
            var response = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

            var result = await _controller.Create(dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<AuthorResponseDTO>>(okResult.Value);
            Assert.True(apiResponse.Success);
        }

        // ❌ Test 2: Get non-existent author returns 404
        [Fact]
        public async Task GetById_NonExistent_Returns404()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((AuthorResponseDTO?)null);

            var result = await _controller.GetById(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ Test 3: Get author by valid ID
        [Fact]
        public async Task GetById_ExistingAuthor_ReturnsAuthor()
        {
            var response = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<AuthorResponseDTO>>(okResult.Value);
            Assert.Equal(1, apiResponse.Data?.AuthorId);
        }

        // ✅ Test 4: Get all authors with pagination
        [Fact]
        public async Task GetAll_WithPagination_ReturnsPagedResults()
        {
            var filter = new PaginationFilter { PageNumber = 1, PageSize = 10 };
            var authors = new List<AuthorResponseDTO> { new() { AuthorId = 1, FirstName = "John", LastName = "Doe" } };
            _mockService.Setup(s => s.GetAllAsync(filter)).ReturnsAsync(authors);

            var result = await _controller.GetAll(filter);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<AuthorResponseDTO>>>(okResult.Value);
            Assert.Single(apiResponse.Data!);
        }

        // ✅ Test 5: Search authors by name (case-insensitive)
        [Fact]
        public async Task Search_ByName_ReturnsMatchingAuthors()
        {
            var searchTerm = "John";
            var authors = new List<AuthorResponseDTO> { new() { AuthorId = 1, FirstName = "John", LastName = "Doe" } };
            _mockService.Setup(s => s.SearchByNameAsync(searchTerm)).ReturnsAsync(authors);

            var result = await _controller.Search(searchTerm);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<AuthorResponseDTO>>>(okResult.Value);
            Assert.Single(apiResponse.Data!);
        }

        // ✅ Test 6: Update author details
        [Fact]
        public async Task Update_ValidAuthor_ReturnsUpdated()
        {
            var dto = new AuthorRequestDTO { FirstName = "Jane", LastName = "Doe" };
            var response = new AuthorResponseDTO { AuthorId = 1, FirstName = "Jane", LastName = "Doe" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(response);

            var result = await _controller.Update(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<AuthorResponseDTO>>(okResult.Value);
            Assert.Equal("Jane", apiResponse.Data?.FirstName);
        }

        // ❌ Test 7: Update non-existent author returns 404
        [Fact]
        public async Task Update_NonExistent_Returns404()
        {
            var dto = new AuthorRequestDTO { FirstName = "Jane", LastName = "Doe" };
            _mockService.Setup(s => s.UpdateAsync(999, dto)).ReturnsAsync((AuthorResponseDTO ?)null);

            var result = await _controller.Update(999, dto);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ Test 8: Get books by author
        [Fact]
        public async Task GetBooks_ExistingAuthor_ReturnsBooks()
        {
            var response = new AuthorWithBooksResponseDTO
            { 
                AuthorId = 1, 
                FirstName = "John", 
                LastName = "Doe",
                Books = new List<BookSummaryDTO> { new() { Isbn = "123", Title = "Book 1" } }
            };
            _mockService.Setup(s => s.GetByIdWithBooksAsync(1)).ReturnsAsync(response);

            var result = await _controller.GetBooks(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<AuthorWithBooksResponseDTO>>(okResult.Value);
            Assert.Single(apiResponse.Data!.Books);
        }

        // ❌ Test 9: Get books for non-existent author returns 404
        [Fact]
        public async Task GetBooks_NonExistent_Returns404()
        {
            _mockService.Setup(s => s.GetByIdWithBooksAsync(999)).ReturnsAsync((AuthorWithBooksResponseDTO?)null);

            var result = await _controller.GetBooks(999);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ Test 10: Filter authors by country
        [Fact]
        public async Task GetByCountry_ValidCountry_ReturnsAuthors()
        {
            var country = "USA";
            var authors = new List<AuthorResponseDTO> { new() { AuthorId = 1, FirstName = "John", LastName = "Doe" } };
            _mockService.Setup(s => s.GetByCountryAsync(country)).ReturnsAsync(authors);

            var result = await _controller.GetByCountry(country);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<AuthorResponseDTO>>>(okResult.Value);
            Assert.Single(apiResponse.Data!);
        }

        // ❌ Test 11: Create duplicate author throws exception
        [Fact]
        public async Task Create_DuplicateAuthor_ThrowsBadRequest()
        {
            var dto = new AuthorRequestDTO { FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.CreateAsync(dto))
                .ThrowsAsync(new BadRequestException("Author already exists"));

            await Assert.ThrowsAsync<BadRequestException>(() => _controller.Create(dto));
        }

        // ✅ Test 12: Empty search returns empty list
        [Fact]
        public async Task Search_EmptyName_ReturnsEmptyList()
        {
            _mockService.Setup(s => s.SearchByNameAsync("")).ReturnsAsync(new List<AuthorResponseDTO>());

            var result = await _controller.Search("");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<AuthorResponseDTO>>>(okResult.Value);
            Assert.Empty(apiResponse.Data!);
        }

        // ✅ Test 13: Upload photo for existing author
        [Fact]
        public async Task UploadPhoto_ValidFile_ReturnsPath()
        {
            var author = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe", Photo = null };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);
            _mockFileUploadService.Setup(s => s.UploadAuthorPhotoAsync(It.IsAny<IFormFile>(), 1))
                .ReturnsAsync("/uploads/authors/1-20240509120000.jpg");
            _mockService.Setup(s => s.UpdatePhotoAsync(1, "/uploads/authors/1-20240509120000.jpg"))
                .ReturnsAsync(new AuthorResponseDTO { AuthorId = 1, Photo = "/uploads/authors/1-20240509120000.jpg" });

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);

            var result = await _controller.UploadPhoto(1, mockFile.Object);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(apiResponse.Success);
        }

        // ❌ Test 14: Upload photo for non-existent author returns 404
        [Fact]
        public async Task UploadPhoto_NonExistentAuthor_Returns404()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((AuthorResponseDTO?)null);

            var mockFile = new Mock<IFormFile>();
            var result = await _controller.UploadPhoto(999, mockFile.Object);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ Test 15: Delete existing photo
        [Fact]
        public async Task DeletePhoto_ExistingPhoto_ReturnsSuccess()
        {
            var author = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe", Photo = "/uploads/authors/1.jpg" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);
            _mockFileUploadService.Setup(s => s.DeleteAuthorPhoto("/uploads/authors/1.jpg")).Returns(true);
            _mockService.Setup(s => s.UpdatePhotoAsync(1, null)).ReturnsAsync(author);

            var result = await _controller.DeletePhoto(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(apiResponse.Data);
        }

        // ❌ Test 16: Delete photo when author has no photo returns 400
        [Fact]
        public async Task DeletePhoto_NoPhoto_Returns400()
        {
            var author = new AuthorResponseDTO { AuthorId = 1, FirstName = "John", LastName = "Doe", Photo = null };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);

            var result = await _controller.DeletePhoto(1);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}