using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Services.Author;

namespace BookStoreApplication.Web.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;
        private readonly FileUploadService _fileUploadService;

        public AuthorController(
            IAuthorService service,
            FileUploadService fileUploadService)
        {
            _service = service;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> Create(
            AuthorRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(
                    ApiResponse<AuthorResponseDTO>
                        .FailResponse("Validation failed", errors));
            }

            var result = await _service.CreateAsync(dto);

            return Ok(
                ApiResponse<AuthorResponseDTO>
                    .SuccessResponse(result, "Author created"));
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetAll(
            [FromQuery] PaginationFilter filter)
        {
            var result = await _service.GetAllAsync(filter);

            return Ok(
                ApiResponse<IEnumerable<AuthorResponseDTO>>
                    .SuccessResponse(result));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> GetById(
            int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<AuthorResponseDTO>
                        .FailResponse($"Author {id} not found"));
            }

            return Ok(
                ApiResponse<AuthorResponseDTO>
                    .SuccessResponse(result));
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> Update(
            int id,
            AuthorRequestDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<AuthorResponseDTO>
                        .FailResponse($"Author {id} not found"));
            }

            return Ok(
                ApiResponse<AuthorResponseDTO>
                    .SuccessResponse(result, "Author updated"));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> Search(
            [FromQuery,
             RegularExpression(
                 @"^[a-zA-Z0-9\s]+$",
                 ErrorMessage = "Name can only contain letters, numbers and spaces")]
            string name)
        {
            var result = await _service.SearchByNameAsync(name);

            return Ok(
                ApiResponse<IEnumerable<AuthorResponseDTO>>
                    .SuccessResponse(result));
        }

        [HttpGet("{id:int:min(1)}/books")]
        public async Task<ActionResult<ApiResponse<AuthorWithBooksResponseDTO>>> GetBooks(
            int id)
        {
            var result = await _service.GetByIdWithBooksAsync(id);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<AuthorWithBooksResponseDTO>
                        .FailResponse($"Author {id} not found"));
            }

            return Ok(
                ApiResponse<AuthorWithBooksResponseDTO>
                    .SuccessResponse(result));
        }

        [HttpGet("country/{country:alpha:minlength(2):maxlength(50)}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetByCountry(
            string country)
        {
            var result = await _service.GetByCountryAsync(country);

            return Ok(
                ApiResponse<IEnumerable<AuthorResponseDTO>>
                    .SuccessResponse(result));
        }

        [HttpPost("{id:int:min(1)}/photo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<string>>> UploadPhoto(
            int id,
            IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return BadRequest(
                    ApiResponse<string>
                        .FailResponse("No photo file provided"));
            }

            var author = await _service.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(
                    ApiResponse<string>
                        .FailResponse($"Author {id} not found"));
            }

            try
            {
                var filePath =
                    await _fileUploadService
                        .UploadAuthorPhotoAsync(photo, id);

                if (!string.IsNullOrEmpty(author.Photo))
                {
                    _fileUploadService
                        .DeleteAuthorPhoto(author.Photo);
                }

                await _service.UpdatePhotoAsync(id, filePath);

                return Ok(
                    ApiResponse<string>
                        .SuccessResponse(
                            filePath,
                            "Photo uploaded and saved successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(
                    ApiResponse<string>
                        .FailResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(
                    ApiResponse<string>
                        .FailResponse(ex.Message));
            }
        }

        [HttpDelete("{id:int:min(1)}/photo")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePhoto(
            int id)
        {
            var author = await _service.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(
                    ApiResponse<bool>
                        .FailResponse($"Author {id} not found"));
            }

            if (string.IsNullOrEmpty(author.Photo))
            {
                return BadRequest(
                    ApiResponse<bool>
                        .FailResponse("No photo to delete"));
            }

            var deleted =
                _fileUploadService
                    .DeleteAuthorPhoto(author.Photo);

            await _service.UpdatePhotoAsync(id, null);

            return Ok(
                ApiResponse<bool>
                    .SuccessResponse(
                        deleted,
                        deleted
                            ? "Photo deleted"
                            : "Photo not found on disk"));
        }
    }
}