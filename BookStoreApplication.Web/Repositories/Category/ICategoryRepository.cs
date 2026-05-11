using BookStoreApplication.Web.Models;
using CategoryEntity = BookStoreApplication.Web.Models.Category;

namespace BookStoreApplication.Web.Repositories.Category
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity?> GetByIdAsync(int id);
        Task<bool> ExistsByDescriptionAsync(string description);
        Task<CategoryEntity> CreateAsync(CategoryEntity category);
        Task<CategoryEntity> UpdateAsync(CategoryEntity category);
        Task<bool> DeleteAsync(int id);
    }
}