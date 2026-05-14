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
    [Authorize(Roles = "User,RegisteredUser,Manager,StoreOwner,Admin")]
    public class ShoppingCartController
        : ControllerBase
    {
        private readonly IShoppingCartService
            _service;

        public ShoppingCartController(
            IShoppingCartService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
            Add(CartItemDto dto)
        {
            await _service.AddAsync(dto);

            return Ok(ApiResponse<string>
                    .MessageResponse("Added to cart"));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult>
            GetCart(int userId)
        {
            var data =
                await _service
                    .GetCartAsync(userId);

            return Ok(
                ApiResponse<IEnumerable<
                    CartItemDto>>
                .SuccessResponse(data));
        }

        [HttpDelete("{userId}/{isbn}")]
        public async Task<IActionResult>
            Remove(
                int userId,
                string isbn)
        {
            await _service
                .RemoveAsync(
                    userId,
                    isbn);

            return Ok(
                ApiResponse<string>
                .SuccessResponse(
                    "Removed"));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult>
            Clear(int userId)
        {
            await _service
                .ClearAsync(userId);

            return Ok(
                ApiResponse<string>
                .SuccessResponse(
                    "Cart cleared"));
        }
    }
}
