using AutoMapper;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.DTOs.Category;
using BookStoreApplication.Web.Repositories.Category;
using CategoryEntity = BookStoreApplication.Web.Models.Category;

namespace BookStoreApplication.Web.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto)
        {
            if (await _repository.ExistsByDescriptionAsync(dto.CatDescription))
                throw new BadRequestException("Category with this description already exists");

            var category = _mapper.Map<CategoryEntity>(dto);
            var result = await _repository.CreateAsync(category);
            return _mapper.Map<CategoryResponseDto>(result);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(int id, CategoryRequestDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<CategoryResponseDto>(result);
        }

       public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}