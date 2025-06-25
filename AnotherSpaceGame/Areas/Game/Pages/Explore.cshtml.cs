using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public string ExploreMessage { get; set; }
        public bool CanExplore { get; set; }
        public Planets? NewPlanet { get; set; }

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
                ExploreMessage = "You do not have enough turns available.";
                return Page();
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
            var chosenType = allowedTypes[random.Next(allowedTypes.Length)];
            if (UserExploration.ExplorationCompleted >= 100m)
            {
                // Found a new planet
                UserExploration.ExplorationCompleted = 0;
                UserExploration.ExplorationPointsNeeded = (int)(UserExploration.ExplorationPointsNeeded * 1.2);
                user.TotalPlanets += 1;
                user.TotalColonies += 1;
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
                    MineralProduced = 0,
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
                // Calculate LandAvailable
                NewPlanet.LandAvailable = NewPlanet.TotalLand - (NewPlanet.Housing + NewPlanet.Commercial + NewPlanet.Industry + NewPlanet.Agriculture + NewPlanet.Mining);
                _context.Planets.Add(NewPlanet);
                ExploreMessage = $"Congratulations! You have discovered a new planet. <br> {turnResult.Message}";
            }
            else
            {
                // Deduct turns
                var turnResultb = await _turnService.TryUseTurnsAsync(user.Id, turnsUsed);
                if (!turnResult.Success)
                {
                    ModelState.AddModelError(string.Empty, turnResult.Message);
                    return Page();
                }
                ExploreMessage = $"Exploration progress: {UserExploration.ExplorationCompleted:0.##}% complete for the next planet. <br> {turnResultb.Message}";
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
                exploration.TurnsRequired = (int)System.Math.Ceiling((double)(exploration.ExplorationPointsNeeded * (1 - (UserExploration.ExplorationCompleted / 100))) / (totalScanningPower + totalShips));
            }
            else
            {
                exploration.TurnsRequired = 0;
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
    }
}
