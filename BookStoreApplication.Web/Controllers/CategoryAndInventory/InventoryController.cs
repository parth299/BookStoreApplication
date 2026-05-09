using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Services.Inventory;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.CategoryAndInventory
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class InventoryController
        : ControllerBase
    {
        private readonly IInventoryService
            _service;

        public InventoryController(
            IInventoryService service)
        {
            _service = service;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            Create(InventoryDto dto)
        {
            var id =
                await _service.CreateAsync(dto);

            return Ok(
                ApiResponse<int>
                .SuccessResponse(id));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult>Patch(int id,InventoryPatchDto dto)
        {
            await _service
                .PatchAsync(id, dto);

            return Ok(
                ApiResponse<string>
                .MessageResponse(
                    "Inventory patched successfully"));
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAll([FromQuery] string? isbn)
        {
            var data =
                await _service
                    .GetAllAsync(isbn);

            return Ok(
                ApiResponse<IEnumerable<
                    InventoryDto>>
                .SuccessResponse(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetById(int id)
        {
            var data =
                await _service
                    .GetByIdAsync(id);

            return Ok(
                ApiResponse<InventoryDto>
                .SuccessResponse(data));
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            Update(
                int id,
                InventoryDto dto)
        {
            await _service
                .UpdateAsync(id, dto);

            return Ok(ApiResponse<string>
                       .MessageResponse("Inventory updated"));
        }

        [HttpGet("available")]
        public async Task<IActionResult>
        GetAvailableInventory()
        {
            var data =
                await _service
                    .GetAvailableInventoryAsync();

            return Ok(
                ApiResponse<IEnumerable<InventoryDto>>
                .SuccessResponse(data));
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult>
            LowStock()
        {
            var data =
                await _service
                    .GetLowStockAsync();

            return Ok(
                ApiResponse<IEnumerable<
                    LowStockDto>>
                .SuccessResponse(data));
        }
    }
}
