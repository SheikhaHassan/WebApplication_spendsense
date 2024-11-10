using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication_spendsense.Pages
{
    public class reportModel : PageModel
    {
        public decimal RentAmount { get; private set; }
        public decimal GroceriesAmount { get; private set; }
        public decimal GasAmount { get; private set; }
        public decimal GymAmount { get; private set; }
        public decimal RestaurantAmount { get; private set; }
        public decimal TravelAmount { get; private set; }
        public decimal VacationAmount { get; private set; }
        public decimal GiftAmount { get; private set; }
        public decimal SavingsAmount { get; private set; }
        public decimal InvestmentsAmount { get; private set; }


        public void OnGet()
        {
            // Retrieve the saved allocations from TempData and convert them back to decimal
            RentAmount = Convert.ToDecimal(TempData["RentAmount"] ?? "0");
            GroceriesAmount = Convert.ToDecimal(TempData["GroceriesAmount"] ?? "0");
            GasAmount = Convert.ToDecimal(TempData["GasAmount"] ?? "0");
            GymAmount = Convert.ToDecimal(TempData["GymAmount"] ?? "0");
            RestaurantAmount = Convert.ToDecimal(TempData["RestaurantAmount"] ?? "0");
            TravelAmount = Convert.ToDecimal(TempData["TravelAmount"] ?? "0");
            VacationAmount = Convert.ToDecimal(TempData["VacationAmount"] ?? "0");
            GiftAmount = Convert.ToDecimal(TempData["GiftAmount"] ?? "0");
            SavingsAmount = Convert.ToDecimal(TempData["SavingsAmount"] ?? "0");
            InvestmentsAmount = Convert.ToDecimal(TempData["InvestmentsAmount"] ?? "0");
        }
    }
}
