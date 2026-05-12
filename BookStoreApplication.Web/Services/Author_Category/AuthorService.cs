using AutoMapper;
using AuthorEntity = BookStoreApplication.Web.Models.Author;
//using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Repositories.Author;

namespace BookStoreApplication.Web.Services.Author_Category
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorResponseDTO>> GetAllAsync(PaginationFilter filter)
        {
            var authors = await _repository.GetAllAsync(filter.PageNumber, filter.PageSize);
            return _mapper.Map<IEnumerable<AuthorResponseDTO>>(authors);
        }

        public async Task<AuthorResponseDTO?> GetByIdAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            return author == null ? null : _mapper.Map<AuthorResponseDTO>(author);
        }

        public async Task<AuthorWithBooksResponseDTO?> GetByIdWithBooksAsync(int id)
        {
            var author = await _repository.GetByIdWithBooksAsync(id);
            if (author == null) return null;

            var dto = _mapper.Map<AuthorWithBooksResponseDTO>(author);
            dto.Books = author.Bookauthors?
                .Select(ba => new BookSummaryDTO  
                {
                    Isbn = ba.Isbn,
                    Title = ba.IsbnNavigation?.Title ?? "Unknown"
                }).ToList() ?? new List<BookSummaryDTO>();

            return dto;
        }

        public async Task<IEnumerable<AuthorResponseDTO>> SearchByNameAsync(string name)
        {
            var authors = await _repository.SearchByNameAsync(name);
            return _mapper.Map<IEnumerable<AuthorResponseDTO>>(authors);
        }

        public async Task<IEnumerable<AuthorResponseDTO>> GetByCountryAsync(string country)
        {
            var authors = await _repository.GetByCountryAsync(country);
            return _mapper.Map<IEnumerable<AuthorResponseDTO>>(authors);
        }

        public async Task<AuthorResponseDTO> CreateAsync(AuthorRequestDTO dto)
        {
            if (await _repository.ExistsByNameAsync(dto.FirstName, dto.LastName))
                throw new BadRequestException("Author with this name already exists");

            var author = _mapper.Map<AuthorEntity>(dto);
            var result = await _repository.CreateAsync(author);
            return _mapper.Map<AuthorResponseDTO>(result);
        }

        public async Task<AuthorResponseDTO?> UpdateAsync(int id, AuthorRequestDTO dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var result = await _repository.UpdateAsync(existing);
            return _mapper.Map<AuthorResponseDTO>(result);
        }

        public async Task<AuthorResponseDTO?> UpdatePhotoAsync(int id, string photoPath)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            // Update only the Photo field
            existing.Photo = photoPath;
            
            // Use existing UpdateAsync which handles SaveChanges
            var result = await _repository.UpdateAsync(existing);
            
            return _mapper.Map<AuthorResponseDTO>(result);
        }
    }
}
