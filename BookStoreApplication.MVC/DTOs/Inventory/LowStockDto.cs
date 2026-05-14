namespace BookStoreApplication.MVC.DTOs.Inventory
{
    public class LowStockDto
    {
        public string ISBN { get; set; } = string.Empty;

        public int AvailableCopies { get; set; }
    }
}
