using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class PlunderColonyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public PlunderColonyModel(
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
        public string PlunderMessage { get; set; }
        public string creditsGainedForPlunder { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == user.Id);

            // Check if the planet exists and belongs to the current user
            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                // Redirect to ManageColonies if not authorized
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }
            var totalPlanetInfra = Planet.Housing + Planet.Commercial + Planet.Industry + Planet.Agriculture + Planet.Mining;
            var mod = GetFactionModifiers(user.Faction);
            creditsGainedForPlunder = ((int)Math.Floor(((Planet.CurrentPopulation * 2500) + ((5500 * (totalPlanetInfra ^ 2)) / Planet.TotalLand) + (200000 * Planet.TotalPlanets) / 15) * mod.FactionPlunderModifier)).ToString("C");

            Id = id;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var userExploration = await _context.Explorations.FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            var UserCommodities = await _context.Commodities
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            // Include Commodities by explicitly loading them after retrieving the user
            await _context.Entry(user).Reference(u => u.Commodities).LoadAsync();

            // Find the planet and check ownership
            var planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == Id && p.ApplicationUserId == user.Id);
            if (planet == null)
                return RedirectToPage("/ManageColonies", new { area = "Game" });

            // Deduct 5 turns
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, 5);
            if (!turnResult.Success)
            {
                ModelState.AddModelError(string.Empty, turnResult.Message);
                Planet = planet; // So the page can re-render with planet info
                return Page();
            }
            var totalPlanetInfra = planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining;
            var mod = GetFactionModifiers(user.Faction);
            creditsGainedForPlunder = ((int)Math.Floor(((planet.CurrentPopulation * 2500) + ((5500 * (totalPlanetInfra ^ 2)) / planet.TotalLand) + (200000 * planet.TotalPlanets) / 15) * mod.FactionPlunderModifier)).ToString("C");
            var creditsGained = (int)Math.Floor(((planet.CurrentPopulation * 2500) + ((5500 * (totalPlanetInfra ^ 2)) / planet.TotalLand) + (200000 * planet.TotalPlanets) / 15) * mod.FactionPlunderModifier);
            UserCommodities.Credits += creditsGained;
            for (int i = 0; i < planet.TotalPlanets; i++)
            {
                userExploration.ExplorationPointsNeeded = (int)Math.Floor(userExploration.ExplorationPointsNeeded / 1.2);
            }
            user.TotalColonies -= 1;
            user.TotalPlanets -= planet.TotalPlanets;
            user.PlanetsPlundered += planet.TotalPlanets;
            // Delete the planet
            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();

            PlunderMessage = "Planet successfully plundered. ";
            // Redirect to ManageColonies with a message
            TempData["PlunderMessage"] = PlunderMessage + "Credits For Plunder:" + creditsGainedForPlunder;
            TempData["TurnMessage"] = turnResult.Message;
            return RedirectToPage("/ManageColonies", new { area = "Game" });
        }

        // Helper for faction modifiers
        private (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier,
                 decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost, decimal FactionPlunderModifier)
            GetFactionModifiers(Faction faction)
        {
            return faction switch
            {
                Faction.Terran => (1.0m, 1.1m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m,0.5m),
                Faction.AMiner => (2.2m, 0.05m, 3.5m, 0.5m, 29m, 1.0m, 0.8m,0.05m),
                Faction.Marauder => (0.5m, 0.05m, 0.5m, 0.5m, 0.5m, 1.0m, 0.05m,50m),
                Faction.Viral => (1.2m, 0.95m, 0.5m, 0.8m, 1.0m, 2.0m, 1.0m,0.1m),
                Faction.Collective => (0.5m, 0.05m, 0.1m, 1.5m, 0.5m, 0.05m, 1.0m,11m),
                Faction.Guardian => (0.15m, 0.01m, 0.01m, 0.05m, 0.01m, 0.1m, 0.75m,0.05m),
                Faction.KalZul => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m,0.05m),
                Faction.DarkMarauder => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m,0.05m),
                _ => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m,1.0m)
            };
        }
    }
}
