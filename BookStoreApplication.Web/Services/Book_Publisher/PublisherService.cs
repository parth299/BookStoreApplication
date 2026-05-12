using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using AutoMapper;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Repositories.Book_Publisher;

namespace BookStoreApplication.Web.Services.Book_Publisher;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public PublisherService(IPublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<PublisherResponseDto> CreateAsync(PublisherCreateDto dto)
    {
        if (dto is null)
        {
            throw new BadRequestException("Publisher request body is required.");
        }

        if (await _publisherRepository.ExistsAsync(dto.PublisherId))
        {
            throw new ConflictException($"Publisher with id '{dto.PublisherId}' already exists.");
        }

        await EnsureStateExistsAsync(dto.StateCode);

        var publisher = _mapper.Map<Publisher>(dto);

        await _publisherRepository.AddAsync(publisher);
        await _publisherRepository.SaveChangesAsync();

        return _mapper.Map<PublisherResponseDto>(publisher);
    }

    public async Task<IReadOnlyList<PublisherResponseDto>> GetAllAsync()
    {
        return (await _publisherRepository.GetAllAsync())
            .Select(_mapper.Map<PublisherResponseDto>)
            .ToList();
    }

    public async Task<PublisherDetailsDto> GetDetailsByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new BadRequestException("Publisher id must be greater than 0.");
        }

        var publisher = await _publisherRepository.GetDetailsByIdAsync(id);
        if (publisher is null)
        {
            throw new NotFoundException($"Publisher with id '{id}' was not found.");
        }

        return _mapper.Map<PublisherDetailsDto>(publisher);
    }

    public async Task<IReadOnlyList<PublisherDetailsDto>> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("Publisher name is required.");
        }

        var trimmedName = name.Trim();
        if (trimmedName.Length > 50)
        {
            throw new BadRequestException("Publisher name cannot exceed 50 characters.");
        }

        var publishers = await _publisherRepository.GetByNameAsync(trimmedName);
        if (publishers.Count == 0)
        {
            throw new NotFoundException($"Publisher with name '{trimmedName}' was not found.");
        }

        return publishers
            .Select(_mapper.Map<PublisherDetailsDto>)
            .ToList();
    }

    public async Task<PublisherResponseDto> UpdateAsync(int id, PublisherUpdateDto dto)
    {
        if (dto is null)
        {
            throw new BadRequestException("Publisher request body is required.");
        }

        if (id <= 0)
        {
            throw new BadRequestException("Publisher id must be greater than 0.");
        }

        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher is null)
        {
            throw new NotFoundException($"Publisher with id '{id}' was not found.");
        }

        await EnsureStateExistsAsync(dto.StateCode);

        _mapper.Map(dto, publisher);

        _publisherRepository.Update(publisher);
        await _publisherRepository.SaveChangesAsync();

        return _mapper.Map<PublisherResponseDto>(publisher);
    }

    
    private async Task EnsureStateExistsAsync(string? stateCode)
    {
        if (string.IsNullOrWhiteSpace(stateCode))
        {
            return;
        }

        var trimmedStateCode = stateCode.Trim();
        if (!await _publisherRepository.StateExistsAsync(trimmedStateCode))
        {
            throw new NotFoundException($"State with code '{trimmedStateCode}' was not found.");
        }
    }
}
