namespace BookStoreApplication.MVC.DTOs.Author
{
 
    public class AuthorWithBooksResponseDTO
    {
        public int AuthorId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Photo { get; set; }
        public List<BookSummaryDTO> Books { get; set; } = new();
    }

    public class BookSummaryDTO
    {
        public string Isbn { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
    
