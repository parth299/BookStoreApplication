using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Implementations
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext
            _context;

        public UserRepository(
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
