using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Data;
using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.DTOs;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Repositories.User 
{
    public class UserReporitory {
        private readonly AppDbContext _context;

        public UserReporitory(AppDbContext context) {
            _context = context;
        }

        public async Task<List<UserEntity>> GetUsersByRoleId(int roleId)
        {
            var users = await _context.Users.Where(u => u.RoleNumber == roleId).ToListAsync();
            return users;
        }

        public async Task<UserEntity?> UpdatePassword(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity?> UpdateUser(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity?> GetByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<UserEntity?> GetByUserId(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserEntity?> CreateUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserEntity>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}