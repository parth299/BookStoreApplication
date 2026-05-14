using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Services.Author_Category;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.Web.Controllers.Author_Category
{
    [ApiController]
    [Route("api/author")]
    [AllowAnonymous]

    [Route("api/[controller]")]
    [Authorize(Roles = "User,RegisteredUser,Manager,StoreOwner,Admin")]
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


        [Authorize(Roles = "Manager,StoreOwner,Admin")]
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

                var response = ApiResponse<AuthorResponseDTO>
                    .FailResponse("Validation failed", 400, errors);

                return BadRequest(response);
            }

            var result = await _service.CreateAsync(dto);

            var success = ApiResponse<AuthorResponseDTO>
                .SuccessResponse(result, "Author created", 200);

            return Ok(success);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetAll(
            [FromQuery] PaginationFilter filter)
        {
            var result = await _service.GetAllAsync(filter);

            var response = ApiResponse<IEnumerable<AuthorResponseDTO>>
                .SuccessResponse(result, "Authors fetched successfully.", 200);

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> GetById(
            int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                var notFound = ApiResponse<AuthorResponseDTO>
                    .NotFound($"Author {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<AuthorResponseDTO>
                .SuccessResponse(result, "Author fetched successfully.", 200);

            return Ok(response);
        }


        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<AuthorResponseDTO>>> Update(
            int id,
            AuthorRequestDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                var notFound = ApiResponse<AuthorResponseDTO>
                    .NotFound($"Author {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<AuthorResponseDTO>
                .SuccessResponse(result, "Author updated", 200);

            return Ok(response);
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

            var response = ApiResponse<IEnumerable<AuthorResponseDTO>>
                .SuccessResponse(result, "Author search completed successfully.", 200);

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}/books")]
        public async Task<ActionResult<ApiResponse<AuthorWithBooksResponseDTO>>> GetBooks(
            int id)
        {
            var result = await _service.GetByIdWithBooksAsync(id);

            if (result == null)
            {
                var notFound = ApiResponse<AuthorWithBooksResponseDTO>
                    .NotFound($"Author {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<AuthorWithBooksResponseDTO>
                .SuccessResponse(result, "Author books fetched successfully.", 200);

            return Ok(response);
        }

        [HttpGet("country/{country:alpha:minlength(2):maxlength(50)}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorResponseDTO>>>> GetByCountry(
            string country)
        {
            var result = await _service.GetByCountryAsync(country);

            var response = ApiResponse<IEnumerable<AuthorResponseDTO>>
                .SuccessResponse(result, "Authors fetched successfully.", 200);

            return Ok(response);
        }


        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        [HttpPost("{id:int:min(1)}/photo")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<string>>> UploadPhoto(
            int id,
            IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                var bad = ApiResponse<string>
                    .BadRequest("No photo file provided");

                return BadRequest(bad);
            }

            var author = await _service.GetByIdAsync(id);

            if (author == null)
            {
                var notFound = ApiResponse<string>
                    .NotFound($"Author {id} not found");

                return NotFound(notFound);
            }

            try
            {
                var filePath =
                    await _fileUploadService
                        .UploadAuthorPhotoAsync(photo, id);

                if (!string.IsNullOrEmpty(author.Photo))
                {
                    _fileUploadService.DeleteAuthorPhoto(author.Photo);
                }

                await _service.UpdatePhotoAsync(id, filePath);

                var response = ApiResponse<string>
                    .SuccessResponse(filePath, "Photo uploaded and saved successfully", 200);

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                var bad = ApiResponse<string>.BadRequest(ex.Message);
                return BadRequest(bad);
            }
            catch (ArgumentException ex)
            {
                var bad = ApiResponse<string>.BadRequest(ex.Message);
                return BadRequest(bad);
            }
        }


        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        [HttpDelete("{id:int:min(1)}/photo")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePhoto(
            int id)
        {
            var author = await _service.GetByIdAsync(id);

            if (author == null)
            {
                var notFound = ApiResponse<bool>
                    .NotFound($"Author {id} not found");

                return NotFound(notFound);
            }

            if (string.IsNullOrEmpty(author.Photo))
            {
                var bad = ApiResponse<bool>
                    .BadRequest("No photo to delete");

                return BadRequest(bad);
            }

            var deleted =
                _fileUploadService
                    .DeleteAuthorPhoto(author.Photo);

            await _service.UpdatePhotoAsync(id, null);

            var response = ApiResponse<bool>
                .SuccessResponse(
                    deleted,
                    deleted ? "Photo deleted" : "Photo not found on disk",
                    200);

            return Ok(response);
        }
    }
}
