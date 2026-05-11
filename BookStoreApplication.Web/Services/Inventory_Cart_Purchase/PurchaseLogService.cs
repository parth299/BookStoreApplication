using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.CartPurchaseInventory;
using BookStoreApplication.Web.Repositories.Implementations;

namespace BookStoreApplication.Web.Services.Inventory_Cart_Purchase
{
    public class PurchaseLogService: IPurchaseLogService
    {
        private readonly IShoppingCartRepository _cartRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IPurchaseLogRepository _purchaseRepository;

        private readonly IReservationService _reservationService;

        private readonly IUserIsExistsRepository _userRepository;

        public PurchaseLogService(
            IShoppingCartRepository cartRepository,
            IInventoryRepository inventoryRepository,
            IPurchaseLogRepository purchaseRepository,
            IReservationService reservationService,
            IUserIsExistsRepository userRepository)
        {
            _cartRepository = cartRepository;

            _inventoryRepository = inventoryRepository;

            _purchaseRepository = purchaseRepository;

            _reservationService = reservationService;
            _userRepository = userRepository;

        }

        public async Task CheckoutAsync(PurchaseDto dto)
        {
            var userExists = await _userRepository.UserExistsAsync(dto.UserId);

            if (!userExists)
            {
                throw new BadRequestException("Invalid user");
            }
            var cartItems = await _cartRepository.GetCartAsync(dto.UserId);

            if (!cartItems.Any())
            {
                throw new BadRequestException("Cart is empty");
            }

            foreach (var item in cartItems)
            {
                var inventory = await _inventoryRepository.GetAvailableByISBNAsync(item.Isbn);

                if (inventory == null)
                    throw new BadRequestException($"Inventory unavailable for ISBN {item.Isbn}");

                // Verify this inventory is reserved by THIS user (not someone else)
                if (!_reservationService.IsReservedByUser(inventory.InventoryId, dto.UserId))
                    throw new BadRequestException($"Inventory for ISBN {item.Isbn} is not reserved by this user");

                inventory.Purchased = 1;
                _inventoryRepository.Update(inventory);

                await _purchaseRepository.AddAsync(
                    new Purchaselog
                    {
                        UserId = dto.UserId,
                        InventoryId = inventory.InventoryId
                    });

                await _reservationService.ReleaseAsync(inventory.InventoryId);
            }

            await _cartRepository.ClearAsync(dto.UserId);

            await _purchaseRepository.SaveAsync();
        }
    }
}
