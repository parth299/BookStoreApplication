using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Exceptions;

namespace BookStoreApplication.Web.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> ExistsByDescriptionAsync(string description)
        {
            return await _context.Categories
                .AnyAsync(c => c.CatDescription == description);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Load category with related books to check for dependencies
            var category = await _context.Categories
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.CatId == id);
                
            if (category == null) return false;

            // Safe delete: prevent deletion if category has related books
            // This maintains referential integrity without DB schema changes
            if (category.Books != null && category.Books.Any())
            {
                throw new BadRequestException(
                    $"Cannot delete category '{category.CatDescription}' because it has {category.Books.Count} associated books. " +
                    "Please reassign or delete the books first.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}