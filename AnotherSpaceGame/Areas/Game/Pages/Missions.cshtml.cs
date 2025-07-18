using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class MissionsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MissionsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser CurrentUser { get; set; }

        [BindProperty]  
        public string MissionMessage { get; set; }
        [BindProperty]
        public string MissionTitle { get; set; }
        [BindProperty]
        public string MissionObjective { get; set; }
        [BindProperty]
        public string MissionReward { get; set; }
        [BindProperty]
        public string MissionStatus { get; set; }
        [BindProperty]
        public string MissionNumber { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // Eagerly load all related data (adjust Include/ThenInclude as needed)
            CurrentUser = await _context.Users
                .Include(u => u.UserProjects)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.Planets)
                .Include(u => u.Fleets)
                .Include(u => u.Turns)
                .Include(u => u.Commodities)
                .Include(u => u.Infrastructer)
                .Include(u => u.EClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ClusterResearch)
                .Include(u => u.Exploration)
                .Include(u => u.Artifacts)
                .Include(u => u.ImportantEvents)
                .Include(u => u.Battlelogs)
                .Include(u => u.Federation)
                .Include(u => u.FederationApplication)
                .Include(u => u.MarketPosts)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.TerranResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ClusterResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.Missions)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            // Null checks and initialization for one-to-one navigation properties
            if (CurrentUser.Missions == null) CurrentUser.Missions = new Missions();
            if (CurrentUser.EClassResearch == null) CurrentUser.EClassResearch = new EClassResearch();
            if (CurrentUser.CyrilClassResearch == null) CurrentUser.CyrilClassResearch = new CyrilClassResearch();
            if (CurrentUser.StrafezResearch == null) CurrentUser.StrafezResearch = new StrafezResearch();
            if (CurrentUser.FClassResearch == null) CurrentUser.FClassResearch = new FClassResearch();
            if (CurrentUser.ProjectsResearch == null) CurrentUser.ProjectsResearch = new ProjectsResearch();
            if (CurrentUser.ViralSpecificResearch == null) CurrentUser.ViralSpecificResearch = new ViralSpecificResearch();
            if (CurrentUser.CollectiveSpecificResearch == null) CurrentUser.CollectiveSpecificResearch = new CollectiveSpecificResearch();
            if (CurrentUser.TerranResearch == null) CurrentUser.TerranResearch = new TerranResearch();
            if (CurrentUser.AMinerResearch == null) CurrentUser.AMinerResearch = new AMinerResearch();
            if (CurrentUser.MarauderResearch == null) CurrentUser.MarauderResearch = new MarauderResearch();
            if (CurrentUser.ViralResearch == null) CurrentUser.ViralResearch = new ViralResearch();
            if (CurrentUser.CollectiveResearch == null) CurrentUser.CollectiveResearch = new CollectiveResearch();
            if (CurrentUser.GuardianResearch == null) CurrentUser.GuardianResearch = new GuardianResearch();
            if (CurrentUser.ClusterResearch == null) CurrentUser.ClusterResearch = new ClusterResearch();
            if (CurrentUser.Exploration == null) CurrentUser.Exploration = new Exploration();
            if (CurrentUser.Infrastructer == null) CurrentUser.Infrastructer = new Infrastructer();
            if (CurrentUser.Commodities == null) CurrentUser.Commodities = new Commodities();
            if (CurrentUser.UserProjects == null) CurrentUser.UserProjects = new UserProjects();
            if (CurrentUser.Missions == null)
            {
                CurrentUser.Missions = new Missions();
            }
            if (CurrentUser.Missions.Mission1 == false)
            {
                MissionTitle = "Mission 1: Welcome to the Game!";
                MissionObjective = "Complete one level of infrastructure.";
                MissionMessage = "Welcome to Another Space Game! Your first mission is to complete at least one level of infrastructure. This will help you get started on your journey through the galaxy.";
                MissionStatus = "Incomplete";
                MissionNumber = "1";
                MissionReward = "+$1,000,000 Credits";
                if(CurrentUser.Infrastructer.TotalLevels >= 1)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission2 == false)
            {
                MissionTitle = "Mission 2: Fleet Management";
                MissionObjective = "Create a fleet with at least one ship.";
                MissionMessage = "Your second mission is to create a fleet with at least one ship. This will allow you to explore the galaxy and engage in battles. We recommend a scout class ship.";
                MissionStatus = "Incomplete";
                MissionNumber = "2";
                MissionReward = "+1,000,000 Food";
                if (CurrentUser.Fleets.Count > 0 && CurrentUser.Fleets.Any(f => f.TotalShips > 0))
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission3 == false)
            {
                MissionTitle = "Mission 3: Planet Colonization";
                MissionObjective = "Colonize a new planet.";
                MissionMessage = "Your third mission is to colonize a new planet. This will expand your territory and resources.";
                MissionStatus = "Incomplete";
                MissionNumber = "3";
                MissionReward = "+$3,000,000 Credits";
                if (CurrentUser.Planets.Count > 1)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission4 == false)
            {
                MissionTitle = "Mission 4: Research and Development";
                MissionObjective = "Complete any Technology research.";
                MissionMessage = "Your fourth mission is to complete a technology research. This will enhance your technology and capabilities.";
                MissionStatus = "Incomplete";
                MissionNumber = "4";
                MissionReward = "+3,000,000 Food";
                if (CurrentUser.EClassResearch.ExaHertzClassRailgun == true || CurrentUser.CyrilClassResearch.CyrilAlloy == true || CurrentUser.StrafezResearch.BasicGeneticEngineering == true || CurrentUser.FClassResearch.FClassEngineering == true || CurrentUser.ProjectsResearch.CapsuleLaboratory == true || CurrentUser.ViralSpecificResearch.ViralClustering == true || CurrentUser.CollectiveSpecificResearch.CollectiveClustering == true || CurrentUser.TerranResearch.SmallTerranHull == true || CurrentUser.AMinerResearch.StarshipEngineering == true || CurrentUser.MarauderResearch.Class3MarauderHull == true || CurrentUser.GuardianResearch.CorvetteClass == true || CurrentUser.CollectiveResearch.FighterClass == true || CurrentUser.ViralResearch.Class1ViralHull == true)
                {
                    MissionStatus = "Complete";
                }

            }
            else if (CurrentUser.Missions.Mission5 == false)
            {
                MissionTitle = "Mission 5: Galactic Trade";
                MissionObjective = "Trade with other players.";
                MissionMessage = "Your fifth mission is to trade with other players via the market.Post something for other players to buy. This will help you gain valuable resources.";
                MissionStatus = "Incomplete";
                MissionNumber = "5";
                MissionReward = "+$5,000,000 Credits";
                if (CurrentUser.MarketPosts.Count > 0)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission6 == false)
            {
                MissionTitle = "Mission 6: Battle Tactics";
                MissionObjective = "Fight a battle against an enemy fleet.";
                MissionMessage = "Your sixth mission is to have a battle against an enemy fleet. This will test your combat skills.";
                MissionStatus = "Incomplete";
                MissionNumber = "6";
                MissionReward = "+$6,000,000 Credits";
                if (CurrentUser.Battlelogs.Count > 1)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission7 == false)
            {
                MissionTitle = "Mission 7: Artifact Discovery";
                MissionObjective = "Discover an artifact on a planet.";
                MissionMessage = "Your seventh mission is to discover an artifact on a planet. Artifacts can provide unique benefits.";
                MissionStatus = "Incomplete";
                MissionNumber = "7";
                MissionReward = "+$7,000,000 Credits";
                if (CurrentUser.Artifacts.Count > 0)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission8 == false)
            {
                MissionTitle = "Mission 8: Federation Diplomacy";
                MissionObjective = "Join or create a federation.";
                MissionMessage = "Your eighth mission is to join or create a federation. Federations provide additional benefits and alliances.";
                MissionStatus = "Incomplete";
                MissionNumber = "8";
                MissionReward = "+$8,000,000 Credits";
                if (CurrentUser.Federation != null || CurrentUser.FederationApplication != null)
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission9 == false)
            {
                MissionTitle = "Mission 9: Galactic Exploration";
                MissionObjective = "Create a new Colony Cluster";
                MissionMessage = "Your ninth mission is to explore more planets and create a new colony cluster. This will help you discover new resources and opportunities.";
                MissionStatus = "Incomplete";
                MissionNumber = "9";
                MissionReward = "+$9,000,000 Credits";
                if (CurrentUser.Planets.Any(p => p.TotalPlanets >= 4))
                {
                    MissionStatus = "Complete";
                }
            }
            else if (CurrentUser.Missions.Mission10 == false)
            {
                MissionTitle = "Mission 10: Ultimate Challenge";
                MissionObjective = "Have atleast one C2/C.2 and have atleast 20 infrastructure. ";
                MissionMessage = "Your final mission is to explore more planets to create a large colony cluster and improve your infrastructure levels. This will prove your mastery of Another Space Game.";
                MissionStatus = "Incomplete";
                MissionNumber = "10";
                MissionReward = "+$100,000,000 Credits,+100,000,000 Food and all your Colonies gain 150 Loyalty (upto 3000 maximum for guardians)";
                if (CurrentUser.Planets.Any(p => p.TotalPlanets >= 16) && CurrentUser.Infrastructer.TotalLevels >= 20)
                {
                    MissionStatus = "Complete";
                }
            }
            
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostCompleteMissionAsync()
        {
            // Use MissionNumber to identify and mark the mission as completed for the user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");
            CurrentUser = await _context.Users
                .Include(u => u.UserProjects)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.Planets)
                .Include(u => u.Fleets)
                .Include(u => u.Turns)
                .Include(u => u.Commodities)
                .Include(u => u.Infrastructer)
                .Include(u => u.EClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ClusterResearch)
                .Include(u => u.Exploration)
                .Include(u => u.Artifacts)
                .Include(u => u.ImportantEvents)
                .Include(u => u.Battlelogs)
                .Include(u => u.Federation)
                .Include(u => u.FederationApplication)
                .Include(u => u.MarketPosts)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.TerranResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ClusterResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.Missions)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            // Null checks and initialization for one-to-one navigation properties
            if (CurrentUser.Missions == null) CurrentUser.Missions = new Missions();
            if (CurrentUser.EClassResearch == null) CurrentUser.EClassResearch = new EClassResearch();
            if (CurrentUser.CyrilClassResearch == null) CurrentUser.CyrilClassResearch = new CyrilClassResearch();
            if (CurrentUser.StrafezResearch == null) CurrentUser.StrafezResearch = new StrafezResearch();
            if (CurrentUser.FClassResearch == null) CurrentUser.FClassResearch = new FClassResearch();
            if (CurrentUser.ProjectsResearch == null) CurrentUser.ProjectsResearch = new ProjectsResearch();
            if (CurrentUser.ViralSpecificResearch == null) CurrentUser.ViralSpecificResearch = new ViralSpecificResearch();
            if (CurrentUser.CollectiveSpecificResearch == null) CurrentUser.CollectiveSpecificResearch = new CollectiveSpecificResearch();
            if (CurrentUser.TerranResearch == null) CurrentUser.TerranResearch = new TerranResearch();
            if (CurrentUser.AMinerResearch == null) CurrentUser.AMinerResearch = new AMinerResearch();
            if (CurrentUser.MarauderResearch == null) CurrentUser.MarauderResearch = new MarauderResearch();
            if (CurrentUser.ViralResearch == null) CurrentUser.ViralResearch = new ViralResearch();
            if (CurrentUser.CollectiveResearch == null) CurrentUser.CollectiveResearch = new CollectiveResearch();
            if (CurrentUser.GuardianResearch == null) CurrentUser.GuardianResearch = new GuardianResearch();
            if (CurrentUser.ClusterResearch == null) CurrentUser.ClusterResearch = new ClusterResearch();
            if (CurrentUser.Exploration == null) CurrentUser.Exploration = new Exploration();
            if (CurrentUser.Infrastructer == null) CurrentUser.Infrastructer = new Infrastructer();
            if (CurrentUser.Commodities == null) CurrentUser.Commodities = new Commodities();
            if (CurrentUser.UserProjects == null) CurrentUser.UserProjects = new UserProjects();

            if (CurrentUser.Missions == null)
            {
                CurrentUser.Missions = new Missions();
            }
            // Example: Mark the mission as complete (adjust logic as needed)
            int missionNum = int.TryParse(MissionNumber, out var num) ? num : 0;
            if (missionNum > 0)
            {
                // Mark the mission as complete in the user's Missions object
                switch (missionNum)
                {
                    case 1: user.Missions.Mission1 = true;
                        CurrentUser.Commodities.Credits += 1000000; // Reward for Mission 1
                        break;
                    case 2: user.Missions.Mission2 = true; 
                        CurrentUser.Commodities.Food += 1000000; // Reward for Mission 2
                        break;
                    case 3: user.Missions.Mission3 = true; 
                        CurrentUser.Commodities.Credits += 3000000; // Reward for Mission 3
                        break;
                    case 4: user.Missions.Mission4 = true; 
                        CurrentUser.Commodities.Food += 3000000; // Reward for Mission 4
                        break;
                    case 5: user.Missions.Mission5 = true; 
                        CurrentUser.Commodities.Credits += 5000000; // Reward for Mission 5
                        break;
                    case 6: user.Missions.Mission6 = true; 
                        CurrentUser.Commodities.Credits += 6000000; // Reward for Mission 6
                        break;
                    case 7: user.Missions.Mission7 = true; 
                        CurrentUser.Commodities.Credits += 7000000; // Reward for Mission 7
                        break;
                    case 8: user.Missions.Mission8 = true; 
                        CurrentUser.Commodities.Credits += 8000000; // Reward for Mission 8
                        break;
                    case 9: user.Missions.Mission9 = true; 
                        CurrentUser.Commodities.Credits += 9000000; // Reward for Mission 9
                        break;
                    case 10: user.Missions.Mission10 = true; 
                        CurrentUser.Commodities.Credits += 100000000; // Reward for Mission 10
                        CurrentUser.Commodities.Food += 100000000; // Reward for Mission 10
                        foreach (var planet in CurrentUser.Planets)
                        {
                            planet.Loyalty += 150; // Set loyalty to maximum for guardians
                            if (planet.Loyalty > 3000 && CurrentUser.Faction == Faction.Guardian)
                            {
                                planet.Loyalty = 3000; // Cap loyalty at 3000
                            }
                        }
                        break;
                }
                await _context.SaveChangesAsync();
            }
            TempData["MissionSuccess"] = "Mission completed successfully!";

            // Optionally, set a success message or redirect
            return RedirectToPage();
        }
    }
}
