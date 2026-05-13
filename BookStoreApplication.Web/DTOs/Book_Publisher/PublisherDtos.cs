namespace BookStoreApplication.Web.DTOs.Book_Publisher;

public class PublisherCreateDto
{
    public int PublisherId { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? StateCode { get; set; }
}

public class PublisherUpdateDto
{
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? StateCode { get; set; }
}

public class PublisherResponseDto
{
    public int PublisherId { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? StateCode { get; set; }
}

public class PublisherDetailsDto
{
    public int PublisherId { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? StateCode { get; set; }
    public string? StateName { get; set; }
    public IReadOnlyList<BookResponseDto> Books { get; set; } = [];
}
