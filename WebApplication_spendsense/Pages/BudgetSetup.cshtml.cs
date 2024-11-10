using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_spendsense.Pages
{
    public class BudgetSetupModel : PageModel
    {
        [BindProperty]
        public decimal IncomeAmount { get; set; }
        public decimal TotalIncome { get; private set; }
        public decimal NeedsAllocation { get; private set; }
        public decimal WantsAllocation { get; private set; }
        public decimal SavingsAllocation { get; private set; }

        // Bindable properties for various budget categories
        [BindProperty]
        public decimal RentAmount { get; set; }
        [BindProperty]
        public decimal GroceriesAmount { get; set; }
        [BindProperty]
        public decimal GasAmount { get; set; }
        [BindProperty]
        public decimal GymAmount { get; set; }
        [BindProperty]
        public decimal RestaurantAmount { get; set; }
        [BindProperty]
        public decimal TravelAmount { get; set; }
        [BindProperty]
        public decimal VacationAmount { get; set; }
        [BindProperty]
        public decimal GiftAmount { get; set; }
        [BindProperty]
        public decimal SavingsAmount { get; set; }
        [BindProperty]
        public decimal InvestmentsAmount { get; set; }

        // Error message property to display any validation or exception errors
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Initialize default values or load existing data if necessary
        }

        public IActionResult OnPostCalculate()
        {
            try
            {
                // Validate that the income amount is a positive number
                if (IncomeAmount <= 0)
                {
                    ErrorMessage = "Income amount must be greater than zero.";
                    return Page();
                }

                // Calculate allocations
                TotalIncome += IncomeAmount;
                NeedsAllocation = TotalIncome * 0.50M;
                WantsAllocation = TotalIncome * 0.30M;
                SavingsAllocation = TotalIncome * 0.20M;

                return Page(); // Return the same page with updated allocations
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging library like NLog, Serilog, etc.)
                ErrorMessage = "An error occurred while calculating the budget allocations. Please try again later.";
                return Page(); // Return the same page with an error message
            }
        }

        public IActionResult OnPostSave()
        {
            try
            {
                // Validate that all amounts are non-negative
                if (RentAmount < 0 || GroceriesAmount < 0 || GasAmount < 0 || GymAmount < 0 ||
                    RestaurantAmount < 0 || TravelAmount < 0 || VacationAmount < 0 ||
                    GiftAmount < 0 || SavingsAmount < 0 || InvestmentsAmount < 0)
                {
                    ErrorMessage = "All allocation amounts must be non-negative.";
                    return Page();
                }

                // Store allocations in TempData to display in the report
                TempData["RentAmount"] = RentAmount.ToString();
                TempData["GroceriesAmount"] = GroceriesAmount.ToString();
                TempData["GasAmount"] = GasAmount.ToString();
                TempData["GymAmount"] = GymAmount.ToString();
                TempData["RestaurantAmount"] = RestaurantAmount.ToString();
                TempData["TravelAmount"] = TravelAmount.ToString();
                TempData["VacationAmount"] = VacationAmount.ToString();
                TempData["GiftAmount"] = GiftAmount.ToString();
                TempData["SavingsAmount"] = SavingsAmount.ToString();
                TempData["InvestmentsAmount"] = InvestmentsAmount.ToString();



                return RedirectToPage("Report");
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging service here)
                ErrorMessage = "An error occurred while saving your budget allocations. Please try again later.";
                return Page(); // Return to the same page with an error message
            }
        }
    }
}
