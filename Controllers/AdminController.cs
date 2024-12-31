using budgetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace budgetManagement.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult user()
        {
            BudgetContext context = new BudgetContext();
            var user= context.Users;
            return View(user);
        }

        [HttpGet]
        public IActionResult AddUser()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User u)
        {
            if (ModelState.IsValid)
            {
                BudgetContext context = new BudgetContext();
                context.Users.Add(u);
                context.SaveChanges();

                ViewBag.Message = $"User {u.Name} was successfully added";
                return View("Message");
            }
            return View();
        }


        [HttpGet]
        public IActionResult EditUser(int UserId)
        {
            BudgetContext context = new BudgetContext();

            var u = context.Users.Find(UserId);
            if (u == null)
            {
                ViewBag.Message = $"User not found. {UserId}";
                return View("Message");
            }

            return View(u);
        }

        [HttpPost]
        public IActionResult EditUser(User u)
        {
            if (ModelState.IsValid)
            {
                BudgetContext context = new BudgetContext();
                context.Users.Update(u);
                context.SaveChanges();
                ViewBag.Message = "User has been updated";
                return View("Message");
            }

            return View(u);
        }


        [HttpGet]
        public IActionResult DeleteUser(int UserId)
        {
            using (var context = new BudgetContext())
            {
                var user = context.Users.Find(UserId);
                if (user == null)
                {
                    ViewBag.Message = $"User not found. {UserId}";
                    return View("Message");
                }

                return View(user);
            }
        }

        [HttpPost]
        public IActionResult ConfirmDeleteUser(int UserId)
        {
            using (var context = new BudgetContext())
            {
                var user = context.Users.Find(UserId);
                if (user == null)
                {
                    ViewBag.Message = $"User not found. {UserId}";
                    return View("Message");
                }

                context.Users.Remove(user);
                context.SaveChanges();
                ViewBag.Message = "User has been deleted";
                return View("Message");
            }
        }

        [HttpGet]
        public IActionResult SearchUser()
        {
            // Render the search form
            return View();
        }

        [HttpPost]
        public IActionResult SearchUser(string searchTerm)
        {
            using (var context = new BudgetContext())
            {
                // Search by name, username, or email (adjust as needed)
                var results = context.Users
                    .Where(u => u.Name.Contains(searchTerm) ||
                                u.Username.Contains(searchTerm) ||
                                u.Email.Contains(searchTerm))
                    .ToList();

                if (!results.Any())
                {
                    ViewBag.Message = $"No users found for '{searchTerm}'.";
                    return View();
                }

                return View("SearchResults", results); // Render results view
            }
        }

    }
}
