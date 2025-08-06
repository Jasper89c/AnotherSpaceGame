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
            if (planet.Name.Contains("H."))
            {
                PlunderMessage = "You cannot plunder a homeworld. Please select another planet.";
                return Page();
            }
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
                userExploration.ExplorationPointsNeeded = SetExplorationPointsNeeded(user);
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
