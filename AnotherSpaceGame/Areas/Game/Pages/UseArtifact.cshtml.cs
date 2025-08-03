using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class UseArtifactModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UseArtifactModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string TargetUsername { get; set; }

        public Artifacts Artifact { get; set; }
        public string UseMessage { get; set; }

        // Artifact shield reduction values
        private static readonly Dictionary<ArtifactName, double> ArtifactShieldValues = new()
        {
            { ArtifactName.EnergyPod, 0.25 }, { ArtifactName.WhiteOrb, 0.25 }, { ArtifactName.BlackOrb, 0.25 }, { ArtifactName.BlueOrb, 0.25 },
            { ArtifactName.GreenOrb, 0.25 }, { ArtifactName.OrangeOrb, 0.25 }, { ArtifactName.YellowOrb, 0.25 }, { ArtifactName.PurpleOrb, 0.25 },
            { ArtifactName.GrayOrb, 0.25 }, { ArtifactName.BrownOrb, 0.25 }, { ArtifactName.MoccasinOrb, 0.25 }, { ArtifactName.GoldenOrb, 0.25 },
            { ArtifactName.TurquoiseOrb, 0.25 }, { ArtifactName.AquaOrb, 0.25 }, { ArtifactName.PinkOrb, 0.25 }, { ArtifactName.PlumOrb, 0.25 },
            { ArtifactName.OrganicBase, 1.25 }, { ArtifactName.AssimilatedBase, 1.25 },
            { ArtifactName.CuartoMapa, 0.5 }, { ArtifactName.BronzeDinero, 0.5 }, { ArtifactName.SilverDinero, 0.5 }, { ArtifactName.GoldDinero, 0.5 },
            { ArtifactName.PlatinumDinero, 0.5 }, { ArtifactName.AmberDinero, 0.5 }, { ArtifactName.GarnetDinero, 0.5 }, { ArtifactName.TopazDinero, 0.5 },
            { ArtifactName.OpalDinero, 0.5 }, { ArtifactName.AmethystDinero, 0.5 }, { ArtifactName.SmallTimeCapsule, 1 },
            { ArtifactName.MinorSuerte, 0.75 }, { ArtifactName.MajorSuerte, 0.75 }, { ArtifactName.MinorRequerido, 0.75 }, { ArtifactName.MinorGente, 0.75 },
            { ArtifactName.MinorAlimento, 0.75 }, { ArtifactName.MinorCosecha, 0.75 }, { ArtifactName.MinorTierra, 0.75 }, { ArtifactName.Traicione, 0.75 },
            { ArtifactName.BigTimeCapsule, 1 }, { ArtifactName.MinorGordo, 1 }, { ArtifactName.MinorBarrera, 0.75 }, { ArtifactName.Historia, 1 },
            { ArtifactName.MinorAfortunado, 1 }, { ArtifactName.MajorAfortunado, 1 }, { ArtifactName.MinorEstructura, 1 }, { ArtifactName.MajorAlimento, 1 },
            { ArtifactName.MajorCosecha, 1 }, { ArtifactName.MajorTierra, 1 }, { ArtifactName.Persiana, 1 }, { ArtifactName.MajorGordo, 1.25 },
            { ArtifactName.MajorBarrera, 1 }, { ArtifactName.Regalo, 1 }, { ArtifactName.MajorProducto, 1 }, { ArtifactName.MajorDinero, 1 },
            { ArtifactName.GrandEstructura, 1.25 }, { ArtifactName.GrandAlimenter, 1.25 }, { ArtifactName.GrandCosecha, 1.25 }, { ArtifactName.GrandGente, 1.25 },
            { ArtifactName.GrandTierra, 1.25 }, { ArtifactName.GrandRequerido, 1.25 }, { ArtifactName.GrandBarrera, 1.25 }, { ArtifactName.GrandProducto, 1.25 },
            { ArtifactName.GrandAlimento, 1.25 }, { ArtifactName.GrandDinero, 1.25 }, { ArtifactName.PlanetaryCore, 1.25 }
        };

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Artifact = await _context.Artifacts
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == id && a.ApplicationUserId == user.Id);

            if (Artifact == null)
                return RedirectToPage("/Artifacts", new { area = "Game" });

            Id = id;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Artifact = await _context.Artifacts
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == Id && a.ApplicationUserId == user.Id);

            if (Artifact == null || Artifact.Total < 1)
            {
                UseMessage = "You do not own this artifact or have none left.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(TargetUsername))
            {
                ModelState.AddModelError(string.Empty, "Please enter a username to use the artifact on.");
                return Page();
            }

            var targetUser = await _userManager.Users
                .Include(u => u.Commodities)
                .Include(u => u.Turns)
                .Include(u => u.Planets)
                .FirstOrDefaultAsync(u => u.UserName == TargetUsername);

            if (targetUser == null)
            {
                ModelState.AddModelError(string.Empty, "Target user not found.");
                return Page();
            }

            // Check for artifact shield and reduce if present
            var shieldValue = ArtifactShieldValues.TryGetValue(Artifact.ArtifactName, out var value) ? value : 0;
            var artifactShieldProp = targetUser.GetType().GetProperty("ArtifactShield"); // expects a double or float property
            double currentShield = 0;
            if (artifactShieldProp != null)
            {
                var shieldObj = artifactShieldProp.GetValue(targetUser) ?? 0m;
                currentShield = Convert.ToDouble((decimal)shieldObj);
                if (currentShield > 0 && shieldValue > 0)
                {
                    // Reduce shield and do nothing else
                    double newShield = currentShield - shieldValue;
                    artifactShieldProp.SetValue(targetUser, newShield < 0 ? 0 : newShield);
                    Artifact.Total -= 1;
                    await _context.SaveChangesAsync();
                    UseMessage = $"Target user has an artifact shield! Shield reduced by {shieldValue}. No other effect.";
                    return Page();
                }
            }

            string effectMsg = Artifact.ArtifactName switch
            {
                // No effect artifacts
                ArtifactName.EnergyPod or ArtifactName.WhiteOrb or ArtifactName.BlackOrb or ArtifactName.BlueOrb or
                ArtifactName.GreenOrb or ArtifactName.OrangeOrb or ArtifactName.YellowOrb or ArtifactName.PurpleOrb or
                ArtifactName.GrayOrb or ArtifactName.BrownOrb or ArtifactName.MoccasinOrb or ArtifactName.GoldenOrb or
                ArtifactName.TurquoiseOrb or ArtifactName.AquaOrb or ArtifactName.PinkOrb or ArtifactName.PlumOrb or
                ArtifactName.OrganicBase or ArtifactName.AssimilatedBase or ArtifactName.MinorRequerido or
                ArtifactName.MinorGente or ArtifactName.MinorAlimento or ArtifactName.MinorCosecha or
                ArtifactName.Traicione or ArtifactName.MinorAfortunado or ArtifactName.MajorAfortunado or
                ArtifactName.MinorEstructura or ArtifactName.MajorAlimento or ArtifactName.MajorCosecha or
                ArtifactName.GrandEstructura or ArtifactName.GrandAlimenter or ArtifactName.GrandCosecha or
                ArtifactName.GrandGente or ArtifactName.GrandProducto or ArtifactName.GrandRequerido or
                ArtifactName.GrandAlimento or ArtifactName.CuartoMapa =>
                    "This artifact has no effect.",

                // Artifact Formula
                //ArtifactName.CuartoMapa =>
                //    $"Gave 1 Artifact Formula to {TargetUsername}.", // TODO: Implement artifact formula logic

                // Credit Increase/Decrease
                ArtifactName.BronzeDinero => AddCredits(targetUser, 50000, TargetUsername),
                ArtifactName.SilverDinero => AddCredits(targetUser, 150000, TargetUsername),
                ArtifactName.GoldDinero => AddCredits(targetUser, 250000, TargetUsername),
                ArtifactName.PlatinumDinero => AddCredits(targetUser, 500000, TargetUsername),
                ArtifactName.AmberDinero => AddCredits(targetUser, -50000, TargetUsername, true),
                ArtifactName.GarnetDinero => AddCredits(targetUser, 150000, TargetUsername),
                ArtifactName.TopazDinero => AddCredits(targetUser, 250000, TargetUsername),
                ArtifactName.OpalDinero => AddCredits(targetUser, 500000, TargetUsername),
                ArtifactName.AmethystDinero => AddCredits(targetUser, 750000, TargetUsername),

                // Turns
                ArtifactName.SmallTimeCapsule => AddTurns(targetUser, 10, TargetUsername),
                ArtifactName.BigTimeCapsule => AddTurns(targetUser, 100, TargetUsername),

                // Luck
                ArtifactName.MinorSuerte => $"Gave +Luck to {TargetUsername}.", // TODO: Implement luck logic
                ArtifactName.MajorSuerte => $"Gave ++Luck to {TargetUsername}.", // TODO: Implement luck logic

                // Ore
                ArtifactName.MinorTierra => ChangeOre(targetUser, -0.01m, TargetUsername),
                ArtifactName.MajorTierra => ChangeOre(targetUser, -0.03m, TargetUsername),
                ArtifactName.GrandTierra => ChangeOre(targetUser, -0.06m, TargetUsername),

                // Land
                ArtifactName.MinorGordo => AddLand(targetUser, 20, 30, TargetUsername),
                ArtifactName.MajorGordo => AddLand(targetUser, 60, 100, TargetUsername),

                // Artifact Shield
                ArtifactName.MinorBarrera => AddArtifactShield(targetUser, 10, TargetUsername),
                ArtifactName.MajorBarrera => AddArtifactShield(targetUser, 20, TargetUsername),
                ArtifactName.GrandBarrera => AddArtifactShield(targetUser, 45, TargetUsername),

                // Turns
                ArtifactName.Historia => RemoveTurns(targetUser, 40, TargetUsername),

                // Delay Attacks
                ArtifactName.Persiana => $"Delayed attacks on {TargetUsername}.", // TODO: Implement delay logic

                // Regalo: 3-5 random artifacts
                ArtifactName.Regalo => GiveRandomArtifacts(targetUser, 3, 5, _context, TargetUsername),

                // Major Producto: Decrease consumer goods
                ArtifactName.MajorProducto => DecreaseConsumerGoods(targetUser, TargetUsername),

                // Major Dinero: -3% credits
                ArtifactName.MajorDinero => ChangeCreditsPercent(targetUser, -0.03m, TargetUsername),

                // Grand Dinero: -10% credits
                ArtifactName.GrandDinero => ChangeCreditsPercent(targetUser, -0.10m, TargetUsername),

                // PCC: gives 1 random planet
                ArtifactName.PlanetaryCore => GiveRandomPlanet(targetUser, user, _context, TargetUsername),

                _ => "This artifact has no effect."
            };

            // Decrement artifact
            Artifact.Total -= 1;
            await _context.SaveChangesAsync();

            UseMessage = $"Used {Artifact.ArtifactName} on {TargetUsername}. {effectMsg}";
            return Page();
        }

        // --- Helper methods for artifact effects ---
        private static string AddArtifactShield(ApplicationUser user, int amount, string target)
        {
            user.ArtifactShield += amount;
            return $"Provided artifact shield to {target} (+{amount} shield).";
        }
        private static string AddCredits(ApplicationUser user, int amount, string target, bool isDecrease = false)
        {
            if (user.Commodities != null)
            {
                user.Commodities.Credits += amount;
                return isDecrease
                    ? $"Took {(-amount):N0} credits from {target}."
                    : $"Gave {amount:N0} credits to {target}.";
            }
            return "Target has no commodities.";
        }

        private static string AddTurns(ApplicationUser user, int turns, string target)
        { 
            if (user.Turns != null)
            {
                user.Turns.CurrentTurns += turns;
                if (user.Turns.CurrentTurns > user.Turns.MaxTurns)
                {
                    user.Turns.CurrentTurns = user.Turns.MaxTurns; // Cap at max turns
                }
                return $"Gave {turns} turns to {target}.";
            }
            return "Target has no turns.";
        }

        private static string RemoveTurns(ApplicationUser user, int turns, string target)
        {
            if (user.Turns != null)
            {
                user.Turns.CurrentTurns -= turns;
                return $"Removed {turns} turns from {target}.";
            }
            return "Target has no turns.";
        }

        private static string ChangeOre(ApplicationUser user, decimal percent, string target)
        {
            // Example: change ore on all planets
            if (user.Planets != null && user.Planets.Any())
            {
                foreach (var planet in user.Planets)
                {
                    int before = planet.AvailableOre;
                    int change = (int)(planet.AvailableOre * percent);
                    planet.AvailableOre += change;
                }
                return $"Changed ore by {percent:P0} for all {target}'s planets.";
            }
            return "Target has no planets.";
        }

        private static string AddLand(ApplicationUser user, int min, int max, string target)
        {
            if (user.Planets != null && user.Planets.Any())
            {
                var topPlanet = user.Planets.OrderByDescending(p => p.TotalLand).FirstOrDefault();
                if (topPlanet != null)
                {
                    int land = new System.Random().Next(min, max + 1);
                    topPlanet.TotalLand += land;
                    topPlanet.LandAvailable += land;
                    return $"Gave {land} land to {target}'s top planet.";
                }
            }
            return "Target has no planets.";
        }

        private static string GiveRandomArtifacts(ApplicationUser user, int min, int max, ApplicationDbContext context, string target)
        {
            // Pick random artifact IDs (excluding common orbs)

            var rand = new System.Random();
            var artifactId = rand.Next(1, 65);
            
                var artifact = context.Artifacts.FirstOrDefault(a => a.ApplicationUserId == user.Id && a.ArtifactId == artifactId);
                if (artifact == null)
                {
                    artifact = new Artifacts(artifactId, 1, user.Id);
                    context.Artifacts.Add(artifact);
                }
                else
                {
                    artifact.Total += 1;
                    if (artifact.Total > artifact.MaxTotal)
                        artifact.Total = artifact.MaxTotal;
                }
            
            return $"Gave 1 {artifact.ArtifactName} to {target}.";
        }

        private static string DecreaseConsumerGoods(ApplicationUser user, string target)
        {
            if (user.Commodities != null)
            {
                long before = user.Commodities.ConsumerGoods;
                int decrease = (int)(before * 0.05); // Example: decrease by 5%
                user.Commodities.ConsumerGoods = System.Math.Max(0, before - decrease);
                return $"Decreased {target}'s consumer goods by {decrease}.";
            }
            return "Target has no commodities.";
        }

        private static string ChangeCreditsPercent(ApplicationUser user, decimal percent, string target)
        {
            if (user.Commodities != null)
            {
                long before = user.Commodities.Credits;
                int change = (int)(before * percent);
                user.Commodities.Credits += change;
                return percent < 0
                    ? $"Decreased {target}'s credits by {System.Math.Abs(change):N0}."
                    : $"Increased {target}'s credits by {change:N0}.";
            }
            return "Target has no commodities.";
        }
        
        private static string GiveRandomPlanet(ApplicationUser target, ApplicationUser fromUser, ApplicationDbContext context, string targetName)
        {
            ServerStats serverStats = context.ServerStats.FirstOrDefault();
            if (serverStats.UWEnabled != true)
            {
                // Faction-based max colonies
                var maxColoniesByFaction = new Dictionary<Faction, int>
                {
                    { Faction.Terran, 16 },
                    { Faction.AMiner, 13 },
                    { Faction.Collective, 14 },
                    { Faction.Marauder, 13 },
                    { Faction.Guardian, 11 },
                    { Faction.Viral, 13 }
                };

                int maxColonies = maxColoniesByFaction.TryGetValue(target.Faction, out int val) ? val : 10;

                // Count current colonies
                int currentColonies = context.Planets.Count(p => p.ApplicationUserId == target.Id);

                if (currentColonies >= maxColonies)
                {
                    return $"{targetName} already has the maximum number of colonies for their faction ({maxColonies}).";
                }

                // Only allow planet types that are handled in the switch
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

                // Set up base values
                var newPlanet = new Planets
                {
                    ApplicationUserId = target.Id,
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
                        newPlanet.PopulationModifier = 0.5m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0.02m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.AvailableOre = random.Next(150, 500);
                        newPlanet.TotalLand = random.Next(50, 1100);
                        break;
                    case PlanetType.Icy:
                        newPlanet.PopulationModifier = 0.75m;
                        newPlanet.AgricultureModifier = 1m;
                        newPlanet.OreModifier = 0.005m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.AvailableOre = random.Next(1500, 4500);
                        newPlanet.TotalLand = random.Next(24, 83);
                        break;
                    case PlanetType.Marshy:
                        newPlanet.PopulationModifier = 0.8m;
                        newPlanet.AgricultureModifier = 0.5m;
                        newPlanet.OreModifier = 0.005m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(50, 150);
                        newPlanet.AvailableOre = random.Next(0, 150);
                        break;
                    case PlanetType.Forest:
                        newPlanet.PopulationModifier = 0.9m;
                        newPlanet.AgricultureModifier = 2m;
                        newPlanet.OreModifier = 0.005m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(50, 350);
                        newPlanet.AvailableOre = random.Next(0, 50);
                        break;
                    case PlanetType.Oceanic:
                        newPlanet.PopulationModifier = 0.8m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0.005m;
                        newPlanet.ArtifactModifier = 0.10m;
                        newPlanet.TotalLand = random.Next(10, 50);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.Rocky:
                        newPlanet.PopulationModifier = 0.75m;
                        newPlanet.AgricultureModifier = 1m;
                        newPlanet.OreModifier = 0.001m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(34, 121);
                        newPlanet.AvailableOre = random.Next(500, 3300);
                        break;
                    case PlanetType.Desert:
                        newPlanet.PopulationModifier = 0.75m;
                        newPlanet.AgricultureModifier = 0.75m;
                        newPlanet.OreModifier = 0.02m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(100, 250);
                        newPlanet.AvailableOre = random.Next(100, 350);
                        break;
                    case PlanetType.Balanced:
                        newPlanet.PopulationModifier = 1.2m;
                        newPlanet.AgricultureModifier = 1m;
                        newPlanet.OreModifier = 0.01m;
                        newPlanet.ArtifactModifier = 0.05m;
                        newPlanet.TotalLand = random.Next(185, 1050);
                        newPlanet.AvailableOre = random.Next(750, 1100);
                        break;
                    case PlanetType.Gas:
                        newPlanet.PopulationModifier = 1m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.05m;
                        newPlanet.TotalLand = random.Next(2, 6);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.URich:
                        newPlanet.PopulationModifier = 0.1m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0.03m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(11, 28);
                        newPlanet.AvailableOre = random.Next(50000, 350000);
                        break;
                    case PlanetType.UEden:
                        newPlanet.PopulationModifier = 10m;
                        newPlanet.AgricultureModifier = 0.02m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(500, 2500);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.USpazial:
                        newPlanet.PopulationModifier = 0.1m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.15m;
                        newPlanet.TotalLand = random.Next(2, 4);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.ULarge:
                        newPlanet.PopulationModifier = 0.2m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(7000, 16000);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.UFertile:
                        newPlanet.PopulationModifier = 0.5m;
                        newPlanet.AgricultureModifier = 1.75m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(950, 2150);
                        newPlanet.AvailableOre = 0;
                        break;
                    case PlanetType.Dead:
                        newPlanet.PopulationModifier = 0.05m;
                        newPlanet.AgricultureModifier = 0m;
                        newPlanet.OreModifier = 0m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(2, 4);
                        newPlanet.AvailableOre = 0;
                        break;
                    default:
                        newPlanet.PopulationModifier = 1m;
                        newPlanet.AgricultureModifier = 1m;
                        newPlanet.OreModifier = 1m;
                        newPlanet.ArtifactModifier = 0.01m;
                        newPlanet.TotalLand = random.Next(2, 4);
                        newPlanet.AvailableOre = 0;
                        break;
                }

                // Calculate LandAvailable
                newPlanet.LandAvailable = newPlanet.TotalLand - (newPlanet.Housing + newPlanet.Commercial + newPlanet.Industry + newPlanet.Agriculture + newPlanet.Mining);

                context.Planets.Add(newPlanet);
                return $"Created a new random planet for {targetName}.";
            }
            else
            {
                return "Planetary Core is disabled due to Ultimate Weapon.";
            }
        }
    }
}

