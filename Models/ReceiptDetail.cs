using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class ReceiptDetail
{
    public int Id { get; set; }

    public int ReceiptId { get; set; }

    public int ObjectId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public virtual Object Object { get; set; } = null!;

    public virtual Receipt Receipt { get; set; } = null!;
}
