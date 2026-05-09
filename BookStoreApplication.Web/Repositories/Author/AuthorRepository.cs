using Microsoft.EntityFrameworkCore;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories.Author
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Authors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<Author?> GetByIdWithBooksAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Bookauthors)
                .ThenInclude(ba => ba.IsbnNavigation)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<IEnumerable<Author>> SearchByNameAsync(string name)
        {
            return await _context.Authors
                .Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetByCountryAsync(string country)
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

        public async Task<Author> CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public void Update(Author author)
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