using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_spendsense.Pages
{
    public class SignUpModel : PageModel
    {
       [BindProperty]
       public required Model.User proprties { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            return RedirectToPage("SuccessPage");
        }
    }
}
