using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Receipt
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public DateTime? Date { get; set; }

    public int UserId { get; set; }

    public int? TotalPrice { get; set; }

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
