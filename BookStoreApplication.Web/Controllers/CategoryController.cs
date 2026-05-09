using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Wrappers;
using BookStoreApplication.Web.Filters;

namespace BookStoreApplication.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // POST /api/categories
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Create(CategoryRequestDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(ApiResponse.Success(result, "Category created"));
        }

        // GET /api/categories
        [HttpGet]
        [ServiceFilter(typeof(LogActionFilter))]  // Custom Action Filter
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryResponseDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse.Success(result));
        }

        // GET /api/categories/{id}
        [HttpGet("{id:int:min(1)}")]
        [ServiceFilter(typeof(LogActionFilter))]  // Custom Action Filter on specific action
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponse.Fail<CategoryResponseDto>($"Category {id} not found"));
            return Ok(ApiResponse.Success(result));
        }

        // PUT /api/categories/{id}
        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<CategoryResponseDto>>> Update(int id, CategoryRequestDto dto)
        {
            // Manual ModelState validation (redundant with ValidationFilter but shows local handling)
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return BadRequest(ApiResponse.Fail<CategoryResponseDto>("Validation failed", errors));
            }

            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(ApiResponse.Fail<CategoryResponseDto>($"Category {id} not found"));
            return Ok(ApiResponse.Success(result, "Category updated"));
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse.Fail<bool>($"Category {id} not found"));
            return Ok(ApiResponse.Success(true, "Category deleted successfully"));
        }
    }
}