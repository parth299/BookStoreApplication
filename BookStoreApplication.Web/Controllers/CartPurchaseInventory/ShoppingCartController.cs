using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Services.Inventory_Cart_Purchase;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.CartAndPurchase
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "RegisteredUser,StoreOwner,Admin")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _service;

        public ShoppingCartController(IShoppingCartService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartItemDto dto)
        {
            await _service.AddAsync(dto);

            var response = ApiResponse<string>.SuccessResponse(
                "Added to cart",
                "Success",
                StatusCodes.Status201Created);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var data = await _service.GetCartAsync(userId);

            var response = ApiResponse<IEnumerable<CartItemDto>>.SuccessResponse(
                data,
                "Cart fetched successfully.",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{userId}/{isbn}")]
        public async Task<IActionResult> Remove(int userId, string isbn)
        {
            await _service.RemoveAsync(userId, isbn);

            var response = ApiResponse<string>.SuccessResponse(
                "Removed from cart successfully.",
                "Success",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Clear(int userId)
        {
            await _service.ClearAsync(userId);

            var response = ApiResponse<string>.SuccessResponse(
                "Cart cleared successfully.",
                "Success",
                StatusCodes.Status200OK);

            return StatusCode(response.StatusCode, response);
        }
    }
}