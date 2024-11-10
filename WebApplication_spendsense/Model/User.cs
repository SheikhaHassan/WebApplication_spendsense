using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication_spendsense.Model
{
    public enum Role { 
    User,
    Admin
    }
    public class User
    {
       
        [Required(ErrorMessage = "Username is required")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 15 characters")]
        public string Username { get; set; }

       
        [Required(ErrorMessage = "Full Name is required")]
        //[StringLength(15, MinimumLength = 5, ErrorMessage = "Name must be under 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        //[StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        // Enum property for Role 
        [Required(ErrorMessage = "Role is required")]
        public Role UserRole { get; set; }

    }
}
