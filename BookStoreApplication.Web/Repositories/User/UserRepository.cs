using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Data;
using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Repositories.User 
{
    public class UserReporitory {
        private readonly AppDbContext _context;

        public UserReporitory(AppDbContext context) {
            _context = context;
        }

        public async Task<List<User>> GetUsersByRoleId(int roleId)
        {
            var users = await _context.Users.Where(u => u.RoleNumber == roleId).ToListAsync();
            return users;
        }

        public async Task<User?> UpdatePassword(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User?> GetByUserId(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}