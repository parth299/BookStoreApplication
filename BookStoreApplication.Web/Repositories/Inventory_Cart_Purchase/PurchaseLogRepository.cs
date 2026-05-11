using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Implementations
{
    public class PurchaseLogRepository
    : IPurchaseLogRepository
    {
        private readonly AppDbContext _context;

        public PurchaseLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Purchaselog log)
        {
            await _context.Purchaselogs.AddAsync(log);
        }

        public async Task<IEnumerable<Purchaselog>>
            GetByUserAsync(int userId)
        {
            return await _context.Purchaselogs
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
