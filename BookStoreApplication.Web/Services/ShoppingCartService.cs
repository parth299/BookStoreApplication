using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;

namespace BookStoreApplication.Web.Services
{
    public class ShoppingCartService
        : IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IReservationService _reservationService;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IShoppingCartRepository cartRepository,
                                   IInventoryRepository inventoryRepository,
                                   IReservationService reservationService, 
                                   IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _inventoryRepository = inventoryRepository;
            _reservationService = reservationService;
            _userRepository = userRepository;
        }


        public async Task AddAsync(CartItemDto dto)
        {
            var userExists = await _userRepository.UserExistsAsync(dto.UserId);

            if (!userExists)
            {
                throw new BadRequestException(
                    "Invalid user");
            }
            var inventory =
                await _inventoryRepository.GetAvailableByISBNAsync(dto.ISBN);

            if (inventory == null)
            {
                throw new BadRequestException(
                    "No inventory available");
            }

            if (_reservationService.IsReserved(inventory.InventoryId))
            {
                throw new BadRequestException(
                    "Inventory already reserved");
            }

            var existing =
                await _cartRepository.GetItemAsync(dto.UserId,dto.ISBN);

            if (existing != null)
            {
                throw new BadRequestException(
                    "Duplicate cart item");
            }

            await _reservationService.ReserveAsync(inventory.InventoryId,dto.UserId);

            var entity = new Shoppingcart
            {
                UserId = dto.UserId,
                Isbn = dto.ISBN
            };

            await _cartRepository.AddAsync(entity);

            await _cartRepository.SaveAsync();
        }

        public async Task<IEnumerable<CartItemDto>>GetCartAsync(int userId)
        {
            var items =
                await _cartRepository.GetCartAsync(userId);

            return items.Select(x =>
                new CartItemDto
                {
                    UserId = x.UserId,
                    ISBN = x.Isbn,
                    Quantity = 1
                });
        }

        public async Task RemoveAsync(int userId,string isbn)
        {
            var item =
                await _cartRepository.GetItemAsync(userId,isbn);

            if (item == null)
            {
                throw new NotFoundException(
                    "Cart item not found");
            }

            var inventory =
                await _inventoryRepository
                    .GetAvailableByISBNAsync(
                        isbn);

            if (inventory != null)
            {
                await _reservationService
                    .ReleaseAsync(
                        inventory.InventoryId);
            }

            await _cartRepository
                .RemoveAsync(item);

            await _cartRepository
                .SaveAsync();
        }

        public async Task ClearAsync(int userId)
        {
            var items =
                await _cartRepository
                    .GetCartAsync(userId);

            foreach (var item in items)
            {
                var inventory =
                    await _inventoryRepository
                        .GetAvailableByISBNAsync(
                            item.Isbn);

                if (inventory != null)
                {
                    await _reservationService
                        .ReleaseAsync(
                            inventory.InventoryId);
                }
            }

            await _cartRepository
                .ClearAsync(userId);

            await _cartRepository
                .SaveAsync();
        }
    }
}
