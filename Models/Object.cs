using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Object
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public int UnitId { get; set; }

    public int SupplierId { get; set; }

    public string? Qrcode { get; set; }

    public string? BarCode { get; set; }

    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual ICollection<ReceiptDetail> ReceiptDetails { get; set; } = new List<ReceiptDetail>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
