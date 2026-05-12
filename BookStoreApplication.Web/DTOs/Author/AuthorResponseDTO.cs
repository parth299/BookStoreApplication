namespace BookStoreApplication.Web.DTOs.Author
{
    public class AuthorResponseDTO
    {
        public int AuthorId { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Photo { get; set; }
    }
}
