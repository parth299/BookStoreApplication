using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Models;
using AutoMapper;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Repositories.Book_Publisher;

namespace BookStoreApplication.Web.Services.Book_Publisher;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public BookService(IBookRepository bookRepository, IPublisherRepository publisherRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    public async Task<BookResponseDto> CreateAsync(BookCreateDto dto)
    {
        if (dto is null)
        {
            throw new BadRequestException("Book request body is required.");
        }

        var isbn = NormalizeIsbn(dto.Isbn);
        if (await _bookRepository.ExistsAsync(isbn))
        {
            throw new ConflictException($"Book with ISBN '{isbn}' already exists.");
        }

        if (!await _publisherRepository.ExistsAsync(dto.PublisherId))
        {
            throw new NotFoundException($"Publisher with id '{dto.PublisherId}' was not found.");
        }

        if (dto.Category.HasValue && !await _bookRepository.CategoryExistsAsync(dto.Category.Value))
        {
            throw new NotFoundException($"Category with id '{dto.Category.Value}' was not found.");
        }

        var book = _mapper.Map<Book>(dto);

        await _bookRepository.AddBookAsync(book);
        await _bookRepository.SaveChangesAsync();

        var created = await _bookRepository.GetBookByIsbnAsync(book.Isbn);
        return _mapper.Map<BookResponseDto>(created!);
    }

    public async Task<IReadOnlyList<BookResponseDto>> GetAllBooksAsync()
    {
        return (await _bookRepository.GetAllBooksAsync())
            .Select(_mapper.Map<BookResponseDto>)
            .ToList();
    }

    public async Task<BookResponseDto> GetByIsbnAsync(string isbn)
    {
        var trimmedIsbn = NormalizeIsbn(isbn);
        var book = await _bookRepository.GetBookByIsbnAsync(trimmedIsbn);
        if (book is null)
        {
            throw new NotFoundException($"Book with ISBN '{trimmedIsbn}' was not found.");
        }

        return _mapper.Map<BookResponseDto>(book);
    }

    public async Task<BookResponseDto> UpdateAsync(string isbn, BookUpdateDto dto)
    {
        if (dto is null)
        {
            throw new BadRequestException("Book request body is required.");
        }

        var trimmedIsbn = NormalizeIsbn(isbn);
        var book = await _bookRepository.GetBookByIsbnAsync(trimmedIsbn);
        if (book is null)
        {
            throw new NotFoundException($"Book with ISBN '{trimmedIsbn}' was not found.");
        }

        if (!await _publisherRepository.ExistsAsync(dto.PublisherId))
        {
            throw new NotFoundException($"Publisher with id '{dto.PublisherId}' was not found.");
        }

        if (dto.Category.HasValue && !await _bookRepository.CategoryExistsAsync(dto.Category.Value))
        {
            throw new NotFoundException($"Category with id '{dto.Category.Value}' was not found.");
        }

        _mapper.Map(dto, book);

        _bookRepository.UpdateBook(book);
        await _bookRepository.SaveChangesAsync();

        return _mapper.Map<BookResponseDto>(book);
    }

    

    public async Task<IReadOnlyList<BookResponseDto>> FilterAsync(BookFilterDto filter)
    {
        if (filter is null)
        {
            throw new BadRequestException("Book filter values are required.");
        }

        if (filter.Category.HasValue && !await _bookRepository.CategoryExistsAsync(filter.Category.Value))
        {
            throw new NotFoundException($"Category with id '{filter.Category.Value}' was not found.");
        }

        if (filter.PublisherId.HasValue && !await _publisherRepository.ExistsAsync(filter.PublisherId.Value))
        {
            throw new NotFoundException($"Publisher with id '{filter.PublisherId.Value}' was not found.");
        }

        return (await _bookRepository.FilterBooksAsync(filter))
            .Select(_mapper.Map<BookResponseDto>)
            .ToList();
    }

    public async Task<IReadOnlyList<BookResponseDto>> SearchAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BadRequestException("Search title is required.");
        }

        return (await _bookRepository.SearchByTitleAsync(title.Trim()))
            .Select(_mapper.Map<BookResponseDto>)
            .ToList();
    }

    public async Task<IReadOnlyList<BookTopRatedDto>> GetTopRatedAsync(int count)
    {
        if (count < 1 || count > 100)
        {
            throw new BadRequestException("Count must be between 1 and 100.");
        }

        return await _bookRepository.GetTopRatedAsync(count);
    }

    private static string NormalizeIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new BadRequestException("ISBN is required.");
        }

        var trimmedIsbn = isbn.Trim();
        if (trimmedIsbn.Length is < 10 or > 13)
        {
            throw new BadRequestException("ISBN must be between 10 and 13 characters.");
        }

        return trimmedIsbn;
    }
}
