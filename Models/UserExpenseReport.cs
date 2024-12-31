using System;
using System.Collections.Generic;

namespace budgetManagement.Models;

public partial class UserExpenseReport
{
    public string Uname { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal ExpenseAmount { get; set; }

    public DateOnly ExpenseDate { get; set; }
}
