namespace BookStoreApplication.MVC.DTOs.Book_Publisher;

public class BookCreateDto
{
    public string Isbn { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? Category { get; set; }
    public string? Edition { get; set; }
    public int PublisherId { get; set; }
}

public class BookUpdateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? Category { get; set; }
    public string? Edition { get; set; }
    public int PublisherId { get; set; }
}

public class BookResponseDto
{
    public string Isbn { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? Category { get; set; }
    public string? Edition { get; set; }
    public int PublisherId { get; set; }
    public string? PublisherName { get; set; }
}

public class BookFilterDto
{
    public int? Category { get; set; }
    public int? PublisherId { get; set; }
    public string? Edition { get; set; }
    public string? Title { get; set; }
}

public class BookTopRatedDto
{
    public string Isbn { get; set; } = null!;
    public string Title { get; set; } = null!;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}
