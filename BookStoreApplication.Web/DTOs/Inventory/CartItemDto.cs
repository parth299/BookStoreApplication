namespace BookStoreApplication.Web.DTOs.Inventory
{
    public class CartItemDto
    {
        public int UserId { get; set; }
        public string ISBN { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
