using System;
using System.Collections.Generic;

namespace InventoryManagement.Models;

public partial class CashFlow
{
    public int Id { get; set; }

    public int? TotalIncome { get; set; }

    public int? TotalCosts { get; set; }
}
