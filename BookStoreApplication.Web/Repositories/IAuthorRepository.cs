using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync(int pageNumber, int pageSize);
        Task<Author?> GetByIdAsync(int id);
        Task<Author?> GetByIdWithBooksAsync(int id);
        Task<IEnumerable<Author>> SearchByNameAsync(string name);
        Task<IEnumerable<Author>> GetByCountryAsync(string country);
        Task<bool> ExistsByNameAsync(string firstName, string lastName);
        Task<Author> CreateAsync(Author author);
        Task<Author> UpdateAsync(Author author);
        void Update(Author author);
        Task SaveChangesAsync();
        Task<int> CountAsync();
    }
}