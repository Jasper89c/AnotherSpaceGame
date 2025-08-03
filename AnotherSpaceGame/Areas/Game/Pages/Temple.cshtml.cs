using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class TempleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;
        public long TempleHeight { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public IEnumerable<string> MarketTypes { get; set; }

        [BindProperty]
        [Required]
        public MarketType SelectedMarketType { get; set; }

        [BindProperty]
        [Range(1, long.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public long Quantity { get; set; }
        public string StatusMessage { get; set; }
        public Commodities Commodities { get; set; }

        public TempleModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Commodities = _context.Commodities.FirstOrDefault(f => f.ApplicationUserId == user.Id);
            TempleHeight = user.TempleHeight;
            Users = _context.Users.OrderByDescending(u => u.TempleHeight).Take(25).ToList();
            MarketTypes = Enum.GetNames(typeof(MarketType))
            .Where(x => x != nameof(MarketType.ConsumerGoods) && x != nameof(MarketType.RawMaterial) && x != nameof(MarketType.Food));
            return Page();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            MarketTypes = Enum.GetNames(typeof(MarketType))
            .Where(x => x != nameof(MarketType.ConsumerGoods) && x != nameof(MarketType.RawMaterial) && x != nameof(MarketType.Food));
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            TempleHeight = user.TempleHeight;
            Users = _context.Users.OrderByDescending(u => u.TempleHeight).Take(25).ToList();
            Commodities = _context.Commodities.FirstOrDefault(f => f.ApplicationUserId == user.Id);
            var metersToAdd = 0;
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please select a market type and enter a valid quantity.";
                return Page();
            }

            // Handle the donation logic here
            // Example: Add the quantity of the selected market type to the user's temple, deduct resources, etc.
            switch (SelectedMarketType)
            {
                case MarketType.Ore:
                    if (Quantity > user.Commodities.Ore)
                    {
                        StatusMessage = "Not enough Ore to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 1000000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 5000000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 10000000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.Ore -= Quantity;
                    }
                        break;
                case MarketType.TerranMetal:
                    if (Quantity > user.Commodities.TerranMetal)
                    {
                        StatusMessage = "Not enough Terran Metal to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.TerranMetal -= Quantity;
                    }
                        break;
                case MarketType.RedCrystal:
                    if (Quantity > user.Commodities.RedCrystal)
                    {
                        StatusMessage = "Not enough Red Crystal to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.RedCrystal -= Quantity;
                    }
                    break;
                case MarketType.WhiteCrystal:
                    if (Quantity > user.Commodities.WhiteCrystal)
                    {
                        StatusMessage = "Not enough White Crystal to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.WhiteCrystal -= Quantity;
                    }
                    break;
                case MarketType.Rutile:
                    if (Quantity > user.Commodities.Rutile)
                    {
                        StatusMessage = "Not enough Rutile to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.Rutile -= Quantity;
                    }
                    break;
                case MarketType.Composite:
                    if (Quantity > user.Commodities.Composite)
                    {
                        StatusMessage = "Not enough Composite to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.Composite -= Quantity;
                    }
                    break;
                case MarketType.StrafezOrganism:
                    if (Quantity > user.Commodities.StrafezOrganism)
                    {
                        StatusMessage = "Not enough Strafez Organism to donate.";
                        return Page();
                    }
                    else
                    {
                        if (Quantity == 2750000)
                        {
                            metersToAdd = 1;
                        }
                        else if (Quantity == 13750000)
                        {
                            metersToAdd = 5;
                        }
                        else if (Quantity == 27500000)
                        {
                            metersToAdd = 10;
                        }
                        Commodities.StrafezOrganism -= Quantity;
                    }
                    break;
                default:
                    break;
            }
            if (metersToAdd > 0)
            {
                user.TempleHeight += metersToAdd;
                _context.Update(user);
                await _context.SaveChangesAsync();
                StatusMessage = $"Successfully donated {Quantity:N0} units of {SelectedMarketType} to the temple!";
            }
            else
            {
                StatusMessage = "Invalid donation amount or type selected.";
                return Page();
            }
            TempleHeight = user.TempleHeight;
            return Page();
        }
    }
}
