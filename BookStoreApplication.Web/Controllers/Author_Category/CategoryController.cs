using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using BookStoreApplication.Web.Services.Author_Category;

namespace BookStoreApplication.Web.Controllers.Author_Category
{
    [ApiController]
    [Route("api/categories")]
    [Route("api/category")]
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Create(
            CategoryRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result, "Category created");

            return Ok(response);
        }

        [HttpGet]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            var response = ApiResponse<IEnumerable<CategoryResponseDto>>
                .SuccessResponse(result, "Success");

            return Ok(response);
        }

        [HttpGet("{id:int:min(1)}")]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> GetById(
            int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                var notFound = ApiResponse<CategoryResponseDto>
                    .NotFound($"Category {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result);

            return Ok(response);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Update(
            int id,
            CategoryRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var bad = ApiResponse<CategoryResponseDto>
                    .BadRequest("Validation failed", errors);

                return BadRequest(bad);
            }

            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                var notFound = ApiResponse<CategoryResponseDto>
                    .NotFound($"Category {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result, "Category updated");

            return Ok(response);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(
            int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                var notFound = ApiResponse<bool>
                    .NotFound($"Category {id} not found");

                return NotFound(notFound);
            }

            var response = ApiResponse<bool>
                .SuccessResponse(true, "Category deleted successfully");

            return Ok(response);
        }
    }
}
