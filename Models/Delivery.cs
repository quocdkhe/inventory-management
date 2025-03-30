using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Delivery
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime? Date { get; set; }

    public int UserId { get; set; }

    public int? TotalPrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; } = new List<DeliveryDetail>();

    public virtual User User { get; set; } = null!;
}
