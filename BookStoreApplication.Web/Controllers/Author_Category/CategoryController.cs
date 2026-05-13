using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;
using BookStoreApplication.Web.DTOs.Category;
using Microsoft.AspNetCore.Authorization;
using BookStoreApplication.Web.Services.Author_Category;

namespace BookStoreApplication.Web.Controllers.Author_Category
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,RegisteredUser,Manager,StoreOwner,Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Create(
            CategoryRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result, "Category created");

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ServiceFilter(typeof(LogActionFilter))]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();

            var response = ApiResponse<IEnumerable<CategoryResponseDto>>
                .SuccessResponse(result, "Success");

            return StatusCode(response.StatusCode, response);
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

                return StatusCode(notFound.StatusCode, notFound);
            }

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result);

            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Manager,StoreOwner,Admin")]
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

                return StatusCode(bad.StatusCode, bad);
            }

            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
            {
                var notFound = ApiResponse<CategoryResponseDto>
                    .NotFound($"Category {id} not found");

                return StatusCode(notFound.StatusCode, notFound);
            }

            var response = ApiResponse<CategoryResponseDto>
                .SuccessResponse(result, "Category updated");

            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(
            int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                var notFound = ApiResponse<bool>
                    .NotFound($"Category {id} not found");

                return StatusCode(notFound.StatusCode, notFound);
            }

            var response = ApiResponse<bool>
                .SuccessResponse(true, "Category deleted successfully");

            return StatusCode(response.StatusCode, response);
        }
    }
}
