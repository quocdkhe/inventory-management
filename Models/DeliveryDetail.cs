using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class DeliveryDetail
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }

    public int ObjectId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public virtual Delivery Delivery { get; set; } = null!;

    public virtual Object Object { get; set; } = null!;
}
