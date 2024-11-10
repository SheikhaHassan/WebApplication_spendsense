using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication_spendsense.Pages
{ 
    public class LoginModel : PageModel
    {
        public void OnGet(string UName,string UPass)
        {
            if (!string.IsNullOrEmpty(UName)) { 
                CookieOptions option = new CookieOptions();


            option.Expires = DateTime.Now.AddDays(30);


            Response.Cookies.Append("name", UName, option);
            Response.Cookies.Append("pass", UPass, option);
            }
        }
    }
}
