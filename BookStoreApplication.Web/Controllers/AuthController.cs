using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;

namespace BookStoreApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly FileUploadService _fileUploadService;

        public AuthorController(IAuthorService service, FileUploadService fileUploadService)
        {
            _service = service;
            _fileUploadService = fileUploadService;
        }

        // POST /api/authors
        [HttpPost]
        [ServiceFilter(typeof(LogActionFilter))]  // Custom Action Filter - logs execution time
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> Create(AuthorRequestDTO dto)
        {
            // Manual ModelState validation example (Local Validation Handling)
            // The ValidationFilter (Global) also does this, but this shows explicit local handling
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse.Fail<AuthorResponseDTO>("Validation failed", errors));
            }

            var result = await _service.CreateAsync(dto);
            return Ok(ApiResponse.Success(result, "Author created"));
        }

        // GET /api/authors (with pagination)
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetAll(
            [FromQuery] PaginationFilter filter)
        {
            var result = await _service.GetAllAsync(filter);
            return Ok(ApiResponse.Success(result));
        }

        // GET /api/authors/{id}
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse.Fail<AuthorResponseDTO>($"Author {id} not found"));
            return Ok(ApiResponse.Success(result));
        }

        // PUT /api/authors/{id}
        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> Update(int id, AuthorRequestDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(ApiResponse.Fail<AuthorResponseDTO>($"Author {id} not found"));
            return Ok(ApiResponse.Success(result, "Author updated"));
        }

        // GET /api/authors/search?name=xxx
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> Search(
            [FromQuery, RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers and spaces")] string name)
        {
            var result = await _service.SearchByNameAsync(name);
            return Ok(ApiResponse.Success(result));
        }

        // GET /api/authors/{id}/books
        [HttpGet("{id:int:min(1)}/books")]
        public async Task<ActionResult<ApiResponse<AuthorWithBooksResponseDTO>>> GetBooks(int id)
        {
            var result = await _service.GetByIdWithBooksAsync(id);
            if (result == null)
                return NotFound(ApiResponse.Fail<AuthorWithBooksResponseDTO>($"Author {id} not found"));
            return Ok(ApiResponse.Success(result));
        }

        // GET /api/authors/country/{country}
        [HttpGet("country/{country:alpha:minlength(2):maxlength(50)}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetByCountry(string country)
        {
            var result = await _service.GetByCountryAsync(country);
            return Ok(ApiResponse.Success(result));
        }

        // POST /api/authors/{id}/photo - Upload author photo
        [HttpPost("{id:int:min(1)}/photo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<string>>> UploadPhoto(int id, IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest(ApiResponse.Fail<string>("No photo file provided"));

            // Verify author exists
            var author = await _service.GetByIdAsync(id);
            if (author == null)
                return NotFound(ApiResponse.Fail<string>($"Author {id} not found"));

            try
            {
                // Upload file and get relative path
                var filePath = await _fileUploadService.UploadAuthorPhotoAsync(photo, id);
                
                // Delete old photo if exists
                if (!string.IsNullOrEmpty(author.Photo))
                {
                    _fileUploadService.DeleteAuthorPhoto(author.Photo);
                }
                
                // Update author's Photo field in database
                var updatedAuthor = await _service.UpdatePhotoAsync(id, filePath);
                
                return Ok(ApiResponse.Success(filePath, "Photo uploaded and saved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.Fail<string>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.Fail<string>(ex.Message));
            }
        }

        // DELETE /api/authors/{id}/photo - Delete author photo
        [HttpDelete("{id:int:min(1)}/photo")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePhoto(int id)
        {
            var author = await _service.GetByIdAsync(id);
            if (author == null)
                return NotFound(ApiResponse.Fail<bool>($"Author {id} not found"));

            if (string.IsNullOrEmpty(author.Photo))
                return BadRequest(ApiResponse.Fail<bool>("No photo to delete"));

            var deleted = _fileUploadService.DeleteAuthorPhoto(author.Photo);
            
            // Clear Photo field from database
            await _service.UpdatePhotoAsync(id, null);
            
            return Ok(ApiResponse.Success(deleted, deleted ? "Photo deleted" : "Photo not found on disk"));
        }
    }
}