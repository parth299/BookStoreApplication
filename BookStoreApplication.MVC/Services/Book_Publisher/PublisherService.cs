using BookStoreApplication.MVC.DTOs.Book_Publisher;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Book_Publisher;

public class PublisherService : IPublisherService
{
    private readonly ApiClient _api;
    public PublisherService(ApiClient api) => _api = api;

    public async Task<PublisherResponseDto> CreateAsync(PublisherCreateDto dto)
        => await _api.PostAsync<PublisherResponseDto>("publishers", dto) ?? new PublisherResponseDto { PublisherId = dto.PublisherId, Name = dto.Name, City = dto.City, StateCode = dto.StateCode };

    public async Task<IReadOnlyList<PublisherResponseDto>> GetAllAsync()
        => await _api.GetListAsync<PublisherResponseDto>("publishers");

    public async Task<PublisherDetailsDto> GetDetailsByIdAsync(int id)
        => await _api.GetAsync<PublisherDetailsDto>($"publishers/{id}") ?? new PublisherDetailsDto { PublisherId = id, Name = "Publisher not found" };

    public async Task<IReadOnlyList<PublisherDetailsDto>> GetByNameAsync(string name)
        => await _api.GetListAsync<PublisherDetailsDto>($"publishers/by-name/{Uri.EscapeDataString(name)}");

    public async Task<PublisherResponseDto> UpdateAsync(int id, PublisherUpdateDto dto)
        => await _api.PutAsync<PublisherResponseDto>($"publishers/{id}", dto) ?? new PublisherResponseDto { PublisherId = id, Name = dto.Name, City = dto.City, StateCode = dto.StateCode };
}
