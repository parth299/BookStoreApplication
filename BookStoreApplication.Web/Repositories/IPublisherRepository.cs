using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories;

public interface IPublisherRepository
{
    Task<Publisher?> GetByIdAsync(int id);
    Task<Publisher?> GetDetailsByIdAsync(int id);
    Task<IReadOnlyList<Publisher>> GetByNameAsync(string name);
    Task<bool> ExistsAsync(int id);
    Task<bool> StateExistsAsync(string stateCode);
    Task<IReadOnlyList<Publisher>> GetAllAsync();
    Task AddAsync(Publisher publisher);
    void Update(Publisher publisher);
    
    Task SaveChangesAsync();
}
