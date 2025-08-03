using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class IncomeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IncomeModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public decimal TaxCreditsPerTurn { get; set; }
        public decimal TaxCreditsPerTurnWithGoods { get; set; }
        public decimal CommercialCreditsPerTurn { get; set; }
        public decimal AgriculturePerTurn { get; set; }
        public decimal AgriculturePerTurnMinusFood { get; set; }
        public decimal IndustryPerTurn { get; set; }
        public decimal IndustryPerTurnMinusGoodsEaten { get; set; }
        public decimal MiningPerTurn { get; set; }
        public decimal InfrastructureCostPerTurn { get; set; }
        public decimal FleetCostPerTurn { get; set; }
        public decimal FoodNeeded { get; set; }
        public decimal GoodsNeeded { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var planets = await _context.Planets
                .Where(p => p.ApplicationUserId == user.Id)
                .ToListAsync();

            var infra = await _context.Infrastructers
                .FirstOrDefaultAsync(i => i.ApplicationUserId == user.Id);

            var fleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == user.Id)
                .ToListAsync();

            foreach (var planet in planets)
            {
                // Calculate income from each planet
                TaxCreditsPerTurn += Math.Floor(((planet.CurrentPopulation * (planet.Loyalty / 5000m)) + (planet.CurrentPopulation / 2m)) * GetFactionModifiers(user.Faction).FactionTaxModifier);
                CommercialCreditsPerTurn += Math.Floor((planet.Commercial * ((infra.Commercial * 0.5m) + 5)) * GetFactionModifiers(user.Faction).FactionCommercialModifier);
                AgriculturePerTurn += Math.Floor(((planet.Agriculture * ((infra.Agriculture * 0.1m) + 1)) * GetFactionModifiers(user.Faction).FactionAgricultureModifier) * planet.AgricultureModifier);
                IndustryPerTurn += Math.Floor((planet.Industry * ((infra.Industry * 0.1m) + 1)) * GetFactionModifiers(user.Faction).FactionIndustryModifier);
                MiningPerTurn += Math.Floor((planet.TotalPlanets * ((0.13m * infra.Mining) + 1)) * GetFactionModifiers(user.Faction).FactionMiningModifier);
                InfrastructureCostPerTurn += Math.Floor((planet.Housing + planet.Industry + planet.Agriculture + planet.Mining + planet.Commercial) * GetFactionModifiers(user.Faction).InfrastructreMaintenanceCost);
                FoodNeeded += planet.FoodRequired;
                GoodsNeeded += planet.GoodsRequired;
            }
            // Calulate goods income
            TaxCreditsPerTurnWithGoods = Math.Floor(TaxCreditsPerTurn + (GoodsNeeded * 5.5m));
            IndustryPerTurnMinusGoodsEaten -= GoodsNeeded;
            // Calculate food income
            AgriculturePerTurnMinusFood = AgriculturePerTurn - FoodNeeded;
            // Calculate fleet cost per turn
            FleetCostPerTurn = fleets.Sum(f => f.TotalUpkeep);

            return Page();
        }
        // Helper for faction modifiers
        private (decimal FactionTaxModifier, decimal FactionCommercialModifier, decimal FactionIndustryModifier,
                 decimal FactionAgricultureModifier, decimal FactionMiningModifier, decimal FactionDemandForGoods, decimal InfrastructreMaintenanceCost)
            GetFactionModifiers(Faction faction)
        {
            return faction switch
            {
                Faction.Terran => (1.0m, 1.1m, 2.2m, 1.2m, 1.0m, 3.5m, 1.0m),
                Faction.AMiner => (2.2m, 0.05m, 3.5m, 0.5m, 29m, 1.0m, 0.8m),
                Faction.Marauder => (0.5m, 0.05m, 0.5m, 0.5m, 0.5m, 1.0m, 0.05m),
                Faction.Viral => (1.2m, 0.95m, 0.5m, 0.8m, 1.0m, 2.0m, 1.0m),
                Faction.Collective => (0.5m, 0.05m, 0.1m, 1.5m, 0.5m, 0.05m, 1.0m),
                Faction.Guardian => (0.15m, 0.01m, 0.01m, 0.05m, 0.01m, 0.1m, 0.75m),
                Faction.KalZul => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
                Faction.DarkMarauder => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 3.5m, 1.0m),
                _ => (1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m, 1.0m)
            };
        }
    }
}
