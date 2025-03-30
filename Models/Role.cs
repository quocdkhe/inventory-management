using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? DisplayName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
