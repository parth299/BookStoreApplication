using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Data;
using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Repositories 
{
    public class UserReporitory {
        private readonly AppDbContext _context;

        public UserReporitory(AppDbContext context) {
            _context = context;
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}