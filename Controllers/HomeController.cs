using budgetManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace budgetManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.ErrorMessage = "Both Username/Email and Password are required.";
                return View(model);
            }

            // Validate user credentials and fetch userId from the database
            BudgetContext context = new BudgetContext();
            var user = context.Users
                .FirstOrDefault(u => (u.Email == model.Username || u.Username == model.Username) && u.Password == model.Password);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid login credentials. Please try again.";
                return View(model);
            }

            // Log the user data for debugging
            _logger.LogInformation("User logged in: {Username}, Role: {Role}, UserId: {UserId}", user.Username, user.Role, user.UserId);

            // Store session data, including UserId
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role ?? "User");
            HttpContext.Session.SetInt32("UserId", user.UserId); // Set the UserId from the database

            // Log the session data for debugging
            _logger.LogInformation("Session Set: UserName = {UserName}, UserRole = {UserRole}, UserId = {UserId}",
                                    HttpContext.Session.GetString("UserName"),
                                    HttpContext.Session.GetString("UserRole"),
                                    HttpContext.Session.GetInt32("UserId"));

            // Redirect to Dashboard
            return RedirectToAction("Dashboard", "Home");
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new User());
        }

        // Handle user registration
        [HttpPost]
        public IActionResult SignUp(User model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                BudgetContext context = new BudgetContext();
                var existingUser = context.Users
                    .FirstOrDefault(u => u.Email == model.Email || u.Username == model.Username);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "An account with this email or username already exists.";
                    return View(model);
                }



                // Create new user
                context.Users.Add(model);
                context.SaveChanges();
                ViewBag.SuccessMessage = "Account created successfully! You can now log in.";

                // Redirect to Login page
                return RedirectToAction("Login");
            }

            return View(model);
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();

            TempData["LogoutMessage"] = "You have been successfully logged out.";


            return RedirectToAction("Index", "Home");
        }


        public IActionResult Dashboard()
        {
            // Retrieve user information from session
            string userName = HttpContext.Session.GetString("UserName");
            string userRole = HttpContext.Session.GetString("UserRole");
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Redirect to login page if user is not authenticated
            if (string.IsNullOrEmpty(userName) || userId == null)
            {
                return RedirectToAction("Login");
            }

            // Fetch dashboard data for the logged-in user from DashboardSummaries table
            BudgetContext context = new BudgetContext();
            var dashboardData = context.DashboardSummaries
                .FirstOrDefault(d => d.UserId == userId.Value);

            // Return default values if no data is found
            if (dashboardData == null)
            {
                dashboardData = new DashboardSummary
                {
                    TotalIncome = 0,
                    TotalExpense = 0,
                    RemainingBudget = 0
                };
            }

            // Pass the dashboard data to the view
            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;
            ViewBag.TotalIncome = dashboardData.TotalIncome;
            ViewBag.TotalExpense = dashboardData.TotalExpense;
            ViewBag.RemainingBudget = dashboardData.RemainingBudget ?? 0;

            return View(dashboardData); // Pass the dashboardData model to the view if needed
        }

        [HttpGet]
        public IActionResult BudgetSetup()
        {
            // Retrieve user information from session
            string userName = HttpContext.Session.GetString("UserName");
            string userRole = HttpContext.Session.GetString("UserRole");
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Redirect to login page if user is not authenticated
            if (string.IsNullOrEmpty(userName) || userId == null)
            {
                return RedirectToAction("Login");
            }

            // Fetch budget data for the logged-in user
            BudgetContext context = new BudgetContext();
            var budget = context.Budgets.FirstOrDefault(b => b.UserId == userId);

            // If no budget data found, create default values
            if (budget == null)
            {
                budget = new Budget
                {
                    UserId = userId.Value,
                    TotalIncome = 0,
                    Needs = 0,
                    Wants = 0,
                    Savings = 0
                };
            }

            // Calculate allocations based on TotalIncome (if not 0)
            ViewBag.TotalIncome = budget.TotalIncome;
            ViewBag.NeedsAllocation = budget.TotalIncome * 0.5m;
            ViewBag.WantsAllocation = budget.TotalIncome * 0.3m;
            ViewBag.SavingsAllocation = budget.TotalIncome * 0.2m;

            return View(budget);
        }

        [HttpPost]
        public IActionResult BudgetSetup(Budget model)
        {
            // Retrieve user information from session
            string userName = HttpContext.Session.GetString("UserName");
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Redirect to login page if user is not authenticated
            if (string.IsNullOrEmpty(userName) || userId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                // Check if budget data already exists
                BudgetContext context = new BudgetContext();
                var existingBudget = context.Budgets.FirstOrDefault(b => b.UserId == userId);

                if (existingBudget != null)
                {
                    // Update existing budget
                    existingBudget.TotalIncome = model.TotalIncome;
                    existingBudget.Needs = model.TotalIncome * 0.5m;
                    existingBudget.Wants = model.TotalIncome * 0.3m;
                    existingBudget.Savings = model.TotalIncome * 0.2m;
                }
                else
                {
                    // Create a new budget if not exists
                    context.Budgets.Add(new Budget
                    {
                        UserId = userId.Value,
                        TotalIncome = model.TotalIncome,
                        Needs = model.TotalIncome * 0.5m,
                        Wants = model.TotalIncome * 0.3m,
                        Savings = model.TotalIncome * 0.2m
                    });
                }

                // Save the changes to the database
                context.SaveChanges();
                ViewBag.Message = "Budget data updated successfully!";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddExpense()
        {
            string userName = HttpContext.Session.GetString("UserName");
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(userName) || userId == null)
            {
                return RedirectToAction("Login");
            }

            // Get categories from the database for the dropdown list
            using (BudgetContext context = new BudgetContext())
            {
                // Get categories from the database
                var categories = context.Categories.ToList();

                // Convert categories to SelectListItem list
                var selectListItems = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

                // Pass the SelectListItem list to the ViewBag
                ViewBag.Categories = selectListItems;
            }
            return View("AddExpense");
        }

        [HttpPost]
        public IActionResult AddExpense(Expense model)
        {
            string userName = HttpContext.Session.GetString("UserName");
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(userName) || userId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                using (BudgetContext context = new BudgetContext())
                {
                    // Add the expense to the database
                    model.UserId = userId.Value; // Assign the logged-in user's ID
                    context.Expenses.Add(model);
                    context.SaveChanges();
                }

                TempData["SuccessMessage"] = "Expense added successfully!";
                return RedirectToAction("Dashboard");
            }

            // If the model is invalid, re-render the AddExpense view
            return View(model);
        }


        // GET: Category/Update
        public IActionResult Update()
        {
            // Retrieve user session details
            string userName = HttpContext.Session.GetString("UserName");
            string userRole = HttpContext.Session.GetString("UserRole");

            // Redirect to login if user is not authenticated
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["Title"] = "Update Category";
            return View();
        }

        // POST: Category/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserCategorySpending model)
        {
            if (ModelState.IsValid)
            {
                string userName = HttpContext.Session.GetString("UserName");
                string userRole = HttpContext.Session.GetString("UserRole");

                if (string.IsNullOrEmpty(userName))
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    int userId = 1;
                    BudgetContext context = new BudgetContext();
                    // Use LINQ to find the category and update it
                    var category = context.UserCategorySpendings
                        .FirstOrDefault(c => c.UserId == userId && c.CategoryName == model.CategoryName);

                    if (category == null)
                    {
                        TempData["Message"] = "Error: Category not found or no amount to update.";
                    }
                    else
                    {
                        category.AmountSpent = model.AmountSpent;
                        context.SaveChanges(); // Save the changes to the database

                        TempData["Message"] = "Category amount successfully updated.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error updating category: {ex.Message}";
                }

                return RedirectToAction("Update");
            }

            return View(model);
        }

        // POST: Category/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string categoryName)
        {
            try
            {
                int userId = 1; // Set to the logged-in user ID
                BudgetContext context = new BudgetContext();

                // Use LINQ to find and delete the category
                var category = context.UserCategorySpendings
                    .FirstOrDefault(c => c.UserId == userId && c.CategoryName == categoryName);

                if (category == null)
                {
                    TempData["Message"] = "Error: Category not found or already deleted.";
                }
                else
                {
                    context.UserCategorySpendings.Remove(category);
                    context.SaveChanges(); // Save the changes to the database

                    TempData["Message"] = "Category successfully deleted.";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error deleting category: {ex.Message}";
            }

            return RedirectToAction("Update");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}