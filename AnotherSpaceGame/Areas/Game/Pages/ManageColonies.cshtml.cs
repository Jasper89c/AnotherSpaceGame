using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGetAsync()
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
            return Page();
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
            var UserExploration = await _context.Explorations
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            var UserCommodities = await _context.Commodities
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
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
            var totalCreditsForPlunder = 0;
            foreach (var planet in planets)
            {
                if (planet.Name.Contains("H."))
                {

                }
                else
                {
                    var totalPlanetInfra = planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining;
                    var mod = GetFactionModifiers(user.Faction);
                    var creditsGainedForPlunder = ((int)Math.Floor(((planet.CurrentPopulation * 2500) + ((5500 * (totalPlanetInfra ^ 2)) / planet.TotalLand) + (200000 * planet.TotalPlanets) / 15) * mod.FactionPlunderModifier)).ToString("C");
                    var creditsGained = (int)Math.Floor(((planet.CurrentPopulation * 2500) + ((5500 * (totalPlanetInfra ^ 2)) / planet.TotalLand) + (200000 * planet.TotalPlanets) / 15) * mod.FactionPlunderModifier);
                    UserCommodities.Credits += creditsGained;
                    totalCreditsForPlunder += creditsGained;
                    UserExploration.ExplorationPointsNeeded = (int)Math.Floor(UserExploration.ExplorationPointsNeeded / 1.2);
                    user.TotalColonies -= 1;
                    user.TotalPlanets -= planet.TotalPlanets;
                    user.PlanetsPlundered += planet.TotalPlanets;
                    _context.Planets.Remove(planet); // Remove the planet from the database
                }
            }

            userTurns.CurrentTurns -= totalTurnsRequired;
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, totalTurnsRequired); // Use your turn service to save
            TempData["PlunderMessage"] = "Credits For Plunder: " + totalCreditsForPlunder.ToString("C");
            TempData["TurnMessage"] = $"Successfully plundered {planets.Count} planet(s) for {totalTurnsRequired} turns.<hr> {turnResult.Message}";
            return RedirectToPage();
        }

        private (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier,
                 decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost, decimal FactionPlunderModifier)
            GetFactionModifiers(Faction faction)
        {
            return faction switch
            {
                Faction.Terran => (1.0m, 1.1m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m, 0.5m),
                Faction.AMiner => (2.2m, 0.05m, 3.5m, 0.5m, 29m, 1.0m, 0.8m, 0.05m),
                Faction.Marauder => (0.5m, 0.05m, 0.5m, 0.5m, 0.5m, 1.0m, 0.05m, 50m),
                Faction.Viral => (1.2m, 0.95m, 0.5m, 0.8m, 1.0m, 2.0m, 1.0m, 0.1m),
                Faction.Collective => (0.5m, 0.05m, 0.1m, 1.5m, 0.5m, 0.05m, 1.0m, 11m),
                Faction.Guardian => (0.15m, 0.01m, 0.01m, 0.05m, 0.01m, 0.1m, 0.75m, 0.05m),
                Faction.KalZul => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m, 0.05m),
                Faction.DarkMarauder => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m, 0.05m),
                _ => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m)
            };
        }
    }
}
