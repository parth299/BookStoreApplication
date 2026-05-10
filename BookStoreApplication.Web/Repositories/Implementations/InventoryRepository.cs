using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Implementations
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Inventory inventory)
        {
            await _context.Inventories.AddAsync(inventory);
        }

        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<IEnumerable<Inventory>>FilterAsync(string? isbn)
        {
            var query =_context.Inventories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(isbn))
            {
                query = query.Where(x =>x.Isbn == isbn);
            }

            return await query.ToListAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories.FirstOrDefaultAsync(x =>x.InventoryId == id);
        }

        public async Task<Inventory?>GetAvailableByISBNAsync(string isbn)
        {
            return await _context.Inventories.FirstOrDefaultAsync(x =>x.Isbn == isbn && x.Purchased == 0);
        }

        public async Task<IEnumerable<Inventory>> GetAvailableInventoryAsync()
        {
            return await _context.Inventories.Where(x => x.Purchased == 0).ToListAsync();
        }

        public async Task<IEnumerable<LowStockDto>>GetLowStockAsync()
        {
            return await _context.Inventories
                .Where(x => x.Purchased == 0)
                .GroupBy(x => x.Isbn)
                .Select(g => new LowStockDto
                {
                    ISBN = g.Key,
                    AvailableCopies = g.Count()
                })
                .Where(x => x.AvailableCopies < 5)
                .ToListAsync();
        }

        public void Update(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
