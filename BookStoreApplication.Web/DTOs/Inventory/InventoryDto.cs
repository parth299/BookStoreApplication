namespace BookStoreApplication.Web.DTOs.Inventory
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }

        public string ISBN { get; set; } = null!;

        public int ConditionRank { get; set; }

        public byte? Purchased { get; set; }
    }
    public class CreateInventoryDto
    {
        public string ISBN { get; set; } = null!;

        public int ConditionRank { get; set; }

        public byte? Purchased { get; set; }
    }
}
