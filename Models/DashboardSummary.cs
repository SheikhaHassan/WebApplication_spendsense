using System;
using System.Collections.Generic;

namespace budgetManagement.Models;

public partial class DashboardSummary
{
    public int UserId { get; set; }

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal? RemainingBudget { get; set; }
}
