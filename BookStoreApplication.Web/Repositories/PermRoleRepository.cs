using System.Collections.ObjectModel;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories
{
    public class PermRoleRepository
    {
        private readonly AppDbContext _context;

        public PermRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permrole>> GetRoles()
        {
            var response = await _context.Permroles.ToListAsync();
            return response;
        }
    }
}