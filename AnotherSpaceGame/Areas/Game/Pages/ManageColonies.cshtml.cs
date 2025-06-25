using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ManageColoniesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService; // Inject your turn service

        [BindProperty]
        public List<int> PlunderPlanetIds { get; set; }

        public ManageColoniesModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public IList<Planets> UserPlanets { get; set; } = new List<Planets>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (user != null)
            {
                UserPlanets = await _context.Planets
                    .Where(p => p.ApplicationUserId == user.Id)
                    .OrderByDescending(p => p.DateTimeAcquired) // Order by acquired date descending
                    .ToListAsync();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var planet = await _context.Planets
                .Where(p => p.Id == id && p.ApplicationUserId == user.Id)
                .FirstOrDefaultAsync();

            if (planet == null)
            {
                return NotFound();
            }

            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();

            // Optionally, update the user's total colonies/planets here if needed

            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostBatchPlunderAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            if (PlunderPlanetIds == null || !PlunderPlanetIds.Any())
            {
                TempData["TurnMessage"] = "No planets selected for plunder.";
                return RedirectToPage();
            }

            int totalTurnsRequired = PlunderPlanetIds.Count * 5;
            var userTurns = await _context.Turns.FirstOrDefaultAsync(t => t.ApplicationUserId == user.Id);

            if (userTurns == null || userTurns.CurrentTurns < totalTurnsRequired)
            {
                TempData["TurnMessage"] = $"Not enough turns. You need {totalTurnsRequired} turns to plunder {PlunderPlanetIds.Count} planet(s).";
                return RedirectToPage();
            }

            // Example: Plunder logic (customize as needed)
            var planets = await _context.Planets
                .Where(p => PlunderPlanetIds.Contains(p.Id) && p.ApplicationUserId == user.Id)
                .ToListAsync();

            foreach (var planet in planets)
            {
                // Your plunder logic here (e.g., resources, status, etc.)
            }

            userTurns.CurrentTurns -= totalTurnsRequired;
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, totalTurnsRequired); // Use your turn service to save

            TempData["TurnMessage"] = $"Successfully plundered {planets.Count} planet(s) for {totalTurnsRequired} turns.<hr> {turnResult.Message}";
            return RedirectToPage();
        }
    }
}
