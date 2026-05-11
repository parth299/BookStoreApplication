using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Services.Inventory;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.CartAndPurchase
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        // ---------------- CREATE ----------------
        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryDto dto)
        {
            var id = await _service.CreateAsync(dto);

            var response = ApiResponse<int>.CreatedResponse(
                id,
                "Inventory created successfully.");

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- PATCH ----------------
        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, InventoryPatchDto dto)
        {
            var exists = await _service.GetByIdAsync(id);

            if (exists == null)
            {
                var notFound = ApiResponse<string>.NotFound(
                    $"Inventory {id} not found");

                return StatusCode(notFound.StatusCode, notFound);
            }

            await _service.PatchAsync(id, dto);

            var response = ApiResponse<string>.SuccessResponse(
                "Inventory patched successfully",
                "Success",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- GET ALL ----------------
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? isbn)
        {
            var data = await _service.GetAllAsync(isbn);

            var response = ApiResponse<IEnumerable<InventoryDto>>.SuccessResponse(
                data,
                "Inventory fetched successfully.",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- GET BY ID ----------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                var notFound = ApiResponse<InventoryDto>.NotFound(
                    $"Inventory {id} not found");

                return StatusCode(notFound.StatusCode, notFound);
            }

            var response = ApiResponse<InventoryDto>.SuccessResponse(
                data,
                "Inventory fetched successfully.",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- UPDATE ----------------
        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryDto dto)
        {
            var exists = await _service.GetByIdAsync(id);

            if (exists == null)
            {
                var notFound = ApiResponse<string>.NotFound(
                    $"Inventory {id} not found");

                return StatusCode(notFound.StatusCode, notFound);
            }

            await _service.UpdateAsync(id, dto);

            var response = ApiResponse<string>.SuccessResponse(
                "Inventory updated successfully",
                "Success",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- AVAILABLE ----------------
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableInventory()
        {
            var data = await _service.GetAvailableInventoryAsync();

            var response = ApiResponse<IEnumerable<InventoryDto>>.SuccessResponse(
                data,
                "Available inventory fetched successfully.",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        // ---------------- LOW STOCK ----------------
        [Authorize(Roles = "StoreOwner,Admin")]
        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock()
        {
            var data = await _service.GetLowStockAsync();

            var response = ApiResponse<IEnumerable<LowStockDto>>.SuccessResponse(
                data,
                "Low stock inventory fetched successfully.",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }
    }
}