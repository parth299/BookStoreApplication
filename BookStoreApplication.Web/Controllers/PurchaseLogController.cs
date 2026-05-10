using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Services;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PurchaseLogController: ControllerBase
    {
        private readonly IPurchaseLogService _service;

        public PurchaseLogController(IPurchaseLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>Checkout(PurchaseDto dto)
        {
            await _service.CheckoutAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Purchase completed"));
        }
    }
}
