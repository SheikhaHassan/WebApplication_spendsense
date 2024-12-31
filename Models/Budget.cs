using System;
using System.Collections.Generic;

namespace budgetManagement.Models;

public partial class Budget
{
    public int BudgetId { get; set; }

    public int UserId { get; set; }

    public decimal TotalIncome { get; set; }

    public decimal Needs { get; set; }

    public decimal Wants { get; set; }

    public decimal Savings { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
