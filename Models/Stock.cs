using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Stock
{
    public int Id { get; set; }

    public int ObjectId { get; set; }

    public int? Quantity { get; set; }

    public virtual Object Object { get; set; } = null!;
}
