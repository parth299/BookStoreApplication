using BookStoreApplication.Web.Models;
using AuthorEntity = BookStoreApplication.Web.Models.Author;

namespace BookStoreApplication.Web.Repositories.Author
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorEntity>> GetAllAsync(int pageNumber, int pageSize);
        Task<AuthorEntity?> GetByIdAsync(int id);
        Task<AuthorEntity?> GetByIdWithBooksAsync(int id);
        Task<IEnumerable<AuthorEntity>> SearchByNameAsync(string name);
        Task<IEnumerable<AuthorEntity>> GetByCountryAsync(string country);
        Task<bool> ExistsByNameAsync(string firstName, string lastName);
        Task<AuthorEntity> CreateAsync(AuthorEntity author);
        Task<AuthorEntity> UpdateAsync(AuthorEntity author);
        void Update(AuthorEntity author);
        Task SaveChangesAsync();
        Task<int> CountAsync();
    }
}
