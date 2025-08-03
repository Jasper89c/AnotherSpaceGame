using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class DeepSpaceMarketModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // Add this field

        public DeepSpaceMarketModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager) // Add constructor to inject UserManager
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<string> MarketTypes { get; set; }

        [BindProperty]
        [Required]
        public MarketType SelectedMarketType { get; set; }

        [BindProperty]
        [Range(1, long.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public long Quantity { get; set; }

        public string StatusMessage { get; set; }

        public void OnGet()
        {
            MarketTypes = Enum.GetNames(typeof(MarketType));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User); // Fixes CS0103 error
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var commodities = _context.Commodities.FirstOrDefault(f => f.ApplicationUserId == user.Id);

            MarketTypes = Enum.GetNames(typeof(MarketType));
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please select a market type and enter a valid quantity.";
                return Page();
            }
            long price = 0;
            long totalCost = 0;
            switch (SelectedMarketType)
            {
                case MarketType.Food:
                    if (Quantity + commodities.Food > 25000000000)
                    {
                        Quantity = (long)(25000000000 - commodities.Food);
                    }
                    price = 35;
                    totalCost = (long)(Quantity * price);
                    if(totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.Ore:
                    if (Quantity + commodities.Ore > 5000000000)
                    {
                        Quantity = (long)(5000000000 - commodities.Ore);
                    }
                    price = 40000;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.RawMaterial:
                    if (Quantity + commodities.RawMaterial > 5000000000)
                    {
                        Quantity = (long)(5000000000 - commodities.RawMaterial);
                    }
                    price = 5;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.ConsumerGoods:
                    if (Quantity + commodities.ConsumerGoods > 5000000000)
                    {
                        Quantity = (long)(5000000000 - commodities.ConsumerGoods);
                    }
                    price = 5;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.TerranMetal:
                    if (Quantity + commodities.TerranMetal > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.TerranMetal);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.RedCrystal:
                    if (Quantity + commodities.RedCrystal > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.RedCrystal);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.WhiteCrystal:
                    if (Quantity + commodities.WhiteCrystal > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.WhiteCrystal);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.Rutile:
                    if (Quantity + commodities.Rutile > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.Rutile);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.Composite:
                    if (Quantity + commodities.Composite > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.Composite);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                case MarketType.StrafezOrganism:
                    if (Quantity + commodities.StrafezOrganism > 2000000000)
                    {
                        Quantity = (long)(2000000000 - commodities.StrafezOrganism);
                    }
                    price = 1500;
                    totalCost = (long)(Quantity * price);
                    if (totalCost > commodities.Credits)
                    {
                        StatusMessage = "Not enough Credits, Enter lower amount to buy.";
                        return Page();
                    }
                    commodities.Food += Quantity;
                    commodities.Credits -= totalCost;
                    break;
                default:
                    break;
            }
            await _context.SaveChangesAsync();

            StatusMessage = $"Successfully purchased {Quantity:N0} units of {SelectedMarketType}.";

            return Page();
        }
    }
}
