using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication_spendsense.Model 
{
    public class ExpenseService

    {
       

        private List<ExpenseCategory> Expenses { get; set; } = new List<ExpenseCategory>
        {
            new ExpenseCategory { Category = "Food", ActualExpenses = 300, AllocatedAmount = 500 },
            new ExpenseCategory { Category = "Transport", ActualExpenses = 100, AllocatedAmount = 150 },
            new ExpenseCategory { Category = "Entertainment", ActualExpenses = 150, AllocatedAmount = 200 },
            new ExpenseCategory { Category = "Utilities", ActualExpenses = 250, AllocatedAmount = 300 }
        };

        // Get Expenses with error handling
        public List<ExpenseCategory> GetExpenses(string categoryFilter = null)
        {
            try
            {
                if (categoryFilter != null && !Expenses.Any(e => e.Category == categoryFilter))
                {
                    throw new ArgumentException($"No expenses found for category: {categoryFilter}");
                }

                var filteredExpenses = string.IsNullOrEmpty(categoryFilter)
                    ? Expenses
                    : Expenses.Where(e => e.Category == categoryFilter).ToList();

                if (!filteredExpenses.Any())
                {
                    throw new Exception("No expenses found matching the filter criteria.");
                }

                return filteredExpenses;
            }
            catch (ArgumentException ex)
            {
                // Log error (use logging library in production)
                Console.WriteLine(ex.Message);
                throw new Exception($"Invalid category filter: {ex.Message}. Please try again.");
            }
            catch (Exception ex)
            {
                // General error handling (e.g., server issues)
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new Exception("An unexpected error occurred while retrieving expenses. Please try again later.");
            }
        }

        // Get Categories with error handling
        public List<string> GetCategories()
        {
            try
            {
                var categories = Expenses.Select(e => e.Category).Distinct().ToList();

                if (!categories.Any())
                {
                    throw new Exception("No categories available.");
                }

                return categories;
            }
            catch (Exception ex)
            {
                // Log error (use logging library in production)
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                throw new Exception("An error occurred while retrieving the categories. Please try again later.");
            }
        }

        // Sort Expenses with error handling
        public List<ExpenseCategory> SortExpenses(bool descending)
        {
            try
            {
                if (Expenses == null || !Expenses.Any())
                {
                    throw new InvalidOperationException("Expense list is empty. Unable to sort.");
                }

                return descending
                    ? Expenses.OrderByDescending(e => e.ActualExpenses).ToList()
                    : Expenses.OrderBy(e => e.ActualExpenses).ToList();
            }
            catch (InvalidOperationException ex)
            {
                // Log error (use logging library in production)
                Console.WriteLine(ex.Message);
                throw new Exception("No expenses to sort. Please check your data.");
            }
            catch (Exception ex)
            {
                // General error handling (e.g., server issues)
                Console.WriteLine($"Error sorting expenses: {ex.Message}");
                throw new Exception("An error occurred while sorting the expenses. Please try again later.");
            }
        }
    }
}
