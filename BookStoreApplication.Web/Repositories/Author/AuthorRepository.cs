using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using AuthorEntity = BookStoreApplication.Web.Models.Author;

namespace BookStoreApplication.Web.Repositories.Author
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Authors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AuthorEntity?> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<AuthorEntity?> GetByIdWithBooksAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Bookauthors)
                .ThenInclude(ba => ba.IsbnNavigation)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<IEnumerable<AuthorEntity>> SearchByNameAsync(string name)
        {
            return await _context.Authors
                .Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<AuthorEntity>> GetByCountryAsync(string country)
        {
            // Filter by country through books/publisher/state relationship
            return await _context.Authors
                .Include(a => a.Bookauthors)
                .ThenInclude(ba => ba.IsbnNavigation)
                .ThenInclude(b => b.Publisher)
                .Where(a => a.Bookauthors.Any(ba => ba.IsbnNavigation.Publisher.StateCodeNavigation.StateName == country))
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string firstName, string lastName)
        {
            return await _context.Authors
                .AnyAsync(a => a.FirstName == firstName && a.LastName == lastName);
        }

        public async Task<AuthorEntity> CreateAsync(AuthorEntity author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<AuthorEntity> UpdateAsync(AuthorEntity author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public void Update(AuthorEntity author)
        {
            _context.Authors.Update(author);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Authors.CountAsync();
        }
    }
}
