using BookStoreApplication.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase
{
    public class UserIsExistsRepository : IUserIsExistsRepository
    {
        private readonly AppDbContext
            _context;

        public UserIsExistsRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool>UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(x =>
                    x.UserId == userId);
        }
    }
}
