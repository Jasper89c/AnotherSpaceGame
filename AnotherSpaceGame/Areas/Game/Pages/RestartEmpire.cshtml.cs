using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class RestartEmpireModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RestartEmpireModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public SelectList FactionOptions { get; set; }

        [BindProperty]
        [Required]
        public string SelectedFaction { get; set; }

        [BindProperty]
        [Required]
        public string ConfirmationInput { get; set; }

        public string ConfirmationString { get; set; }

        [TempData]
        public string TempConfirmationString { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
            ConfirmationString = GenerateRandomString(8);
            TempConfirmationString = ConfirmationString;
            FactionOptions = GetFactionSelectList();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            ConfirmationString = TempConfirmationString;
            FactionOptions = GetFactionSelectList();

            if (!ModelState.IsValid)
            {
                StatusMessage = "Please select a faction and enter the confirmation string.";
                return Page();
            }

            if (ConfirmationInput != ConfirmationString)
            {
                StatusMessage = "Confirmation string does not match. Please try again.";
                ConfirmationString = GenerateRandomString(8);
                TempConfirmationString = ConfirmationString;
                return Page();
            }

            var user = await _userManager.Users
                .Include(u => u.Commodities)
                .Include(u => u.Planets)
                .Include(u => u.ImportantEvents)
                .Include(u => u.Fleets)
                .FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (!Enum.TryParse<Faction>(SelectedFaction, out var parsedFaction) ||
                parsedFaction == Faction.KalZul || parsedFaction == Faction.DarkMarauder)
            {
                StatusMessage = "Invalid faction selected.";
                return Page();
            }

            // Use a transaction for atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                user.Faction = parsedFaction;
                ResetCommodities(user.Commodities);
                await ResetPlanetsAsync(user, parsedFaction);
                await ResetResearchAsync(user, parsedFaction);
                await ResetOtherPropertiesAsync(user);

                foreach (var fleet in user.Fleets.ToList())
                {
                    _context.Fleets.Remove(fleet);
                }

                user.ImportantEvents.Add(new ImportantEvents
                {
                    ApplicationUserId = user.Id,
                    ImportantEventTypes = ImportantEventTypes.Misc,
                    Text = $"Your empire has been restarted. You are now a {user.Faction}."
                });

                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                StatusMessage = $"Faction changed to {SelectedFaction}.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessage = $"An error occurred: {ex.Message}";
            }

            ConfirmationString = GenerateRandomString(8);
            TempConfirmationString = ConfirmationString;
            return Page();
        }

        private SelectList GetFactionSelectList()
        {
            var factions = Enum.GetValues(typeof(Faction))
                .Cast<Faction>()
                .Where(f => f != Faction.KalZul && f != Faction.DarkMarauder)
                .Select(f => new { Value = f.ToString(), Text = f.ToString() })
                .ToList();

            return new SelectList(factions, "Value", "Text");
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var data = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            var result = new StringBuilder(length);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
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

        private async Task ResetPlanetsAsync(ApplicationUser user, Faction faction)
        {
            // Remove old planets
            var userPlanets = _context.Planets.Where(p => p.ApplicationUserId == user.Id);
            _context.Planets.RemoveRange(userPlanets);

            // Add new starting planet
            var planet = new Planets
            {
                ApplicationUserId = user.Id,
                Name = "H." + Random.Shared.RandomString(4),
                Type = faction switch
                {
                    Faction.Viral => PlanetType.TaintedC1,
                    Faction.Collective => PlanetType.SimilareC1,
                    _ => PlanetType.Balanced
                }
            };
            user.Planets = new List<Planets> { planet };
            _context.Planets.Add(planet);

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
            _context.Missions.Add(Missions);

            // Faction-specific research
            switch (faction)
            {
                case Faction.Viral:
                    var viralSpecificResearch = new ViralSpecificResearch { ApplicationUserId = userId };
                    var viralResearch = new ViralResearch { ApplicationUserId = userId };
                    user.ViralSpecificResearch = viralSpecificResearch;
                    user.ViralResearch = viralResearch;
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
            user.EmpireAge = (DateTime.UtcNow - user.PlayingSince).TotalDays;

            // Damage Protection
            user.DamageProtection = DateTime.Now.AddDays(1);

            // Last Activity
            user.LastAction = DateTime.UtcNow;

            // Artifact Shield
            user.ArtifactShield = 0m;

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
            user.Infrastructer = infrastructer;
            _context.Infrastructers.Add(infrastructer);

            // Artifacts
            _context.Artifacts.RemoveRange(_context.Artifacts.Where(a => a.ApplicationUserId == user.Id));


        }
    }
}