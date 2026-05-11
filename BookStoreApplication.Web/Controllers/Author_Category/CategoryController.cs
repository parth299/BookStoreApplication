using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.DTOs.Category;
using BookStoreApplication.Web.Services.Category;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.Web.Controllers.Author_Category
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Create(
            CategoryRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);

            return Ok(
                ApiResponse<CategoryResponseDto>.SuccessResponse(
                    result,
                    "Category created"));
        }

        [HttpGet]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(
                ApiResponse<IEnumerable<CategoryResponseDto>>
                    .SuccessResponse(result));
        }

        [HttpGet("{id:int:min(1)}")]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> GetById(
            int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<CategoryResponseDto>.FailResponse(
                        $"Category {id} not found"));
            }

            return Ok(
                ApiResponse<CategoryResponseDto>.SuccessResponse(
                    result));
        }

        [Authorize(Roles = "StoreOwner,Admin")]
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

                return BadRequest(
                    ApiResponse<CategoryResponseDto>.FailResponse(
                        "Validation failed",
                        errors));
            }

            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                return NotFound(
                    ApiResponse<CategoryResponseDto>.FailResponse(
                        $"Category {id} not found"));
            }

            return Ok(
                ApiResponse<CategoryResponseDto>.SuccessResponse(
                    result,
                    "Category updated"));
        }

        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(
            int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound(
                    ApiResponse<bool>.FailResponse(
                        $"Category {id} not found"));
            }

            return Ok(
                ApiResponse<bool>.SuccessResponse(
                    true,
                    "Category deleted successfully"));
        }
    }
}