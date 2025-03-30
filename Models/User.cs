using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class User
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual Role Role { get; set; } = null!;
}
