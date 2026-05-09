namespace BookStoreApplication.Web.DTOs
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }

        public string ISBN { get; set; } = null!;

        public int ConditionRank { get; set; }

        public byte? Purchased { get; set; }
    }
}
