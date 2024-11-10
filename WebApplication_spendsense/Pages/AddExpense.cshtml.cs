using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_spendsense.Pages
{
    public class AddExpenseModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter the expense description")]
        public string Expense { get; set; }

        [BindProperty]
        [Range(0.01, 100000, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a date")]
        public DateTime Date { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a category")]
        public string Category { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public bool IsSuccess { get; set; } = false;

        public void OnGet()
        {
            Categories = GetCategories();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Categories = GetCategories(); // Keep categories after validation fails
                return Page();
            }



            IsSuccess = true;
            ModelState.Clear(); // Clear the form after successful submission

            return Page();
        }

        private List<SelectListItem> GetCategories()
        {

            return new List<SelectListItem>
        {
            new SelectListItem { Value = "Food", Text = "Food" },
            new SelectListItem { Value = "Transport", Text = "Transport" },
            new SelectListItem { Value = "Utilities", Text = "Utilities" },
            new SelectListItem { Value = "Entertainment", Text = "Entertainment" }
        };

        }
    }
}
