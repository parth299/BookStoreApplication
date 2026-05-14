using BookStoreApplication.MVC.DTOs.Book_Publisher;

namespace BookStoreApplication.MVC.Services.Book_Publisher;

public interface IPublisherService
{
    Task<PublisherResponseDto> CreateAsync(PublisherCreateDto dto);
    Task<IReadOnlyList<PublisherResponseDto>> GetAllAsync();
    Task<PublisherDetailsDto> GetDetailsByIdAsync(int id);
    Task<IReadOnlyList<PublisherDetailsDto>> GetByNameAsync(string name);
    Task<PublisherResponseDto> UpdateAsync(int id, PublisherUpdateDto dto);
    
}
