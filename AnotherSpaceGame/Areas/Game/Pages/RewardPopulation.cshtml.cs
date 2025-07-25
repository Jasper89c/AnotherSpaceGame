using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RewardPopulationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public RewardPopulationModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public Planets Planet { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        [Range(1, 45, ErrorMessage = "Turns must be between 1 and 45.")]
        public int TurnsToUse { get; set; }

        public string RewardMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == user.Id);

            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }

            Id = id;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var commodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == Id && p.ApplicationUserId == user.Id);
            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Calculate total cost
            int totalCost = Planet.CurrentPopulation * 2 * TurnsToUse;

            // Check if user has enough turns
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, TurnsToUse);
            if (!turnResult.Success)
            {
                ModelState.AddModelError(string.Empty, turnResult.Message);
                return Page();
            }

            // Increase loyalty, but do not exceed 5000
            int loyaltyIncrease = 5 * TurnsToUse;
            int newLoyalty = Planet.Loyalty + loyaltyIncrease;
            if (newLoyalty > 5000)
            {
                loyaltyIncrease = 5000 - Planet.Loyalty;
                Planet.Loyalty = 5000;
            }
            else
            {
                Planet.Loyalty = newLoyalty;
            }
            if(Planet.Loyalty > 3000 && user.Faction == Faction.Guardian)
            {
                Planet.Loyalty = 3000; // Ensure loyalty does not go below 0
            }

            // Optionally, deduct credits or resources if needed (not specified in your request)
            commodities.Credits -= totalCost;
            await _context.SaveChangesAsync();

            RewardMessage = $"Loyalty increased by {5 * TurnsToUse}. Total cost: {totalCost:N0} credits.<hr>{turnResult.Message}";
            TempData["RewardMessage"] = RewardMessage;
            return RedirectToPage(new { id = Planet.Id });
        }
    }
}
