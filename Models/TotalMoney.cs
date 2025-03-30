using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class TotalMoney
{
    public int Id { get; set; }

    public int? TotalIncome { get; set; }

    public int? TotalCosts { get; set; }
}
