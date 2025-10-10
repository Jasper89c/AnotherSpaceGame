using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class UltimateWeaponCompletedModel : PageModel
    {
        [BindProperty]
        public string OptionGroup1 { get; set; }
        [BindProperty]
        public string OptionGroup2 { get; set; }
        [BindProperty]
        public string OptionGroup3 { get; set; }
        [BindProperty]
        public string OptionGroup4 { get; set; }
        [BindProperty]
        public Faction? SelectedFaction { get; set; }
        public List<Faction> AvailableFactions { get; set; } = new();
        private  ServerStats serverStats;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UltimateWeaponCompletedModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            serverStats = _context.ServerStats.FirstOrDefault();


            if (serverStats.UWHolderId != user.Id)
            {
               return RedirectToPage("/Game/ImportantEvents");
            }
            AvailableFactions = Enum.GetValues<Faction>()
                .Where(f => f != Faction.KalZul && f != Faction.DarkMarauder)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            OnGet();
            var user = await _userManager.Users
                .Include(u => u.Commodities)
                .Include(u => u.Planets)
                .Include(u => u.ImportantEvents)
                .Include(u => u.Fleets)
                .Include(u => u.Artifacts)
                .Include(u => u.UserProjects)
                .Include(u => u.ProjectsResearch)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            serverStats = _context.ServerStats.FirstOrDefault();
            var uwShips = _context.UWShips.FirstOrDefault(u => u.ApplicationUserId == user.Id);
            // uw winners list
            var uwWinners = new UWWinners
            {
                Name = user.UserName,
                PowerRating = user.PowerRating.ToString("N0"),
                DaysTaken = user.EmpireAge.ToString("N0"),
                Date = DateTime.Now,
                TotalPlanets = user.TotalPlanets.ToString("N0")
            };
            _context.UWWinners.Add(uwWinners);
            if (serverStats.UWHolderId != user.Id)
            {
                return RedirectToPage("/Game/ImportantEvents");
            }

            // Validate the selected faction
            if (SelectedFaction == null || !AvailableFactions.Contains(SelectedFaction.Value))
            {
                ModelState.AddModelError(string.Empty, "Please select a valid faction.");
                return Page();
            }
            // Use a transaction for atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //Set the UW Ships
                if (uwShips == null)
                {
                    uwShips = new UWShips { ApplicationUserId = user.Id };
                    _context.UWShips.Add(uwShips);
                }
                if (uwShips.UWship1 != true)
                {
                    uwShips.UWship1 = true;
                }
                else if (uwShips.UWship2 != true && uwShips.UWship1 == true)
                {
                    uwShips.UWship2 = true;
                }
                else if (uwShips.UWship3 != true && uwShips.UWship1 == true && uwShips.UWship2 == true)
                {
                    uwShips.UWship3 = true;
                }
                else if (uwShips.UWship4 != true && uwShips.UWship1 == true && uwShips.UWship2 == true && uwShips.UWship3 == true)
                {
                    uwShips.UWship4 = true;
                }


                // set the user faction
                user.Faction = SelectedFaction.Value;
                // set the user Planets
                await ResetPlanetsAsync(user, SelectedFaction.Value);
                // set the user Research
                await ResetResearchAsync(user, SelectedFaction.Value);
                // set the user Commodities
                ResetCommodities(user.Commodities);
                // reset other user properties
                ResetOtherPropertiesAsync(user);
                // option 2
                var artifact1 = new Artifacts();
                if (OptionGroup2 == "21")
                {
                    artifact1 = new Artifacts(52,30,user.Id);
                }
                else if (OptionGroup2 == "22")
                {                    
                    artifact1 = new Artifacts(30,10,user.Id);
                }
                else if (OptionGroup2 == "23")
                {
                    artifact1 = new Artifacts(63,10,user.Id);
                }
                // option 3
                var artifact2 = new Artifacts();
                if (OptionGroup3 == "31")
                {
                    if (artifact1.ArtifactId == 52)
                    {
                        artifact1 = new Artifacts(52, 60, user.Id);
                    }
                    else
                    {
                        artifact2 = new Artifacts(52, 30, user.Id);
                    }
                }
                else if (OptionGroup3 == "32")
                {
                    if (artifact1.ArtifactId == 30)
                    {
                        artifact1 = new Artifacts(30, 20, user.Id);
                    }
                    else
                    {
                        artifact2 = new Artifacts(30, 10, user.Id);
                    }                    
                }
                else if (OptionGroup3 == "33")
                {
                    if (artifact1.ArtifactId == 63)
                    {
                        artifact1 = new Artifacts(63, 20, user.Id);
                    }
                    else
                    {
                        artifact2 = new Artifacts(63, 10, user.Id);
                    }                    
                }
                // option 4
                var artifact3 = new Artifacts();
                if (OptionGroup4 == "31")
                {
                    if (artifact1.ArtifactId == 52)
                    {
                        artifact1 = new Artifacts(52, 60, user.Id);
                    }
                    else if (artifact2.ArtifactId == 52)
                    {
                        artifact2 = new Artifacts(52, 60, user.Id);
                    }
                    else
                    {
                        artifact3 = new Artifacts(52, 30, user.Id);
                    }
                }
                else if (OptionGroup4 == "32")
                {
                    if (artifact1.ArtifactId == 30)
                    {
                        artifact1 = new Artifacts(30, 20, user.Id);
                    }
                    else if (artifact2.ArtifactId == 30)
                    {
                        artifact2 = new Artifacts(30, 20, user.Id);
                    }
                    else
                    {
                        artifact3 = new Artifacts(30, 10, user.Id);
                    }
                }
                else if (OptionGroup4 == "33")
                {
                    if (artifact1.ArtifactId == 63)
                    {
                        artifact1 = new Artifacts(63, 20, user.Id);
                    }
                    else if (artifact2.ArtifactId == 63)
                    {
                        artifact2 = new Artifacts(63, 20, user.Id);
                    }
                    else
                    {
                        artifact3 = new Artifacts(63, 10, user.Id);
                    }
                }
                if(artifact1.Total > 0)
                {
                    user.Artifacts.Add(artifact1);
                }
                if(artifact2.Total > 0)
                {
                    user.Artifacts.Add(artifact2);
                }
                if(artifact3.Total > 0)
                {
                    user.Artifacts.Add(artifact3);
                }
                // +15 Small Time Capsules
                var artifact4 = new Artifacts(37, 15, user.Id);
                // +25 Minor Gordos
                var artifact5 = new Artifacts(50, 25, user.Id);
                // +5 Planetary Core Capsules 
                var artifact6 = new Artifacts(64, 5, user.Id);
                user.Artifacts.Add(artifact4);
                user.Artifacts.Add(artifact5);
                user.Artifacts.Add(artifact6);

                // fleet reset
                foreach (var fleet in user.Fleets.ToList())
                {
                    _context.Fleets.Remove(fleet);
                }
                // important event
                user.ImportantEvents.Add(new ImportantEvents
                {
                    ApplicationUserId = user.Id,
                    DateAndTime = DateTime.Now,
                    ImportantEventTypes = ImportantEventTypes.Misc,
                    Text = $"Congratulations on completing the Ultimate Weapon,Your empire has been restarted. You are now a {user.Faction}."
                });
                
                serverStats.UWHolderId = null;
                serverStats.UWHolderName = null;
                serverStats.UWCompleted = false;
                serverStats.UWEnabled = false;
                
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                // Log the error (not shown here)
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                return Page();
            }
            return RedirectToPage("/ImportantEvents", "Game");
        }

        private async Task ResetPlanetsAsync(ApplicationUser user, Faction faction)
        {
            // Remove old planets
            var userPlanets = _context.Planets.Where(p => p.ApplicationUserId == user.Id);
            _context.Planets.RemoveRange(userPlanets);

            // Add new starting planet
            var planet = new Planets
            {
                ApplicationUserId = user.Id,
                Name = "UW." + Random.Shared.RandomString(4),
                Type = faction switch
                {
                    Faction.Viral => PlanetType.TaintedC1,
                    Faction.Collective => PlanetType.SimilareC1,
                    _ => PlanetType.Balanced
                }
            };
            var planet1 = new Planets
            {
                ApplicationUserId = user.Id                
            };
            if(OptionGroup1 == "11")
            {
                planet1.Name = "UW." + Random.Shared.RandomString(4);
                planet1.Type = PlanetType.ULarge;
                planet1.TotalLand = 25000;
                planet1.LandAvailable = 25000 - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining);
                if (faction == Faction.Guardian)
                {
                    planet1.Loyalty = 3000;
                }
            }
            else if(OptionGroup1 == "12")
            {
                planet1.Name = "UW." + Random.Shared.RandomString(4);
                planet1.Type = PlanetType.UEden;
                planet1.TotalLand = 25000;
                planet1.LandAvailable = 25000 - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining);
                if (faction == Faction.Guardian)
                {
                    planet1.Loyalty = 3000;
                }
            }
            else if(OptionGroup1 == "13")
            {
                planet1.Name = "UW." + Random.Shared.RandomString(4);
                planet1.Type = PlanetType.UFertile;
                planet1.TotalLand = 10000;
                planet1.LandAvailable = 10000 - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining);
                if (faction == Faction.Guardian)
                {
                    planet1.Loyalty = 3000;
                }
            }
            else if(OptionGroup1 == "14")
            {
                planet1.Name = "UW." + Random.Shared.RandomString(4);
                planet1.Type = PlanetType.Barren;
                planet1.TotalLand = 20000;
                planet1.LandAvailable = 20000 - (planet.Housing + planet.Commercial + planet.Industry + planet.Agriculture + planet.Mining);
                if (faction == Faction.Guardian)
                {
                    planet1.Loyalty = 3000;
                }
            }

            user.Planets = new List<Planets> { planet, planet1 };
            _context.Planets.Add(planet);
            _context.Planets.Add(planet1);

            // Reset colony/planet counts
            user.TotalColonies = 1;
            user.TotalPlanets = 1;
        }
        private async Task ResetResearchAsync(ApplicationUser user, Faction faction)
        {
            // Remove all existing research objects for this user
            var userId = user.Id;

            // Remove all research entities for this user
            _context.EClassResearches.RemoveRange(_context.EClassResearches.Where(r => r.ApplicationUserId == userId));
            _context.CyrilClassResearches.RemoveRange(_context.CyrilClassResearches.Where(r => r.ApplicationUserId == userId));
            _context.StrafezResearches.RemoveRange(_context.StrafezResearches.Where(r => r.ApplicationUserId == userId));
            _context.FClassResearches.RemoveRange(_context.FClassResearches.Where(r => r.ApplicationUserId == userId));
            _context.ProjectsResearches.RemoveRange(_context.ProjectsResearches.Where(r => r.ApplicationUserId == userId));
            _context.UserProjects.RemoveRange(_context.UserProjects.Where(r => r.ApplicationUserId == userId));
            _context.ViralSpecificResearches.RemoveRange(_context.ViralSpecificResearches.Where(r => r.ApplicationUserId == userId));
            _context.CollectiveSpecificResearches.RemoveRange(_context.CollectiveSpecificResearches.Where(r => r.ApplicationUserId == userId));
            _context.TerranResearches.RemoveRange(_context.TerranResearches.Where(r => r.ApplicationUserId == userId));
            _context.AMinerResearches.RemoveRange(_context.AMinerResearches.Where(r => r.ApplicationUserId == userId));
            _context.MarauderResearches.RemoveRange(_context.MarauderResearches.Where(r => r.ApplicationUserId == userId));
            _context.ViralResearches.RemoveRange(_context.ViralResearches.Where(r => r.ApplicationUserId == userId));
            _context.CollectiveResearches.RemoveRange(_context.CollectiveResearches.Where(r => r.ApplicationUserId == userId));
            _context.GuardianResearches.RemoveRange(_context.GuardianResearches.Where(r => r.ApplicationUserId == userId));
            _context.ClusterResearches.RemoveRange(_context.ClusterResearches.Where(r => r.ApplicationUserId == userId));
            _context.Missions.RemoveRange(_context.Missions.Where(r => r.ApplicationUserId == userId));
            _context.MarketPosts.RemoveRange(_context.MarketPosts.Where(mp => mp.ApplicationUserId == userId));
            // Save removals before adding new ones
            await _context.SaveChangesAsync();

            // Always add these (one-to-one navigation properties)
            var eClassResearch = new EClassResearch { ApplicationUserId = userId };
            var cyrilClassResearch = new CyrilClassResearch { ApplicationUserId = userId };
            var strafezResearch = new StrafezResearch { ApplicationUserId = userId };
            var fClassResearch = new FClassResearch { ApplicationUserId = userId };
            var projectsResearch = new ProjectsResearch { ApplicationUserId = userId };
            var userProjects = new UserProjects { ApplicationUserId = userId };
            var Missions = new Missions { ApplicationUserId = userId };

            user.EClassResearch = eClassResearch;
            user.CyrilClassResearch = cyrilClassResearch;
            user.StrafezResearch = strafezResearch;
            user.FClassResearch = fClassResearch;
            user.ProjectsResearch = projectsResearch;
            user.Missions = Missions;

            _context.EClassResearches.Add(eClassResearch);
            _context.CyrilClassResearches.Add(cyrilClassResearch);
            _context.StrafezResearches.Add(strafezResearch);
            _context.FClassResearches.Add(fClassResearch);
            _context.ProjectsResearches.Add(projectsResearch);
            _context.UserProjects.Add(userProjects);
            _context.Missions.Add(Missions);

            // Faction-specific research
            switch (faction)
            {
                case Faction.Viral:
                    var viralSpecificResearch = new ViralSpecificResearch { ApplicationUserId = userId };
                    var viralResearch = new ViralResearch { ApplicationUserId = userId };
                    var viralReversedShips = new ViralReversedShips { ApplicationUserId = userId };
                    user.ViralSpecificResearch = viralSpecificResearch;
                    user.ViralResearch = viralResearch;
                    user.ViralReversedShips = viralReversedShips;
                    _context.ViralReversedShips.Add(viralReversedShips);
                    _context.ViralSpecificResearches.Add(viralSpecificResearch);
                    _context.ViralResearches.Add(viralResearch);
                    break;
                case Faction.Collective:
                    var collectiveSpecificResearch = new CollectiveSpecificResearch { ApplicationUserId = userId };
                    var collectiveResearch = new CollectiveResearch { ApplicationUserId = userId };
                    user.CollectiveSpecificResearch = collectiveSpecificResearch;
                    user.CollectiveResearch = collectiveResearch;
                    _context.CollectiveSpecificResearches.Add(collectiveSpecificResearch);
                    _context.CollectiveResearches.Add(collectiveResearch);
                    break;
                case Faction.Terran:
                    var terranResearch = new TerranResearch { ApplicationUserId = userId };
                    var clusterResearchT = new ClusterResearch { ApplicationUserId = userId };
                    user.TerranResearch = terranResearch;
                    user.ClusterResearch = clusterResearchT;
                    _context.TerranResearches.Add(terranResearch);
                    _context.ClusterResearches.Add(clusterResearchT);
                    break;
                case Faction.AMiner:
                    var aMinerResearch = new AMinerResearch { ApplicationUserId = userId };
                    var clusterResearchA = new ClusterResearch { ApplicationUserId = userId };
                    user.AMinerResearch = aMinerResearch;
                    user.ClusterResearch = clusterResearchA;
                    _context.AMinerResearches.Add(aMinerResearch);
                    _context.ClusterResearches.Add(clusterResearchA);
                    break;
                case Faction.Marauder:
                    var marauderResearch = new MarauderResearch { ApplicationUserId = userId };
                    var clusterResearchM = new ClusterResearch { ApplicationUserId = userId };
                    user.MarauderResearch = marauderResearch;
                    user.ClusterResearch = clusterResearchM;
                    _context.MarauderResearches.Add(marauderResearch);
                    _context.ClusterResearches.Add(clusterResearchM);
                    break;
                case Faction.Guardian:
                    var guardianResearch = new GuardianResearch { ApplicationUserId = userId };
                    var clusterResearchG = new ClusterResearch { ApplicationUserId = userId };
                    user.GuardianResearch = guardianResearch;
                    user.ClusterResearch = clusterResearchG;
                    _context.GuardianResearches.Add(guardianResearch);
                    _context.ClusterResearches.Add(clusterResearchG);
                    break;
                    // Add more cases as needed for other factions
            }
        }
        private void ResetCommodities(Commodities commodities)
        {
            commodities.Food = 250000;
            commodities.Ore = 2000;
            commodities.RawMaterial = 10000;
            commodities.ConsumerGoods = 10000;
            commodities.TerranMetal = 10000;
            commodities.RedCrystal = 10000;
            commodities.WhiteCrystal = 10000;
            commodities.Rutile = 10000;
            commodities.Composite = 10000;
            commodities.StrafezOrganism = 10000;
            commodities.Credits = 250000;
        }
        private async Task ResetOtherPropertiesAsync(ApplicationUser user)
        {
            // Turns
            // Remove existing turns if they exist
            var existingTurns = _context.Turns.FirstOrDefault(t => t.ApplicationUserId == user.Id);
            if (existingTurns != null)
            {
                _context.Turns.Remove(existingTurns);
            }
            // Create a new turns object
            var turns = new Turns { ApplicationUserId = user.Id };
            user.Turns = turns;
            _context.Turns.Add(turns);

            // PowerRating and EmpireAge
            user.PowerRating = 1000;
            user.EmpireAge = (DateTime.Now - user.PlayingSince).TotalDays;

            // Damage Protection
            user.DamageProtection = DateTime.Now.AddDays(1);

            // Last Activity
            user.LastAction = DateTime.Now;

            // Artifact Shield
            user.ArtifactShield = 0m;
            // Temple height
            user.TempleHeight = 0;

            // Exploration
            // Remove existing exploration for this user if it exists
            var existingExploration = _context.Explorations.FirstOrDefault(e => e.ApplicationUserId == user.Id);
            if (existingExploration != null)
            {
                _context.Explorations.Remove(existingExploration);
            }
            // Create a new exploration object
            var exploration = new Exploration { ApplicationUserId = user.Id };
            user.Exploration = exploration;
            _context.Explorations.Add(exploration);

            // Infrastructer
            // Remove existing infrastructer if it exists
            var existingInfrastructer = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            if (existingInfrastructer != null)
            {
                _context.Infrastructers.Remove(existingInfrastructer);
            }
            // Create a new infrastructer object
            var infrastructer = new Infrastructer { ApplicationUserId = user.Id };
            infrastructer.UnusedLevels = 20;
            user.Infrastructer = infrastructer;
            _context.Infrastructers.Add(infrastructer);

            // Artifacts
            _context.Artifacts.RemoveRange(_context.Artifacts.Where(a => a.ApplicationUserId == user.Id));

        }
    }
}
