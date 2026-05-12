using BookStoreApplication.Web.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace BookStoreApplication.Web.Services.Inventory_Cart_Purchase
{
    public class ReservationService: IReservationService
    {
        private readonly IMemoryCache _cache;

        public ReservationService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task ReserveAsync(int inventoryId, int userId)
        {
            var key = $"reserved_{inventoryId}";

            _cache.Set(key, userId, TimeSpan.FromMinutes(15));

            await Task.CompletedTask;
        }
        public bool IsReservedByUser(int inventoryId, int userId)
        {
            if (_cache.TryGetValue($"reserved_{inventoryId}", out int reservedUserId))
                return reservedUserId == userId;
            return false;
        }
        public bool IsReserved(int inventoryId)
        {
            return _cache.TryGetValue($"reserved_{inventoryId}",out _);
        }

        public async Task ReleaseAsync(int inventoryId)
        {
            _cache.Remove($"reserved_{inventoryId}");

            await Task.CompletedTask;
        }
    }
}
