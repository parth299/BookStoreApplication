using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Implementations
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Shoppingcart cart)
        {
            await _context.Shoppingcarts.AddAsync(cart);
        }

        public async Task<IEnumerable<Shoppingcart>>GetCartAsync(int userId)
        {
            return await _context.Shoppingcarts
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<Shoppingcart?>GetItemAsync(int userId,string isbn)
        {
            return await _context.Shoppingcarts.FirstOrDefaultAsync(x =>x.UserId == userId && x.Isbn == isbn);
        }

        public Task RemoveAsync(Shoppingcart cart)
        {
            _context.Shoppingcarts.Remove(cart);

            return Task.CompletedTask;
        }

        public async Task ClearAsync(int userId)
        {
            var items = await _context.Shoppingcarts
                       .Where(x => x.UserId == userId)
                       .ToListAsync();

            _context.Shoppingcarts.RemoveRange(items);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
