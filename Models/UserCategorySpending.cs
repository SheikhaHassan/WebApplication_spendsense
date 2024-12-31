using System;
using System.Collections.Generic;

namespace budgetManagement.Models;

public partial class UserCategorySpending
{
    public int RecordId { get; set; }

    public int UserId { get; set; }

    public string CategoryName { get; set; } = null!;

    public decimal? AmountSpent { get; set; }

    public virtual User User { get; set; } = null!;
}
