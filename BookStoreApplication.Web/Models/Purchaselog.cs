using System;
using System.Collections.Generic;

namespace BookStoreApplication.Web.Models;

public partial class Purchaselog
{
    public int UserId { get; set; }

    public int InventoryId { get; set; }

    public virtual User User { get; set; } = null!;
}
