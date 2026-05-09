namespace BookStoreApplication.Web.DTOs
{
    public class CategoryResponseDto
    {
        public int CatId { get; set; }
        public string CatDescription { get; set; } = null!;
    }
}