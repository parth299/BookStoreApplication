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
    public class PurchaseLogController
        : ControllerBase
    {
        private readonly IPurchaseLogService
            _service;

        public PurchaseLogController(
            IPurchaseLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
            Checkout(PurchaseDto dto)
        {
            await _service
                .CheckoutAsync(dto);

            return Ok(
                ApiResponse<string>
                .SuccessResponse(
                    "Purchase completed"));
        }
    }
}
