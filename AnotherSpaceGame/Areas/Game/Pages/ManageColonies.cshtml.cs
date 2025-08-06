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
            if (planets.Any(x => x.Name.Contains("H.")))
            {
                TempData["TurnMessage"] = "You cannot plunder a homeworld. Please select another planet.";
                return Page();
            }
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
                    UserExploration.ExplorationPointsNeeded = SetExplorationPointsNeeded(user);                    
                    user.TotalColonies -= 1;
                    user.TotalPlanets -= planet.TotalPlanets;
                    user.PlanetsPlundered += planet.TotalPlanets;
                    _context.Planets.Remove(planet); // Remove the planet from the database
                }
            }

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

        public long SetExplorationPointsNeeded(ApplicationUser user)
        {
            long explorationPointsNeeded = user.TotalPlanets switch
            {
                1 => 5000,
                2 => 6000,
                3 => 7200,
                4 => 8640,
                5 => 10368,
                6 => 12442,
                7 => 14930,
                8 => 17916,
                9 => 21499,
                10 => 25799,
                11 => 30959,
                12 => 37150,
                13 => 44581,
                14 => 53497,
                15 => 64196,
                16 => 77035,
                17 => 92442,
                18 => 110931,
                19 => 133117,
                20 => 159740,
                21 => 191688,
                22 => 230026,
                23 => 276031,
                24 => 331237,
                25 => 397484,
                26 => 476981,
                27 => 572377,
                28 => 686853,
                29 => 824223,
                30 => 989068,
                31 => 1186882,
                32 => 1424258,
                33 => 1709109,
                34 => 2050931,
                35 => 2461118,
                36 => 2953341,
                37 => 3544009,
                38 => 4252811,
                39 => 5103373,
                40 => 6124048,
                41 => 7348858,
                42 => 8818629,
                43 => 10582355,
                44 => 12698826,
                45 => 15238592,
                46 => 18286310,
                47 => 21943572,
                48 => 26332286,
                49 => 31598744,
                50 => 37918492,
                51 => 45502191,
                52 => 54602629,
                53 => 65523155,
                54 => 78627786,
                55 => 94353343,
                56 => 113224011,
                57 => 135868814,
                58 => 163042576,
                59 => 195651092,
                60 => 234781310,
                61 => 281737572,
                62 => 338085086,
                63 => 405702103,
                64 => 486842524,
                65 => 584211029,
                66 => 701053235,
                67 => 841263881,
                68 => 1009516658,
                69 => 1211419989,
                70 => 1453703987,
                71 => 1744444785,
                72 => 2093333742,
                73 => 2512000490,
                74 => 3014400588,
                75 => 3617280705,
                76 => 4340736847,
                77 => 5208884216,
                78 => 6250661059,
                79 => 7500793271,
                80 => 9000951925,
                81 => 10801142310,
                82 => 12961370772,
                83 => 15553644926,
                84 => 18664373912,
                85 => 22397248694,
                86 => 26876698433,
                87 => 32252038120,
                88 => 38702445743,
                89 => 46442934892,
                90 => 55731521871,
                91 => 66877826245,
                92 => 80253391494,
                93 => 96304069792,
                94 => 115564883751,
                95 => 138677860501,
                96 => 166413432601,
                97 => 199696119121,
                98 => 239635342946,
                99 => 287562411535,
                _ => 287562411535 // Default if not in range
            };

            return explorationPointsNeeded;
        }
    }
}
