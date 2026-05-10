using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly AppDbContext _context;

    public PublisherRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Publisher?> GetByIdAsync(int id)
    {
        return _context.Publishers.FirstOrDefaultAsync(p => p.PublisherId == id);
    }

    public Task<Publisher?> GetDetailsByIdAsync(int id)
    {
        return _context.Publishers
            .Include(p => p.StateCodeNavigation)
            .Include(p => p.Books)
            .FirstOrDefaultAsync(p => p.PublisherId == id);
    }

    public async Task<IReadOnlyList<Publisher>> GetByNameAsync(string name)
    {
        return await _context.Publishers
            .Include(p => p.StateCodeNavigation)
            .Include(p => p.Books)
            .Where(p => p.Name == name)
            .OrderBy(p => p.PublisherId)
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _context.Publishers.AnyAsync(p => p.PublisherId == id);
    }

    public Task<bool> StateExistsAsync(string stateCode)
    {
        return _context.States.AnyAsync(s => s.StateCode == stateCode);
    }

    public async Task<IReadOnlyList<Publisher>> GetAllAsync()
    {
        return await _context.Publishers
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public Task AddAsync(Publisher publisher)
    {
        return _context.Publishers.AddAsync(publisher).AsTask();
    }

    public void Update(Publisher publisher)
    {
        _context.Publishers.Update(publisher);
    }


    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

}
