using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ExploreModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public ExploreModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public Exploration UserExploration { get; set; }
        public UserProjects UserProjects { get; set; }
        public ProjectsResearch ProjectsResearch { get; set; }
        public string ExploreMessage { get; set; }
        public bool CanExplore { get; set; }
        public Planets? NewPlanet { get; set; }
        [BindProperty]
        public PlanetType SelectedPlanetType { get; set; } = PlanetType.Barren;
        [BindProperty]
        public bool EnableFeature { get; set; }
        public IEnumerable<SelectListItem> PlanetTypeOptions =>
        Enum.GetValues(typeof(PlanetType))
        .Cast<PlanetType>().Where(pt => 
        pt != PlanetType.AssimilatedC1 && 
        pt != PlanetType.AssimilatedC2 && 
        pt != PlanetType.AssimilatedC3 && 
        pt != PlanetType.ClusterLevel1 && 
        pt != PlanetType.ClusterLevel2 && 
        pt != PlanetType.ClusterLevel3 && 
        pt != PlanetType.InfectedC1 && 
        pt != PlanetType.InfectedC2 && 
        pt != PlanetType.InfectedC3 && 
        pt != PlanetType.TaintedC1 && 
        pt != PlanetType.TaintedC2 && 
        pt != PlanetType.TaintedC3 && 
        pt != PlanetType.TaintedC4 && 
        pt != PlanetType.SimilareC1 && 
        pt != PlanetType.SimilareC2 && 
        pt != PlanetType.SimilareC3 && 
        pt != PlanetType.SimilareC4 && 
        pt != PlanetType.SimilareC5)
        .Select(pt => new SelectListItem
        {
            Value = pt.ToString(),
            Text = pt.ToString(),
            Selected = pt == PlanetType.Barren
        });
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // Block exploration for Collective or Marauder factions
            if (user.Faction == Faction.Collective || user.Faction == Faction.Marauder)
            {
                ExploreMessage = "Your faction cannot explore for new planets.";
                CanExplore = false;
                return Page();
            }

            UserExploration = await _context.Explorations
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            EnableFeature = UserExploration.EnableFeature; // <-- Add this property to your Exploration model
            UserProjects = await _context.UserProjects
                .FirstOrDefaultAsync(up => up.ApplicationUserId == user.Id);
            ProjectsResearch = await _context.ProjectsResearches
                .FirstOrDefaultAsync(up => up.ApplicationUserId == user.Id);

            if (UserExploration == null)
            {
                UserExploration = new Exploration { ApplicationUserId = user.Id };
                _context.Explorations.Add(UserExploration);
                await _context.SaveChangesAsync();
            }


            await UpdateExplorationStats(user, UserExploration);
            CanExplore = await UserHasFleet(user) && !IsAtColonyCap(user);
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(int turnsUsed)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            Turns turns = await _context.Turns.FirstOrDefaultAsync(t => t.ApplicationUserId == user.Id);
            // Block exploration for Collective or Marauder factions
            if (user.Faction == Faction.Collective || user.Faction == Faction.Marauder)
            {
                ExploreMessage = "Your faction cannot explore for new planets.";
                CanExplore = false;
                return Page();
            }

            UserExploration = await _context.Explorations
                .FirstOrDefaultAsync(e => e.ApplicationUserId == user.Id);
            UserExploration.EnableFeature = EnableFeature;
            UserProjects = await _context.UserProjects
                .FirstOrDefaultAsync(up => up.ApplicationUserId == user.Id);
            ProjectsResearch = await _context.ProjectsResearches
                .FirstOrDefaultAsync(up => up.ApplicationUserId == user.Id);

            if (UserExploration == null)
                return RedirectToPage();

            await UpdateExplorationStats(user, UserExploration);

            if (!await UserHasFleet(user))
            {
                ExploreMessage = "You need at least one ship in your fleet to explore.";
                return Page();
            }

            if (IsAtColonyCap(user))
            {
                ExploreMessage = "You are at your maximum colony cap and cannot explore more planets.";
                return Page();
            }
            if (turnsUsed > turns.CurrentTurns)
            {
                turnsUsed = turns.CurrentTurns;
            }
            if (turnsUsed > UserExploration.TurnsRequired)
            {
                turnsUsed = UserExploration.TurnsRequired;
            }
            if (turnsUsed <= 0)
            {
                ExploreMessage = "You must use at least 1 turn.";
                return Page();
            }

            // Deduct turns
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, turnsUsed);
            if (!turnResult.Success)
            {
                ModelState.AddModelError(string.Empty, turnResult.Message);
                return Page();
            }

            // Calculate progress
            decimal percentPerTurn = 100m / UserExploration.TurnsRequired;
            decimal progress = percentPerTurn * turnsUsed;
            UserExploration.ExplorationCompleted += progress;
            var allowedTypes = new[]
            {
                PlanetType.Barren,
                PlanetType.Icy,
                PlanetType.Marshy,
                PlanetType.Forest,
                PlanetType.Oceanic,
                PlanetType.Rocky,
                PlanetType.Desert,
                PlanetType.Balanced,
                PlanetType.Gas,
                PlanetType.URich,
                PlanetType.UEden,
                PlanetType.USpazial,
                PlanetType.ULarge,
                PlanetType.UFertile,
                PlanetType.Dead
            };
            var random = new System.Random();
            // Define your weighted planet types and their chances (sum should be 100)
            var weightedTypes = new (PlanetType Type, int Weight)[]
            {
                (PlanetType.Barren, 30),
                (PlanetType.Icy, 30),
                (PlanetType.Marshy, 30),
                (PlanetType.Forest, 30),
                (PlanetType.Oceanic, 30),
                (PlanetType.Rocky, 30),
                (PlanetType.Desert, 30),
                (PlanetType.Balanced, 30),
                (PlanetType.Gas, 30),
                (PlanetType.URich, 1),
                (PlanetType.UEden, 1),
                (PlanetType.USpazial, 1),
                (PlanetType.ULarge, 1),
                (PlanetType.UFertile, 1),
                (PlanetType.Dead, 1)
            };

            int totalWeight = weightedTypes.Sum(wt => wt.Weight);
            int roll = random.Next(totalWeight);
            int cumulative = 0;
            PlanetType chosenType = PlanetType.Barren; // default fallback

            foreach (var (type, weight) in weightedTypes)
            {
                cumulative += weight;
                if (roll < cumulative)
                {
                    chosenType = type;
                    break;
                }
            }
            if (UserExploration.ExplorationCompleted >= 100m)
            {
                // Found a new planet
                NewPlanet = new Planets
                {
                    ApplicationUserId = user.Id,
                    Name = "E." + random.Next(1000, 9999).ToString(),
                    Type = chosenType,
                    FoodRequired = 1,
                    GoodsRequired = 1,
                    CurrentPopulation = 10,
                    MaxPopulation = 10,
                    Loyalty = 2500,
                    AvailableLabour = 8,
                    Housing = 1,
                    Commercial = 0,
                    Industry = 0,
                    Agriculture = 0,
                    Mining = 1,
                    MineralProduced = (MineralType)random.Next(0, 6),
                    PowerRating = 0
                };
                // Set type-specific values
                switch (chosenType)
                {
                    case PlanetType.Barren:
                        NewPlanet.PopulationModifier = 0.5m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0.02m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.AvailableOre = random.Next(150, 500);
                        NewPlanet.TotalLand = random.Next(50, 1100);
                        break;
                    case PlanetType.Icy:
                        NewPlanet.PopulationModifier = 0.75m;
                        NewPlanet.AgricultureModifier = 1m;
                        NewPlanet.OreModifier = 0.005m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.AvailableOre = random.Next(1500, 4500);
                        NewPlanet.TotalLand = random.Next(24, 83);
                        break;
                    case PlanetType.Marshy:
                        NewPlanet.PopulationModifier = 0.8m;
                        NewPlanet.AgricultureModifier = 0.5m;
                        NewPlanet.OreModifier = 0.005m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(50, 150);
                        NewPlanet.AvailableOre = random.Next(0, 150);
                        break;
                    case PlanetType.Forest:
                        NewPlanet.PopulationModifier = 0.9m;
                        NewPlanet.AgricultureModifier = 2m;
                        NewPlanet.OreModifier = 0.005m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(50, 350);
                        NewPlanet.AvailableOre = random.Next(0, 50);
                        break;
                    case PlanetType.Oceanic:
                        NewPlanet.PopulationModifier = 0.8m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0.005m;
                        NewPlanet.ArtifactModifier = 0.10m;
                        NewPlanet.TotalLand = random.Next(10, 50);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.Rocky:
                        NewPlanet.PopulationModifier = 0.75m;
                        NewPlanet.AgricultureModifier = 1m;
                        NewPlanet.OreModifier = 0.001m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(34, 121);
                        NewPlanet.AvailableOre = random.Next(500, 3300);
                        break;
                    case PlanetType.Desert:
                        NewPlanet.PopulationModifier = 0.75m;
                        NewPlanet.AgricultureModifier = 0.75m;
                        NewPlanet.OreModifier = 0.02m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(100, 250);
                        NewPlanet.AvailableOre = random.Next(100, 350);
                        break;
                    case PlanetType.Balanced:
                        NewPlanet.PopulationModifier = 1.2m;
                        NewPlanet.AgricultureModifier = 1m;
                        NewPlanet.OreModifier = 0.01m;
                        NewPlanet.ArtifactModifier = 0.05m;
                        NewPlanet.TotalLand = random.Next(185, 1050);
                        NewPlanet.AvailableOre = random.Next(750, 1100);
                        break;
                    case PlanetType.Gas:
                        NewPlanet.PopulationModifier = 1m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.05m;
                        NewPlanet.TotalLand = random.Next(2, 6);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.URich:
                        NewPlanet.PopulationModifier = 0.1m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0.03m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(11, 28);
                        NewPlanet.AvailableOre = random.Next(50000, 350000);
                        break;
                    case PlanetType.UEden:
                        NewPlanet.PopulationModifier = 10m;
                        NewPlanet.AgricultureModifier = 0.02m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(500, 2500);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.USpazial:
                        NewPlanet.PopulationModifier = 0.1m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.15m;
                        NewPlanet.TotalLand = random.Next(2, 4);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.ULarge:
                        NewPlanet.PopulationModifier = 0.2m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(7000, 16000);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.UFertile:
                        NewPlanet.PopulationModifier = 0.5m;
                        NewPlanet.AgricultureModifier = 1.75m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(950, 2150);
                        NewPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.Dead:
                        NewPlanet.PopulationModifier = 0.05m;
                        NewPlanet.AgricultureModifier = 0m;
                        NewPlanet.OreModifier = 0m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(2, 4);
                        NewPlanet.AvailableOre = 0;
                        break;
                    default:
                        NewPlanet.PopulationModifier = 1m;
                        NewPlanet.AgricultureModifier = 1m;
                        NewPlanet.OreModifier = 1m;
                        NewPlanet.ArtifactModifier = 0.01m;
                        NewPlanet.TotalLand = random.Next(2, 4);
                        NewPlanet.AvailableOre = 0;
                        break;
                }
                // S Class
                var Srandom = new System.Random();
                // 0.1% chance (1 in 1000)
                if (Srandom.Next(1000) == 0)
                {
                    // Your special code here
                    NewPlanet.Name = "S." + random.Next(1000, 9999).ToString();
                    NewPlanet.TotalLand = NewPlanet.TotalLand * random.Next(2, 11); // Increase land by a factor of 2 to 4
                }
                // Calculate LandAvailable
                NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                if (NewPlanet.Type != SelectedPlanetType && UserExploration.EnableFeature == true && ProjectsResearch.AdvancedExploration == true)
                {
                    UserExploration.ExplorationCompleted = 0;
                    user.ColoniesExplored += 1;
                    UserExploration.ExplorationPointsNeeded = SetExplorationPointsNeeded(user);
                    ModelState.AddModelError(string.Empty, "Planet plundered due to not being the chosen type.");
                    ExploreMessage = turnResult.Message;
                    NewPlanet = null;
                }
                else
                {
                    UserExploration.ExplorationCompleted = 0;
                    user.TotalPlanets += 1;
                    user.TotalColonies += 1;
                    user.ColoniesExplored += 1;
                    UserExploration.ExplorationPointsNeeded = SetExplorationPointsNeeded(user);
                    _context.Planets.Add(NewPlanet);
                    ExploreMessage = turnResult.Message;
                    ExploreMessage = $"Congratulations! You have discovered a new planet. <br> {turnResult.Message}";
                }
            }
            else
            {
                ExploreMessage = $"Exploration progress: {UserExploration.ExplorationCompleted:0.##}% complete for the next planet. <br> {turnResult.Message}";
            }
            ServerStats serverStats = _context.ServerStats.FirstOrDefault();
            if (serverStats.UWEnabled == true && serverStats.UWHolderId == user.Id)
            {
                user.DamageProtection = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            await UpdateExplorationStats(user, UserExploration);
            CanExplore = await UserHasFleet(user) && !IsAtColonyCap(user);
            return Page();
        }

        private async Task UpdateExplorationStats(ApplicationUser user, Exploration exploration)
        {
            // Get all fleets for the user
            var fleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == user.Id)
                .ToListAsync();

            var shipIds = fleets.Select(f => f.ShipId).ToList();
            var ships = await _context.Ships
                .Where(s => shipIds.Contains(s.Id))
                .ToListAsync();

            int totalShips = fleets.Sum(f => f.TotalShips);
            int totalScanningPower = fleets.Sum(f =>
            {
                var ship = ships.FirstOrDefault(s => s.Id == f.ShipId);
                return ship != null ? f.TotalShips * ship.ScanningPower : 0;
            });

            exploration.ShipsInFleet = totalShips;
            exploration.ScanningPower = totalScanningPower;
            exploration.TotalPlanets = user.TotalPlanets;
            exploration.TotalColonies = user.TotalColonies;

            // Calculate turns required
            if (totalScanningPower + totalShips > 0)
            {
                // Convert 'exploration.ExplorationPointsNeeded' to double to match the type of the numerator
                exploration.TurnsRequired = (int)Math.Ceiling((double)(exploration.ExplorationPointsNeeded * (1 - (UserExploration.ExplorationCompleted / 100))) / (totalScanningPower + totalShips));
            }
            else
            {
                exploration.TurnsRequired = 0;
            }
            if(exploration.TurnsRequired <= 0)
            {
                exploration.TurnsRequired = 1;
            }
        }

        private async Task<bool> UserHasFleet(ApplicationUser user)
        {
            return await _context.Fleets.AnyAsync(f => f.ApplicationUserId == user.Id && f.TotalShips > 0);
        }

        private bool IsAtColonyCap(ApplicationUser user)
        {
            // Example: cap at 16, adjust as needed or use faction-based logic
            var maxColoniesByFaction = new Dictionary<Faction, int>
            {
                { Faction.Terran, 16 },
                { Faction.AMiner, 13 },
                { Faction.Collective, 14 },
                { Faction.Marauder, 13 },
                { Faction.Guardian, 11 },
                { Faction.Viral, 13 }
            };
            int cap = maxColoniesByFaction.TryGetValue(user.Faction, out int val) ? val : 10;
            return user.TotalColonies >= cap;
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
