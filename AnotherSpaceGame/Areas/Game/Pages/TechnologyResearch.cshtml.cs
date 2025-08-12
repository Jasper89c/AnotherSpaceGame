using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class TechnologyResearchModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;
        public TechnologyResearchModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }
        [BindProperty]
        public int TurnsToInvest { get; set; }
        public string StatusMessage { get; set; }
        public string ResearchMessage { get; set; }

        public ApplicationUser user { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            return Page();
        }

        //terran
        public async Task<IActionResult> OnPostResearchSmallTerranHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if(TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.SmallTerranHullTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.SmallTerranHullTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.SmallTerranHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Small Terran Hull.";
                return Page();
            }
            user.TerranResearch.SmallTerranHullTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.SmallTerranHullTurnsRequired <= 0)
            {
                user.TerranResearch.SmallTerranHullTurnsRequired = 0;
                user.TerranResearch.SmallTerranHull = true;
                ResearchMessage = "You have successfully researched the Small Terran Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Small Terran Hull. {user.TerranResearch.SmallTerranHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchMediumTerranHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.MediumTerranHullTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.MediumTerranHullTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.MediumTerranHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Medium Terran Hull.";
                return Page();
            }
            user.TerranResearch.MediumTerranHullTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.MediumTerranHullTurnsRequired <= 0)
            {
                user.TerranResearch.MediumTerranHullTurnsRequired = 0;
                user.TerranResearch.MediumTerranHull = true;
                ResearchMessage = "You have successfully researched the Medium Terran Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Medium Terran Hull. {user.TerranResearch.MediumTerranHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();


            return Page();
        }
        public async Task<IActionResult> OnPostResearchLargeTerranHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.LargeTerranHullTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.LargeTerranHullTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.LargeTerranHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Large Terran Hull.";
                return Page();
            }
            user.TerranResearch.LargeTerranHullTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.LargeTerranHullTurnsRequired <= 0)
            {
                user.TerranResearch.LargeTerranHullTurnsRequired = 0;
                user.TerranResearch.LargeTerranHull = true;
                ResearchMessage = "You have successfully researched the Large Terran Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Large Terran Hull. {user.TerranResearch.LargeTerranHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranCorvetteAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.TerranCorvetteTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.TerranCorvetteTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranCorvetteTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Corvette.";
                return Page();
            }
            user.TerranResearch.TerranCorvetteTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranCorvetteTurnsRequired <= 0)
            {
                user.TerranResearch.TerranCorvetteTurnsRequired = 0;
                user.TerranResearch.TerranCorvette = true;
                ResearchMessage = "You have successfully researched the Terran Corvette!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Corvette. {user.TerranResearch.TerranCorvetteTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.TerranFrigateTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.TerranFrigateTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Frigate.";
                return Page();
            }
            user.TerranResearch.TerranFrigateTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranFrigateTurnsRequired <= 0)
            {
                user.TerranResearch.TerranFrigateTurnsRequired = 0;
                user.TerranResearch.TerranFrigate = true;
                ResearchMessage = "You have successfully researched the Terran Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Frigate. {user.TerranResearch.TerranFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (TurnsToInvest > user.Turns.CurrentTurns)
            {
                TurnsToInvest = user.Turns.CurrentTurns;
            }
            if (TurnsToInvest > user.TerranResearch.TerranDestroyerTurnsRequired)
            {
                TurnsToInvest = user.TerranResearch.TerranDestroyerTurnsRequired;
            }
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Destroyer.";
                return Page();
            }
            user.TerranResearch.TerranDestroyerTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranDestroyerTurnsRequired <= 0)
            {
                user.TerranResearch.TerranDestroyerTurnsRequired = 0;
                user.TerranResearch.TerranDestroyer = true;
                ResearchMessage = "You have successfully researched Terran Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Destroyer. {user.TerranResearch.TerranDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranCruiserTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Cruiser.";
                return Page();
            }
            user.TerranResearch.TerranCruiserTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranCruiserTurnsRequired <= 0)
            {
                user.TerranResearch.TerranCruiserTurnsRequired = 0;
                user.TerranResearch.TerranCruiser = true;
                ResearchMessage = "You have successfully researched the Terran Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Cruiser. {user.TerranResearch.TerranCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranBattleshipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranBattleshipTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranBattleshipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranBattleshipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Battleship.";
                return Page();
            }
            user.TerranResearch.TerranBattleshipTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranBattleshipTurnsRequired <= 0)
            {
                user.TerranResearch.TerranBattleshipTurnsRequired = 0;
                user.TerranResearch.TerranBattleship = true;
                ResearchMessage = "You have successfully researched the Terran Battleship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Battleship. {user.TerranResearch.TerranBattleshipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranAdvancedScoutAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranAdvancedScoutTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranAdvancedScoutTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranAdvancedScoutTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Advanced Scout.";
                return Page();
            }
            user.TerranResearch.TerranAdvancedScoutTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranAdvancedScoutTurnsRequired <= 0)
            {
                user.TerranResearch.TerranAdvancedScoutTurnsRequired = 0;
                user.TerranResearch.TerranAdvancedScout = true;
                ResearchMessage = "You have successfully researched the Terran Advanced Scout!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Advanced Scout. {user.TerranResearch.TerranAdvancedScoutTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranDreadnaughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranDreadnaughtTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranDreadnaughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranDreadnaughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Dreadnaught.";
                return Page();
            }
            user.TerranResearch.TerranDreadnaughtTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranDreadnaughtTurnsRequired <= 0)
            {
                user.TerranResearch.TerranDreadnaughtTurnsRequired = 0;
                user.TerranResearch.TerranDreadnaught = true;
                ResearchMessage = "You have successfully researched the Terran Dreadnaught!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Dreadnaught. {user.TerranResearch.TerranDreadnaughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranStarbaseAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranStarbaseTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranStarbaseTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranStarbaseTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Starbase.";
                return Page();
            }
            user.TerranResearch.TerranStarbaseTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranStarbaseTurnsRequired <= 0)
            {
                user.TerranResearch.TerranStarbaseTurnsRequired = 0;
                user.TerranResearch.TerranStarbase = true;
                ResearchMessage = "You have successfully researched the Terran Starbase!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Starbase. {user.TerranResearch.TerranStarbaseTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchTerranJuggernaughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.TerranJuggernaughtTurnsRequired)
                TurnsToInvest = user.TerranResearch.TerranJuggernaughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.TerranJuggernaughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched the Terran Juggernaught.";
                return Page();
            }
            user.TerranResearch.TerranJuggernaughtTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.TerranJuggernaughtTurnsRequired <= 0)
            {
                user.TerranResearch.TerranJuggernaughtTurnsRequired = 0;
                user.TerranResearch.TerranJuggernaught = true;
                ResearchMessage = "You have successfully researched the Terran Juggernaught!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching the Terran Juggernaught. {user.TerranResearch.TerranJuggernaughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedRadarAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.AdvancedRadarTurnsRequired)
                TurnsToInvest = user.TerranResearch.AdvancedRadarTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.AdvancedRadarTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Advanced Radar.";
                return Page();
            }
            user.TerranResearch.AdvancedRadarTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.AdvancedRadarTurnsRequired <= 0)
            {
                user.TerranResearch.AdvancedRadarTurnsRequired = 0;
                user.TerranResearch.AdvancedRadar = true;
                ResearchMessage = "You have successfully researched Advanced Radar!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Advanced Radar. {user.TerranResearch.AdvancedRadarTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchCrystalLensesAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.CrystalLensesTurnsRequired)
                TurnsToInvest = user.TerranResearch.CrystalLensesTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.CrystalLensesTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Crystal Lenses.";
                return Page();
            }
            user.TerranResearch.CrystalLensesTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.CrystalLensesTurnsRequired <= 0)
            {
                user.TerranResearch.CrystalLensesTurnsRequired = 0;
                user.TerranResearch.CrystalLenses = true;
                ResearchMessage = "You have successfully researched Crystal Lenses!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Crystal Lenses. {user.TerranResearch.CrystalLensesTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchPhotonTorpedoAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.PhotonTorpedoTurnsRequired)
                TurnsToInvest = user.TerranResearch.PhotonTorpedoTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.PhotonTorpedoTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Photon Torpedo.";
                return Page();
            }
            user.TerranResearch.PhotonTorpedoTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.PhotonTorpedoTurnsRequired <= 0)
            {
                user.TerranResearch.PhotonTorpedoTurnsRequired = 0;
                user.TerranResearch.PhotonTorpedo = true;
                ResearchMessage = "You have successfully researched Photon Torpedo!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Photon Torpedo. {user.TerranResearch.PhotonTorpedoTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchPhotonCorvetteAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.PhotonCorvetteTurnsRequired)
                TurnsToInvest = user.TerranResearch.PhotonCorvetteTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.PhotonCorvetteTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Photon Corvette.";
                return Page();
            }
            user.TerranResearch.PhotonCorvetteTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.PhotonCorvetteTurnsRequired <= 0)
            {
                user.TerranResearch.PhotonCorvetteTurnsRequired = 0;
                user.TerranResearch.PhotonCorvette = true;
                ResearchMessage = "You have successfully researched Photon Corvette!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Photon Corvette. {user.TerranResearch.PhotonCorvetteTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchPhotonFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.PhotonFrigateTurnsRequired)
                TurnsToInvest = user.TerranResearch.PhotonFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.PhotonFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Photon Frigate.";
                return Page();
            }
            user.TerranResearch.PhotonFrigateTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.PhotonFrigateTurnsRequired <= 0)
            {
                user.TerranResearch.PhotonFrigateTurnsRequired = 0;
                user.TerranResearch.PhotonFrigate = true;
                ResearchMessage = "You have successfully researched Photon Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Photon Frigate. {user.TerranResearch.PhotonFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchPhotonDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.PhotonDestroyerTurnsRequired)
                TurnsToInvest = user.TerranResearch.PhotonDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.PhotonDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Photon Destroyer.";
                return Page();
            }
            user.TerranResearch.PhotonDestroyerTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.PhotonDestroyerTurnsRequired <= 0)
            {
                user.TerranResearch.PhotonDestroyerTurnsRequired = 0;
                user.TerranResearch.PhotonDestroyer = true;
                ResearchMessage = "You have successfully researched Photon Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Photon Destroyer. {user.TerranResearch.PhotonDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }
        public async Task<IActionResult> OnPostResearchPhotonCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.TerranResearch.PhotonCruiserTurnsRequired)
                TurnsToInvest = user.TerranResearch.PhotonCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.TerranResearch.PhotonCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Photon Cruiser.";
                return Page();
            }
            user.TerranResearch.PhotonCruiserTurnsRequired -= TurnsToInvest;
            if (user.TerranResearch.PhotonCruiserTurnsRequired <= 0)
            {
                user.TerranResearch.PhotonCruiserTurnsRequired = 0;
                user.TerranResearch.PhotonCruiser = true;
                ResearchMessage = "You have successfully researched Photon Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Photon Cruiser. {user.TerranResearch.PhotonCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();

            return Page();
        }

        //aminer
        public async Task<IActionResult> OnPostResearchStarshipEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.StarshipEngineeringTurnsRequired)
                TurnsToInvest = user.AMinerResearch.StarshipEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.StarshipEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Starship Engineering.";
                return Page();
            }
            user.AMinerResearch.StarshipEngineeringTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.StarshipEngineeringTurnsRequired <= 0)
            {
                user.AMinerResearch.StarshipEngineeringTurnsRequired = 0;
                user.AMinerResearch.StarshipEngineering = true;
                ResearchMessage = "You have successfully researched Starship Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Starship Engineering. {user.AMinerResearch.StarshipEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFlagshipEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.FlagshipEngineeringTurnsRequired)
                TurnsToInvest = user.AMinerResearch.FlagshipEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.FlagshipEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Flagship Engineering.";
                return Page();
            }
            user.AMinerResearch.FlagshipEngineeringTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.FlagshipEngineeringTurnsRequired <= 0)
            {
                user.AMinerResearch.FlagshipEngineeringTurnsRequired = 0;
                user.AMinerResearch.FlagshipEngineering = true;
                ResearchMessage = "You have successfully researched Flagship Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Flagship Engineering. {user.AMinerResearch.FlagshipEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AdvancedEngineeringTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AdvancedEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Advanced Engineering.";
                return Page();
            }
            user.AMinerResearch.AdvancedEngineeringTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                user.AMinerResearch.AdvancedEngineeringTurnsRequired = 0;
                user.AMinerResearch.AdvancedEngineering = true;
                ResearchMessage = "You have successfully researched Advanced Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Advanced Engineering. {user.AMinerResearch.AdvancedEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchSensorEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.SensorEngineeringTurnsRequired)
                TurnsToInvest = user.AMinerResearch.SensorEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.SensorEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Sensor Engineering.";
                return Page();
            }
            user.AMinerResearch.SensorEngineeringTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.SensorEngineeringTurnsRequired <= 0)
            {
                user.AMinerResearch.SensorEngineeringTurnsRequired = 0;
                user.AMinerResearch.SensorEngineering = true;
                ResearchMessage = "You have successfully researched Sensor Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Sensor Engineering. {user.AMinerResearch.SensorEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchStarbaseEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.StarbaseEngineeringTurnsRequired)
                TurnsToInvest = user.AMinerResearch.StarbaseEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.StarbaseEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Starbase Engineering.";
                return Page();
            }
            user.AMinerResearch.StarbaseEngineeringTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.StarbaseEngineeringTurnsRequired <= 0)
            {
                user.AMinerResearch.StarbaseEngineeringTurnsRequired = 0;
                user.AMinerResearch.StarbaseEngineering = true;
                ResearchMessage = "You have successfully researched Starbase Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Starbase Engineering. {user.AMinerResearch.StarbaseEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaCorvetteAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaCorvetteTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaCorvetteTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaCorvetteTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Corvette.";
                return Page();
            }
            user.AMinerResearch.AsphaCorvetteTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaCorvetteTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaCorvetteTurnsRequired = 0;
                user.AMinerResearch.AsphaCorvette = true;
                ResearchMessage = "You have successfully researched Aspha Corvette!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Corvette. {user.AMinerResearch.AsphaCorvetteTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaFrigateTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Frigate.";
                return Page();
            }
            user.AMinerResearch.AsphaFrigateTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaFrigateTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaFrigateTurnsRequired = 0;
                user.AMinerResearch.AsphaFrigate = true;
                ResearchMessage = "You have successfully researched Aspha Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Frigate. {user.AMinerResearch.AsphaFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaDestroyerTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Destroyer.";
                return Page();
            }
            user.AMinerResearch.AsphaDestroyerTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaDestroyerTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaDestroyerTurnsRequired = 0;
                user.AMinerResearch.AsphaDestroyer = true;
                ResearchMessage = "You have successfully researched Aspha Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Destroyer. {user.AMinerResearch.AsphaDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaCruiserTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Cruiser.";
                return Page();
            }
            user.AMinerResearch.AsphaCruiserTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaCruiserTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaCruiserTurnsRequired = 0;
                user.AMinerResearch.AsphaCruiser = true;
                ResearchMessage = "You have successfully researched Aspha Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Cruiser. {user.AMinerResearch.AsphaCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaBattleshipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaBattleshipTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaBattleshipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaBattleshipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Battleship.";
                return Page();
            }
            user.AMinerResearch.AsphaBattleshipTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaBattleshipTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaBattleshipTurnsRequired = 0;
                user.AMinerResearch.AsphaBattleship = true;
                ResearchMessage = "You have successfully researched Aspha Battleship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Battleship. {user.AMinerResearch.AsphaBattleshipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaDreadnoughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaDreadnoughtTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaDreadnoughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaDreadnoughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Dreadnought.";
                return Page();
            }
            user.AMinerResearch.AsphaDreadnoughtTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaDreadnoughtTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaDreadnoughtTurnsRequired = 0;
                user.AMinerResearch.AsphaDreadnought = true;
                ResearchMessage = "You have successfully researched Aspha Dreadnought!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Dreadnought. {user.AMinerResearch.AsphaDreadnoughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaSeekerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaSeekerTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaSeekerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaSeekerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Seeker.";
                return Page();
            }
            user.AMinerResearch.AsphaSeekerTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaSeekerTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaSeekerTurnsRequired = 0;
                user.AMinerResearch.AsphaSeeker = true;
                ResearchMessage = "You have successfully researched Aspha Seeker!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Seeker. {user.AMinerResearch.AsphaSeekerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaRangerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaRangerTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaRangerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaRangerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Ranger.";
                return Page();
            }
            user.AMinerResearch.AsphaRangerTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaRangerTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaRangerTurnsRequired = 0;
                user.AMinerResearch.AsphaRanger = true;
                ResearchMessage = "You have successfully researched Aspha Ranger!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Ranger. {user.AMinerResearch.AsphaRangerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaLightStarbaseAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaLightStarbaseTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaLightStarbaseTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaLightStarbaseTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Light Starbase.";
                return Page();
            }
            user.AMinerResearch.AsphaLightStarbaseTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaLightStarbaseTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaLightStarbaseTurnsRequired = 0;
                user.AMinerResearch.AsphaLightStarbase = true;
                ResearchMessage = "You have successfully researched Aspha Light Starbase!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Light Starbase. {user.AMinerResearch.AsphaLightStarbaseTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAsphaHeavyStarbaseAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired)
                TurnsToInvest = user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Aspha Heavy Starbase.";
                return Page();
            }
            user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired <= 0)
            {
                user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired = 0;
                user.AMinerResearch.AsphaHeavyStarbase = true;
                ResearchMessage = "You have successfully researched Aspha Heavy Starbase!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Aspha Heavy Starbase. {user.AMinerResearch.AsphaHeavyStarbaseTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchGClassShipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.GClassShipTurnsRequired)
                TurnsToInvest = user.AMinerResearch.GClassShipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.GClassShipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched G Class Ship.";
                return Page();
            }
            user.AMinerResearch.GClassShipTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.GClassShipTurnsRequired <= 0)
            {
                user.AMinerResearch.GClassShipTurnsRequired = 0;
                user.AMinerResearch.GClassShip = true;
                ResearchMessage = "You have successfully researched G Class Ship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching G Class Ship. {user.AMinerResearch.GClassShipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchGClassJuggernaughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.AMinerResearch.GClassJuggernaughtTurnsRequired)
                TurnsToInvest = user.AMinerResearch.GClassJuggernaughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.AMinerResearch.GClassJuggernaughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched G Class Juggernaught.";
                return Page();
            }
            user.AMinerResearch.GClassJuggernaughtTurnsRequired -= TurnsToInvest;
            if (user.AMinerResearch.GClassJuggernaughtTurnsRequired <= 0)
            {
                user.AMinerResearch.GClassJuggernaughtTurnsRequired = 0;
                user.AMinerResearch.GClassJuggernaught = true;
                ResearchMessage = "You have successfully researched G Class Juggernaught!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching G Class Juggernaught. {user.AMinerResearch.GClassJuggernaughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // Collective
        public async Task<IActionResult> OnPostResearchFighterClassAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.FighterClassTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.FighterClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.FighterClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Fighter Class.";
                return Page();
            }
            user.CollectiveResearch.FighterClassTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.FighterClassTurnsRequired <= 0)
            {
                user.CollectiveResearch.FighterClassTurnsRequired = 0;
                user.CollectiveResearch.FighterClass = true;
                ResearchMessage = "You have successfully researched Fighter Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Fighter Class. {user.CollectiveResearch.FighterClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFrigateClassAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.FrigateClassTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.FrigateClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.FrigateClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Frigate Class.";
                return Page();
            }
            user.CollectiveResearch.FrigateClassTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.FrigateClassTurnsRequired <= 0)
            {
                user.CollectiveResearch.FrigateClassTurnsRequired = 0;
                user.CollectiveResearch.FrigateClass = true;
                ResearchMessage = "You have successfully researched Frigate Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Frigate Class. {user.CollectiveResearch.FrigateClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchDestroyerClassAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.DestroyerClassTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.DestroyerClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.DestroyerClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Destroyer Class.";
                return Page();
            }
            user.CollectiveResearch.DestroyerClassTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.DestroyerClassTurnsRequired <= 0)
            {
                user.CollectiveResearch.DestroyerClassTurnsRequired = 0;
                user.CollectiveResearch.DestroyerClass = true;
                ResearchMessage = "You have successfully researched Destroyer Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Destroyer Class. {user.CollectiveResearch.DestroyerClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCruiserClassAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.CruiserClassTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.CruiserClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.CruiserClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cruiser Class.";
                return Page();
            }
            user.CollectiveResearch.CruiserClassTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.CruiserClassTurnsRequired <= 0)
            {
                user.CollectiveResearch.CruiserClassTurnsRequired = 0;
                user.CollectiveResearch.CruiserClass = true;
                ResearchMessage = "You have successfully researched Cruiser Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cruiser Class. {user.CollectiveResearch.CruiserClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassShipsAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassShipsTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassShipsTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassShipsTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Ships.";
                return Page();
            }
            user.CollectiveResearch.HClassShipsTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassShipsTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassShipsTurnsRequired = 0;
                user.CollectiveResearch.HClassShips = true;
                ResearchMessage = "You have successfully researched H-Class Ships!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Ships. {user.CollectiveResearch.HClassShipsTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchRClassFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.RClassFrigateTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.RClassFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.RClassFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched R-Class Frigate.";
                return Page();
            }
            user.CollectiveResearch.RClassFrigateTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.RClassFrigateTurnsRequired <= 0)
            {
                user.CollectiveResearch.RClassFrigateTurnsRequired = 0;
                user.CollectiveResearch.RClassFrigate = true;
                ResearchMessage = "You have successfully researched R-Class Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching R-Class Frigate. {user.CollectiveResearch.RClassFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassDestroyerTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Destroyer.";
                return Page();
            }
            user.CollectiveResearch.HClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassDestroyerTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassDestroyerTurnsRequired = 0;
                user.CollectiveResearch.HClassDestroyer = true;
                ResearchMessage = "You have successfully researched H-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Destroyer. {user.CollectiveResearch.HClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchRClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.RClassDestroyerTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.RClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.RClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched R-Class Destroyer.";
                return Page();
            }
            user.CollectiveResearch.RClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.RClassDestroyerTurnsRequired <= 0)
            {
                user.CollectiveResearch.RClassDestroyerTurnsRequired = 0;
                user.CollectiveResearch.RClassDestroyer = true;
                ResearchMessage = "You have successfully researched R-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching R-Class Destroyer. {user.CollectiveResearch.RClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassCruiserTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Cruiser.";
                return Page();
            }
            user.CollectiveResearch.HClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassCruiserTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassCruiserTurnsRequired = 0;
                user.CollectiveResearch.HClassCruiser = true;
                ResearchMessage = "You have successfully researched H-Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Cruiser. {user.CollectiveResearch.HClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchRClassBattleshipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.RClassBattleshipTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.RClassBattleshipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.RClassBattleshipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched R-Class Battleship.";
                return Page();
            }
            user.CollectiveResearch.RClassBattleshipTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.RClassBattleshipTurnsRequired <= 0)
            {
                user.CollectiveResearch.RClassBattleshipTurnsRequired = 0;
                user.CollectiveResearch.RClassBattleship = true;
                ResearchMessage = "You have successfully researched R-Class Battleship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching R-Class Battleship. {user.CollectiveResearch.RClassBattleshipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassDreadnoughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassDreadnoughtTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassDreadnoughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassDreadnoughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Dreadnought.";
                return Page();
            }
            user.CollectiveResearch.HClassDreadnoughtTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassDreadnoughtTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassDreadnoughtTurnsRequired = 0;
                user.CollectiveResearch.HClassDreadnought = true;
                ResearchMessage = "You have successfully researched H-Class Dreadnought!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Dreadnought. {user.CollectiveResearch.HClassDreadnoughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchRClassDreadnoughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.RClassDreadnoughtTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.RClassDreadnoughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.RClassDreadnoughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched R-Class Dreadnought.";
                return Page();
            }
            user.CollectiveResearch.RClassDreadnoughtTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.RClassDreadnoughtTurnsRequired <= 0)
            {
                user.CollectiveResearch.RClassDreadnoughtTurnsRequired = 0;
                user.CollectiveResearch.RClassDreadnought = true;
                ResearchMessage = "You have successfully researched R-Class Dreadnought!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching R-Class Dreadnought. {user.CollectiveResearch.RClassDreadnoughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchRClassJuggernaughtAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.RClassJuggernaughtTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.RClassJuggernaughtTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.RClassJuggernaughtTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched R-Class Juggernaught.";
                return Page();
            }
            user.CollectiveResearch.RClassJuggernaughtTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.RClassJuggernaughtTurnsRequired <= 0)
            {
                user.CollectiveResearch.RClassJuggernaughtTurnsRequired = 0;
                user.CollectiveResearch.RClassJuggernaught = true;
                ResearchMessage = "You have successfully researched R-Class Juggernaught!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching R-Class Juggernaught. {user.CollectiveResearch.RClassJuggernaughtTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassLeviathanAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassLeviathanTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassLeviathanTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassLeviathanTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Leviathan.";
                return Page();
            }
            user.CollectiveResearch.HClassLeviathanTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassLeviathanTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassLeviathanTurnsRequired = 0;
                user.CollectiveResearch.HClassLeviathan = true;
                ResearchMessage = "You have successfully researched H-Class Leviathan!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Leviathan. {user.CollectiveResearch.HClassLeviathanTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchHClassStarbaseAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveResearch.HClassStarbaseTurnsRequired)
                TurnsToInvest = user.CollectiveResearch.HClassStarbaseTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveResearch.HClassStarbaseTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched H-Class Starbase.";
                return Page();
            }
            user.CollectiveResearch.HClassStarbaseTurnsRequired -= TurnsToInvest;
            if (user.CollectiveResearch.HClassStarbaseTurnsRequired <= 0)
            {
                user.CollectiveResearch.HClassStarbaseTurnsRequired = 0;
                user.CollectiveResearch.HClassStarbase = true;
                ResearchMessage = "You have successfully researched H-Class Starbase!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching H-Class Starbase. {user.CollectiveResearch.HClassStarbaseTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCollectiveClusteringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired)
                TurnsToInvest = user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Collective Clustering.";
                return Page();
            }
            user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired -= TurnsToInvest;
            if (user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired <= 0)
            {
                user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired = 0;
                user.CollectiveSpecificResearch.CollectiveClustering = true;
                ResearchMessage = "You have successfully researched Collective Clustering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Collective Clustering. {user.CollectiveSpecificResearch.CollectiveClusteringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // viral
        public async Task<IActionResult> OnPostResearchClass1ViralHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.Class1ViralHullTurnsRequired)
                TurnsToInvest = user.ViralResearch.Class1ViralHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.Class1ViralHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 1 Viral Hull.";
                return Page();
            }
            user.ViralResearch.Class1ViralHullTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.Class1ViralHullTurnsRequired <= 0)
            {
                user.ViralResearch.Class1ViralHullTurnsRequired = 0;
                user.ViralResearch.Class1ViralHull = true;
                ResearchMessage = "You have successfully researched Class 1 Viral Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 1 Viral Hull. {user.ViralResearch.Class1ViralHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchClass2ViralHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.Class2ViralHullTurnsRequired)
                TurnsToInvest = user.ViralResearch.Class2ViralHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.Class2ViralHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 2 Viral Hull.";
                return Page();
            }
            user.ViralResearch.Class2ViralHullTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.Class2ViralHullTurnsRequired <= 0)
            {
                user.ViralResearch.Class2ViralHullTurnsRequired = 0;
                user.ViralResearch.Class2ViralHull = true;
                ResearchMessage = "You have successfully researched Class 2 Viral Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 2 Viral Hull. {user.ViralResearch.Class2ViralHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedExplorationAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.AdvancedExplorationTurnsRequired)
                TurnsToInvest = user.ViralResearch.AdvancedExplorationTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.AdvancedExplorationTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Advanced Exploration.";
                return Page();
            }
            user.ViralResearch.AdvancedExplorationTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.AdvancedExplorationTurnsRequired <= 0)
            {
                user.ViralResearch.AdvancedExplorationTurnsRequired = 0;
                user.ViralResearch.AdvancedExploration = true;
                ResearchMessage = "You have successfully researched Advanced Exploration!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Advanced Exploration. {user.ViralResearch.AdvancedExplorationTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchKohoutekScoutAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.KohoutekScoutTurnsRequired)
                TurnsToInvest = user.ViralResearch.KohoutekScoutTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.KohoutekScoutTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Kohoutek Scout.";
                return Page();
            }
            user.ViralResearch.KohoutekScoutTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.KohoutekScoutTurnsRequired <= 0)
            {
                user.ViralResearch.KohoutekScoutTurnsRequired = 0;
                user.ViralResearch.KohoutekScout = true;
                ResearchMessage = "You have successfully researched Kohoutek Scout!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Kohoutek Scout. {user.ViralResearch.KohoutekScoutTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchVClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.VClassDestroyerTurnsRequired)
                TurnsToInvest = user.ViralResearch.VClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.VClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched V Class Destroyer.";
                return Page();
            }
            user.ViralResearch.VClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.VClassDestroyerTurnsRequired <= 0)
            {
                user.ViralResearch.VClassDestroyerTurnsRequired = 0;
                user.ViralResearch.VClassDestroyer = true;
                ResearchMessage = "You have successfully researched V Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching V Class Destroyer. {user.ViralResearch.VClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchVClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.VClassCruiserTurnsRequired)
                TurnsToInvest = user.ViralResearch.VClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.VClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched V Class Cruiser.";
                return Page();
            }
            user.ViralResearch.VClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.VClassCruiserTurnsRequired <= 0)
            {
                user.ViralResearch.VClassCruiserTurnsRequired = 0;
                user.ViralResearch.VClassCruiser = true;
                ResearchMessage = "You have successfully researched V Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching V Class Cruiser. {user.ViralResearch.VClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchBClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.BClassCruiserTurnsRequired)
                TurnsToInvest = user.ViralResearch.BClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.BClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched B Class Cruiser.";
                return Page();
            }
            user.ViralResearch.BClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.BClassCruiserTurnsRequired <= 0)
            {
                user.ViralResearch.BClassCruiserTurnsRequired = 0;
                user.ViralResearch.BClassCruiser = true;
                ResearchMessage = "You have successfully researched B Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching B Class Cruiser. {user.ViralResearch.BClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchBClassStarbaseAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralResearch.BClassStarbaseTurnsRequired)
                TurnsToInvest = user.ViralResearch.BClassStarbaseTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralResearch.BClassStarbaseTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched B Class Starbase.";
                return Page();
            }
            user.ViralResearch.BClassStarbaseTurnsRequired -= TurnsToInvest;
            if (user.ViralResearch.BClassStarbaseTurnsRequired <= 0)
            {
                user.ViralResearch.BClassStarbaseTurnsRequired = 0;
                user.ViralResearch.BClassStarbase = true;
                ResearchMessage = "You have successfully researched B Class Starbase!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching B Class Starbase. {user.ViralResearch.BClassStarbaseTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchViralClusteringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralSpecificResearch.ViralClusteringTurnsRequired)
                TurnsToInvest = user.ViralSpecificResearch.ViralClusteringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralSpecificResearch.ViralClusteringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Viral Clustering.";
                return Page();
            }
            user.ViralSpecificResearch.ViralClusteringTurnsRequired -= TurnsToInvest;
            if (user.ViralSpecificResearch.ViralClusteringTurnsRequired <= 0)
            {
                user.ViralSpecificResearch.ViralClusteringTurnsRequired = 0;
                user.ViralSpecificResearch.ViralClustering = true;
                ResearchMessage = "You have successfully researched Viral Clustering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Viral Clustering. {user.ViralSpecificResearch.ViralClusteringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchUnreverseEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired)
                TurnsToInvest = user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Unreverse Engineering.";
                return Page();
            }
            user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired -= TurnsToInvest;
            if (user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired <= 0)
            {
                user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired = 0;
                user.ViralSpecificResearch.UnreverseEngineering = true;
                ResearchMessage = "You have successfully researched Unreverse Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Unreverse Engineering. {user.ViralSpecificResearch.UnreverseEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // guardian
        public async Task<IActionResult> OnPostResearchFighterClassGAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.FighterClassTurnsRequired)
                TurnsToInvest = user.GuardianResearch.FighterClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.FighterClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Fighter Class.";
                return Page();
            }
            user.GuardianResearch.FighterClassTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.FighterClassTurnsRequired <= 0)
            {
                user.GuardianResearch.FighterClassTurnsRequired = 0;
                user.GuardianResearch.FighterClass = true;
                ResearchMessage = "You have successfully researched Guardian Fighter Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Fighter Class. {user.GuardianResearch.FighterClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCorvetteClassAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.CorvetteClassTurnsRequired)
                TurnsToInvest = user.GuardianResearch.CorvetteClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.CorvetteClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Corvette Class.";
                return Page();
            }
            user.GuardianResearch.CorvetteClassTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.CorvetteClassTurnsRequired <= 0)
            {
                user.GuardianResearch.CorvetteClassTurnsRequired = 0;
                user.GuardianResearch.CorvetteClass = true;
                ResearchMessage = "You have successfully researched Guardian Corvette Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Corvette Class. {user.GuardianResearch.CorvetteClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFrigateClassGAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.FrigateClassTurnsRequired)
                TurnsToInvest = user.GuardianResearch.FrigateClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.FrigateClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Frigate Class.";
                return Page();
            }
            user.GuardianResearch.FrigateClassTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.FrigateClassTurnsRequired <= 0)
            {
                user.GuardianResearch.FrigateClassTurnsRequired = 0;
                user.GuardianResearch.FrigateClass = true;
                ResearchMessage = "You have successfully researched Guardian Frigate Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Frigate Class. {user.GuardianResearch.FrigateClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchDestroyerClassGAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.DestroyerClassTurnsRequired)
                TurnsToInvest = user.GuardianResearch.DestroyerClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.DestroyerClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Destroyer Class.";
                return Page();
            }
            user.GuardianResearch.DestroyerClassTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.DestroyerClassTurnsRequired <= 0)
            {
                user.GuardianResearch.DestroyerClassTurnsRequired = 0;
                user.GuardianResearch.DestroyerClass = true;
                ResearchMessage = "You have successfully researched Guardian Destroyer Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Destroyer Class. {user.GuardianResearch.DestroyerClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCruiserClassGAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.CruiserClassTurnsRequired)
                TurnsToInvest = user.GuardianResearch.CruiserClassTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.CruiserClassTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Cruiser Class.";
                return Page();
            }
            user.GuardianResearch.CruiserClassTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.CruiserClassTurnsRequired <= 0)
            {
                user.GuardianResearch.CruiserClassTurnsRequired = 0;
                user.GuardianResearch.CruiserClass = true;
                ResearchMessage = "You have successfully researched Guardian Cruiser Class!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Cruiser Class. {user.GuardianResearch.CruiserClassTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchWeaponsPlatformAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.WeaponsPlatformTurnsRequired)
                TurnsToInvest = user.GuardianResearch.WeaponsPlatformTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.WeaponsPlatformTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Weapons Platform.";
                return Page();
            }
            user.GuardianResearch.WeaponsPlatformTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.WeaponsPlatformTurnsRequired <= 0)
            {
                user.GuardianResearch.WeaponsPlatformTurnsRequired = 0;
                user.GuardianResearch.WeaponsPlatform = true;
                ResearchMessage = "You have successfully researched Guardian Weapons Platform!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Weapons Platform. {user.GuardianResearch.WeaponsPlatformTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedScoutsAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.AdvancedScoutsTurnsRequired)
                TurnsToInvest = user.GuardianResearch.AdvancedScoutsTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.AdvancedScoutsTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Advanced Scouts.";
                return Page();
            }
            user.GuardianResearch.AdvancedScoutsTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.AdvancedScoutsTurnsRequired <= 0)
            {
                user.GuardianResearch.AdvancedScoutsTurnsRequired = 0;
                user.GuardianResearch.AdvancedScouts = true;
                ResearchMessage = "You have successfully researched Guardian Advanced Scouts!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Advanced Scouts. {user.GuardianResearch.AdvancedScoutsTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLClassFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.LClassFrigateTurnsRequired)
                TurnsToInvest = user.GuardianResearch.LClassFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.LClassFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian L-Class Frigate.";
                return Page();
            }
            user.GuardianResearch.LClassFrigateTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.LClassFrigateTurnsRequired <= 0)
            {
                user.GuardianResearch.LClassFrigateTurnsRequired = 0;
                user.GuardianResearch.LClassFrigate = true;
                ResearchMessage = "You have successfully researched Guardian L-Class Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian L-Class Frigate. {user.GuardianResearch.LClassFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.LClassDestroyerTurnsRequired)
                TurnsToInvest = user.GuardianResearch.LClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.LClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian L-Class Destroyer.";
                return Page();
            }
            user.GuardianResearch.LClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.LClassDestroyerTurnsRequired <= 0)
            {
                user.GuardianResearch.LClassDestroyerTurnsRequired = 0;
                user.GuardianResearch.LClassDestroyer = true;
                ResearchMessage = "You have successfully researched Guardian L-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian L-Class Destroyer. {user.GuardianResearch.LClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.LClassCruiserTurnsRequired)
                TurnsToInvest = user.GuardianResearch.LClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.LClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian L-Class Cruiser.";
                return Page();
            }
            user.GuardianResearch.LClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.LClassCruiserTurnsRequired <= 0)
            {
                user.GuardianResearch.LClassCruiserTurnsRequired = 0;
                user.GuardianResearch.LClassCruiser = true;
                ResearchMessage = "You have successfully researched Guardian L-Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian L-Class Cruiser. {user.GuardianResearch.LClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchKClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.KClassDestroyerTurnsRequired)
                TurnsToInvest = user.GuardianResearch.KClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.KClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian K-Class Destroyer.";
                return Page();
            }
            user.GuardianResearch.KClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.KClassDestroyerTurnsRequired <= 0)
            {
                user.GuardianResearch.KClassDestroyerTurnsRequired = 0;
                user.GuardianResearch.KClassDestroyer = true;
                ResearchMessage = "You have successfully researched Guardian K-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian K-Class Destroyer. {user.GuardianResearch.KClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchKClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.KClassCruiserTurnsRequired)
                TurnsToInvest = user.GuardianResearch.KClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.KClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian K-Class Cruiser.";
                return Page();
            }
            user.GuardianResearch.KClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.KClassCruiserTurnsRequired <= 0)
            {
                user.GuardianResearch.KClassCruiserTurnsRequired = 0;
                user.GuardianResearch.KClassCruiser = true;
                ResearchMessage = "You have successfully researched Guardian K-Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian K-Class Cruiser. {user.GuardianResearch.KClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedEngineeringGAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.AdvancedEngineeringTurnsRequired)
                TurnsToInvest = user.GuardianResearch.AdvancedEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Advanced Engineering.";
                return Page();
            }
            user.GuardianResearch.AdvancedEngineeringTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                user.GuardianResearch.AdvancedEngineeringTurnsRequired = 0;
                user.GuardianResearch.AdvancedEngineering = true;
                ResearchMessage = "You have successfully researched Guardian Advanced Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Advanced Engineering. {user.GuardianResearch.AdvancedEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFocusCrystalAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.FocusCrystalTurnsRequired)
                TurnsToInvest = user.GuardianResearch.FocusCrystalTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.FocusCrystalTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Focus Crystal.";
                return Page();
            }
            user.GuardianResearch.FocusCrystalTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.FocusCrystalTurnsRequired <= 0)
            {
                user.GuardianResearch.FocusCrystalTurnsRequired = 0;
                user.GuardianResearch.FocusCrystal = true;
                ResearchMessage = "You have successfully researched Guardian Focus Crystal!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Focus Crystal. {user.GuardianResearch.FocusCrystalTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchOrganicSensorsAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.OrganicSensorsTurnsRequired)
                TurnsToInvest = user.GuardianResearch.OrganicSensorsTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.OrganicSensorsTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Organic Sensors.";
                return Page();
            }
            user.GuardianResearch.OrganicSensorsTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.OrganicSensorsTurnsRequired <= 0)
            {
                user.GuardianResearch.OrganicSensorsTurnsRequired = 0;
                user.GuardianResearch.OrganicSensors = true;
                ResearchMessage = "You have successfully researched Guardian Organic Sensors!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Organic Sensors. {user.GuardianResearch.OrganicSensorsTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLanceTorpodoesAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.GuardianResearch.LanceTorpodoesTurnsRequired)
                TurnsToInvest = user.GuardianResearch.LanceTorpodoesTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.GuardianResearch.LanceTorpodoesTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Guardian Lance Torpodoes.";
                return Page();
            }
            user.GuardianResearch.LanceTorpodoesTurnsRequired -= TurnsToInvest;
            if (user.GuardianResearch.LanceTorpodoesTurnsRequired <= 0)
            {
                user.GuardianResearch.LanceTorpodoesTurnsRequired = 0;
                user.GuardianResearch.LanceTorpodoes = true;
                ResearchMessage = "You have successfully researched Guardian Lance Torpodoes!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Guardian Lance Torpodoes. {user.GuardianResearch.LanceTorpodoesTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // marauder
        public async Task<IActionResult> OnPostResearchClass3MarauderHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.Class3MarauderHullTurnsRequired)
                TurnsToInvest = user.MarauderResearch.Class3MarauderHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.Class3MarauderHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 3 Marauder Hull.";
                return Page();
            }
            user.MarauderResearch.Class3MarauderHullTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.Class3MarauderHullTurnsRequired <= 0)
            {
                user.MarauderResearch.Class3MarauderHullTurnsRequired = 0;
                user.MarauderResearch.Class3MarauderHull = true;
                ResearchMessage = "You have successfully researched Class 3 Marauder Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 3 Marauder Hull. {user.MarauderResearch.Class3MarauderHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchClass4MarauderHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.Class4MarauderHullTurnsRequired)
                TurnsToInvest = user.MarauderResearch.Class4MarauderHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.Class4MarauderHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 4 Marauder Hull.";
                return Page();
            }
            user.MarauderResearch.Class4MarauderHullTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.Class4MarauderHullTurnsRequired <= 0)
            {
                user.MarauderResearch.Class4MarauderHullTurnsRequired = 0;
                user.MarauderResearch.Class4MarauderHull = true;
                ResearchMessage = "You have successfully researched Class 4 Marauder Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 4 Marauder Hull. {user.MarauderResearch.Class4MarauderHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchClass5MarauderHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.Class5MarauderHullTurnsRequired)
                TurnsToInvest = user.MarauderResearch.Class5MarauderHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.Class5MarauderHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 5 Marauder Hull.";
                return Page();
            }
            user.MarauderResearch.Class5MarauderHullTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.Class5MarauderHullTurnsRequired <= 0)
            {
                user.MarauderResearch.Class5MarauderHullTurnsRequired = 0;
                user.MarauderResearch.Class5MarauderHull = true;
                ResearchMessage = "You have successfully researched Class 5 Marauder Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 5 Marauder Hull. {user.MarauderResearch.Class5MarauderHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchClass6MarauderHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.Class6MarauderHullTurnsRequired)
                TurnsToInvest = user.MarauderResearch.Class6MarauderHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.Class6MarauderHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Class 6 Marauder Hull.";
                return Page();
            }
            user.MarauderResearch.Class6MarauderHullTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.Class6MarauderHullTurnsRequired <= 0)
            {
                user.MarauderResearch.Class6MarauderHullTurnsRequired = 0;
                user.MarauderResearch.Class6MarauderHull = true;
                ResearchMessage = "You have successfully researched Class 6 Marauder Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Class 6 Marauder Hull. {user.MarauderResearch.Class6MarauderHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedEngineeringMAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.AdvancedEngineeringTurnsRequired)
                TurnsToInvest = user.MarauderResearch.AdvancedEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Advanced Engineering M.";
                return Page();
            }
            user.MarauderResearch.AdvancedEngineeringTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.AdvancedEngineeringTurnsRequired <= 0)
            {
                user.MarauderResearch.AdvancedEngineeringTurnsRequired = 0;
                user.MarauderResearch.AdvancedEngineering = true;
                ResearchMessage = "You have successfully researched Advanced Engineering M!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Advanced Engineering M. {user.MarauderResearch.AdvancedEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMarauderCorvetteAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.MarauderCorvetteTurnsRequired)
                TurnsToInvest = user.MarauderResearch.MarauderCorvetteTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.MarauderCorvetteTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Marauder Corvette.";
                return Page();
            }
            user.MarauderResearch.MarauderCorvetteTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.MarauderCorvetteTurnsRequired <= 0)
            {
                user.MarauderResearch.MarauderCorvetteTurnsRequired = 0;
                user.MarauderResearch.MarauderCorvette = true;
                ResearchMessage = "You have successfully researched Marauder Corvette!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Marauder Corvette. {user.MarauderResearch.MarauderCorvetteTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMarauderFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.MarauderFrigateTurnsRequired)
                TurnsToInvest = user.MarauderResearch.MarauderFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.MarauderFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Marauder Frigate.";
                return Page();
            }
            user.MarauderResearch.MarauderFrigateTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.MarauderFrigateTurnsRequired <= 0)
            {
                user.MarauderResearch.MarauderFrigateTurnsRequired = 0;
                user.MarauderResearch.MarauderFrigate = true;
                ResearchMessage = "You have successfully researched Marauder Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Marauder Frigate. {user.MarauderResearch.MarauderFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMarauderDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.MarauderDestroyerTurnsRequired)
                TurnsToInvest = user.MarauderResearch.MarauderDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.MarauderDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Marauder Destroyer.";
                return Page();
            }
            user.MarauderResearch.MarauderDestroyerTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.MarauderDestroyerTurnsRequired <= 0)
            {
                user.MarauderResearch.MarauderDestroyerTurnsRequired = 0;
                user.MarauderResearch.MarauderDestroyer = true;
                ResearchMessage = "You have successfully researched Marauder Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Marauder Destroyer. {user.MarauderResearch.MarauderDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMarauderCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.MarauderCruiserTurnsRequired)
                TurnsToInvest = user.MarauderResearch.MarauderCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.MarauderCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Marauder Cruiser.";
                return Page();
            }
            user.MarauderResearch.MarauderCruiserTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.MarauderCruiserTurnsRequired <= 0)
            {
                user.MarauderResearch.MarauderCruiserTurnsRequired = 0;
                user.MarauderResearch.MarauderCruiser = true;
                ResearchMessage = "You have successfully researched Marauder Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Marauder Cruiser. {user.MarauderResearch.MarauderCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMarauderBattleshipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.MarauderBattleshipTurnsRequired)
                TurnsToInvest = user.MarauderResearch.MarauderBattleshipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.MarauderBattleshipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Marauder Battleship.";
                return Page();
            }
            user.MarauderResearch.MarauderBattleshipTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.MarauderBattleshipTurnsRequired <= 0)
            {
                user.MarauderResearch.MarauderBattleshipTurnsRequired = 0;
                user.MarauderResearch.MarauderBattleship = true;
                ResearchMessage = "You have successfully researched Marauder Battleship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Marauder Battleship. {user.MarauderResearch.MarauderBattleshipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchTypeDFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.TypeDFrigateTurnsRequired)
                TurnsToInvest = user.MarauderResearch.TypeDFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.TypeDFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Type D Frigate.";
                return Page();
            }
            user.MarauderResearch.TypeDFrigateTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.TypeDFrigateTurnsRequired <= 0)
            {
                user.MarauderResearch.TypeDFrigateTurnsRequired = 0;
                user.MarauderResearch.TypeDFrigate = true;
                ResearchMessage = "You have successfully researched Type D Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Type D Frigate. {user.MarauderResearch.TypeDFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchTypeDDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.TypeDDestroyerTurnsRequired)
                TurnsToInvest = user.MarauderResearch.TypeDDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.TypeDDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Type D Destroyer.";
                return Page();
            }
            user.MarauderResearch.TypeDDestroyerTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.TypeDDestroyerTurnsRequired <= 0)
            {
                user.MarauderResearch.TypeDDestroyerTurnsRequired = 0;
                user.MarauderResearch.TypeDDestroyer = true;
                ResearchMessage = "You have successfully researched Type D Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Type D Destroyer. {user.MarauderResearch.TypeDDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchTypeDCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.TypeDCruiserTurnsRequired)
                TurnsToInvest = user.MarauderResearch.TypeDCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.TypeDCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Type D Cruiser.";
                return Page();
            }
            user.MarauderResearch.TypeDCruiserTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.TypeDCruiserTurnsRequired <= 0)
            {
                user.MarauderResearch.TypeDCruiserTurnsRequired = 0;
                user.MarauderResearch.TypeDCruiser = true;
                ResearchMessage = "You have successfully researched Type D Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Type D Cruiser. {user.MarauderResearch.TypeDCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchTypeDBattleshipAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.MarauderResearch.TypeDBattleshipTurnsRequired)
                TurnsToInvest = user.MarauderResearch.TypeDBattleshipTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.MarauderResearch.TypeDBattleshipTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Type D Battleship.";
                return Page();
            }
            user.MarauderResearch.TypeDBattleshipTurnsRequired -= TurnsToInvest;
            if (user.MarauderResearch.TypeDBattleshipTurnsRequired <= 0)
            {
                user.MarauderResearch.TypeDBattleshipTurnsRequired = 0;
                user.MarauderResearch.TypeDBattleship = true;
                ResearchMessage = "You have successfully researched Type D Battleship!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Type D Battleship. {user.MarauderResearch.TypeDBattleshipTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // cyril class
        public async Task<IActionResult> OnPostResearchCyrilAlloyAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.CyrilAlloyTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.CyrilAlloyTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.CyrilAlloyTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cyril Alloy.";
                return Page();
            }
            user.CyrilClassResearch.CyrilAlloyTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.CyrilAlloyTurnsRequired <= 0)
            {
                user.CyrilClassResearch.CyrilAlloyTurnsRequired = 0;
                user.CyrilClassResearch.CyrilAlloy = true;
                ResearchMessage = "You have successfully researched Cyril Alloy!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cyril Alloy. {user.CyrilClassResearch.CyrilAlloyTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchSmallCyrilHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.SmallCyrilHullTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.SmallCyrilHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.SmallCyrilHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Small Cyril Hull.";
                return Page();
            }
            user.CyrilClassResearch.SmallCyrilHullTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.SmallCyrilHullTurnsRequired <= 0)
            {
                user.CyrilClassResearch.SmallCyrilHullTurnsRequired = 0;
                user.CyrilClassResearch.SmallCyrilHull = true;
                ResearchMessage = "You have successfully researched Small Cyril Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Small Cyril Hull. {user.CyrilClassResearch.SmallCyrilHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchMediumCyrilHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.MediumCyrilHullTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.MediumCyrilHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.MediumCyrilHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Medium Cyril Hull.";
                return Page();
            }
            user.CyrilClassResearch.MediumCyrilHullTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.MediumCyrilHullTurnsRequired <= 0)
            {
                user.CyrilClassResearch.MediumCyrilHullTurnsRequired = 0;
                user.CyrilClassResearch.MediumCyrilHull = true;
                ResearchMessage = "You have successfully researched Medium Cyril Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Medium Cyril Hull. {user.CyrilClassResearch.MediumCyrilHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLargeCyrilHullAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.LargeCyrilHullTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.LargeCyrilHullTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.LargeCyrilHullTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Large Cyril Hull.";
                return Page();
            }
            user.CyrilClassResearch.LargeCyrilHullTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.LargeCyrilHullTurnsRequired <= 0)
            {
                user.CyrilClassResearch.LargeCyrilHullTurnsRequired = 0;
                user.CyrilClassResearch.LargeCyrilHull = true;
                ResearchMessage = "You have successfully researched Large Cyril Hull!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Large Cyril Hull. {user.CyrilClassResearch.LargeCyrilHullTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCyrilCorvetteAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.CyrilCorvetteTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.CyrilCorvetteTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.CyrilCorvetteTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cyril Corvette.";
                return Page();
            }
            user.CyrilClassResearch.CyrilCorvetteTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.CyrilCorvetteTurnsRequired <= 0)
            {
                user.CyrilClassResearch.CyrilCorvetteTurnsRequired = 0;
                user.CyrilClassResearch.CyrilCorvette = true;
                ResearchMessage = "You have successfully researched Cyril Corvette!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cyril Corvette. {user.CyrilClassResearch.CyrilCorvetteTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCyrilFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.CyrilFrigateTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.CyrilFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.CyrilFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cyril Frigate.";
                return Page();
            }
            user.CyrilClassResearch.CyrilFrigateTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.CyrilFrigateTurnsRequired <= 0)
            {
                user.CyrilClassResearch.CyrilFrigateTurnsRequired = 0;
                user.CyrilClassResearch.CyrilFrigate = true;
                ResearchMessage = "You have successfully researched Cyril Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cyril Frigate. {user.CyrilClassResearch.CyrilFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCyrilDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.CyrilDestroyerTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.CyrilDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.CyrilDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cyril Destroyer.";
                return Page();
            }
            user.CyrilClassResearch.CyrilDestroyerTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.CyrilDestroyerTurnsRequired <= 0)
            {
                user.CyrilClassResearch.CyrilDestroyerTurnsRequired = 0;
                user.CyrilClassResearch.CyrilDestroyer = true;
                ResearchMessage = "You have successfully researched Cyril Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cyril Destroyer. {user.CyrilClassResearch.CyrilDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchCyrilCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.CyrilClassResearch.CyrilCruiserTurnsRequired)
                TurnsToInvest = user.CyrilClassResearch.CyrilCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.CyrilClassResearch.CyrilCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Cyril Cruiser.";
                return Page();
            }
            user.CyrilClassResearch.CyrilCruiserTurnsRequired -= TurnsToInvest;
            if (user.CyrilClassResearch.CyrilCruiserTurnsRequired <= 0)
            {
                user.CyrilClassResearch.CyrilCruiserTurnsRequired = 0;
                user.CyrilClassResearch.CyrilCruiser = true;
                ResearchMessage = "You have successfully researched Cyril Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Cyril Cruiser. {user.CyrilClassResearch.CyrilCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // e class
        public async Task<IActionResult> OnPostResearchExaHertzClassRailgunAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // If you have a property for ExaHertzClassRailgunTurnsRequired, use it here.
            // Otherwise, you may need to add it to EClassResearch.
            // For demonstration, let's assume EClassFighter is the unlock for ExaHertzClassRailgun.
            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.EClassResearch.ExaHertzClassRailgunTurnsRequired)
                TurnsToInvest = user.EClassResearch.ExaHertzClassRailgunTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.EClassResearch.ExaHertzClassRailgunTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched ExaHertz Class Railgun.";
                return Page();
            }
            user.EClassResearch.ExaHertzClassRailgunTurnsRequired -= TurnsToInvest;
            if (user.EClassResearch.ExaHertzClassRailgunTurnsRequired <= 0)
            {
                user.EClassResearch.ExaHertzClassRailgunTurnsRequired = 0;
                user.EClassResearch.ExaHertzClassRailgun = true;
                ResearchMessage = "You have successfully researched ExaHertz Class Railgun!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching ExaHertz Class Railgun. {user.EClassResearch.ExaHertzClassRailgunTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchEClassFighterAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.EClassResearch.EClassFighterTurnsRequired)
                TurnsToInvest = user.EClassResearch.EClassFighterTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.EClassResearch.EClassFighterTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched E-Class Fighter.";
                return Page();
            }
            user.EClassResearch.EClassFighterTurnsRequired -= TurnsToInvest;
            if (user.EClassResearch.EClassFighterTurnsRequired <= 0)
            {
                user.EClassResearch.EClassFighterTurnsRequired = 0;
                user.EClassResearch.EClassFighter = true;
                ResearchMessage = "You have successfully researched E-Class Fighter!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching E-Class Fighter. {user.EClassResearch.EClassFighterTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchEClassFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.EClassResearch.EClassFrigateTurnsRequired)
                TurnsToInvest = user.EClassResearch.EClassFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.EClassResearch.EClassFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched E-Class Frigate.";
                return Page();
            }
            user.EClassResearch.EClassFrigateTurnsRequired -= TurnsToInvest;
            if (user.EClassResearch.EClassFrigateTurnsRequired <= 0)
            {
                user.EClassResearch.EClassFrigateTurnsRequired = 0;
                user.EClassResearch.EClassFrigate = true;
                ResearchMessage = "You have successfully researched E-Class Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching E-Class Frigate. {user.EClassResearch.EClassFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchEClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.EClassResearch.EClassDestroyerTurnsRequired)
                TurnsToInvest = user.EClassResearch.EClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.EClassResearch.EClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched E-Class Destroyer.";
                return Page();
            }
            user.EClassResearch.EClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.EClassResearch.EClassDestroyerTurnsRequired <= 0)
            {
                user.EClassResearch.EClassDestroyerTurnsRequired = 0;
                user.EClassResearch.EClassDestroyer = true;
                ResearchMessage = "You have successfully researched E-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching E-Class Destroyer. {user.EClassResearch.EClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // f class
        public async Task<IActionResult> OnPostResearchFClassEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.FClassResearch.FClassEngineeringTurnsRequired)
                TurnsToInvest = user.FClassResearch.FClassEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.FClassResearch.FClassEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched F-Class Engineering.";
                return Page();
            }
            user.FClassResearch.FClassEngineeringTurnsRequired -= TurnsToInvest;
            if (user.FClassResearch.FClassEngineeringTurnsRequired <= 0)
            {
                user.FClassResearch.FClassEngineeringTurnsRequired = 0;
                user.FClassResearch.FClassEngineering = true;
                ResearchMessage = "You have successfully researched F-Class Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching F-Class Engineering. {user.FClassResearch.FClassEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFClassFrigateAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.FClassResearch.FClassFrigateTurnsRequired)
                TurnsToInvest = user.FClassResearch.FClassFrigateTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.FClassResearch.FClassFrigateTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched F-Class Frigate.";
                return Page();
            }
            user.FClassResearch.FClassFrigateTurnsRequired -= TurnsToInvest;
            if (user.FClassResearch.FClassFrigateTurnsRequired <= 0)
            {
                user.FClassResearch.FClassFrigateTurnsRequired = 0;
                user.FClassResearch.FClassFrigate = true;
                ResearchMessage = "You have successfully researched F-Class Frigate!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching F-Class Frigate. {user.FClassResearch.FClassFrigateTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFClassDestroyerAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.FClassResearch.FClassDestroyerTurnsRequired)
                TurnsToInvest = user.FClassResearch.FClassDestroyerTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.FClassResearch.FClassDestroyerTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched F-Class Destroyer.";
                return Page();
            }
            user.FClassResearch.FClassDestroyerTurnsRequired -= TurnsToInvest;
            if (user.FClassResearch.FClassDestroyerTurnsRequired <= 0)
            {
                user.FClassResearch.FClassDestroyerTurnsRequired = 0;
                user.FClassResearch.FClassDestroyer = true;
                ResearchMessage = "You have successfully researched F-Class Destroyer!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching F-Class Destroyer. {user.FClassResearch.FClassDestroyerTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchFClassCruiserAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.FClassResearch.FClassCruiserTurnsRequired)
                TurnsToInvest = user.FClassResearch.FClassCruiserTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.FClassResearch.FClassCruiserTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched F-Class Cruiser.";
                return Page();
            }
            user.FClassResearch.FClassCruiserTurnsRequired -= TurnsToInvest;
            if (user.FClassResearch.FClassCruiserTurnsRequired <= 0)
            {
                user.FClassResearch.FClassCruiserTurnsRequired = 0;
                user.FClassResearch.FClassCruiser = true;
                ResearchMessage = "You have successfully researched F-Class Cruiser!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching F-Class Cruiser. {user.FClassResearch.FClassCruiserTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // strafez class
        public async Task<IActionResult> OnPostResearchBasicGeneticEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.BasicGeneticEngineeringTurnsRequired)
                TurnsToInvest = user.StrafezResearch.BasicGeneticEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.BasicGeneticEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Basic Genetic Engineering.";
                return Page();
            }
            user.StrafezResearch.BasicGeneticEngineeringTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.BasicGeneticEngineeringTurnsRequired <= 0)
            {
                user.StrafezResearch.BasicGeneticEngineeringTurnsRequired = 0;
                user.StrafezResearch.BasicGeneticEngineering = true;
                ResearchMessage = "You have successfully researched Basic Genetic Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Basic Genetic Engineering. {user.StrafezResearch.BasicGeneticEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchSmallGeneticEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.SmallGeneticEngineeringTurnsRequired)
                TurnsToInvest = user.StrafezResearch.SmallGeneticEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.SmallGeneticEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Small Genetic Engineering.";
                return Page();
            }
            user.StrafezResearch.SmallGeneticEngineeringTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.SmallGeneticEngineeringTurnsRequired <= 0)
            {
                user.StrafezResearch.SmallGeneticEngineeringTurnsRequired = 0;
                user.StrafezResearch.SmallGeneticEngineering = true;
                ResearchMessage = "You have successfully researched Small Genetic Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Small Genetic Engineering. {user.StrafezResearch.SmallGeneticEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLargeGeneticEngineeringAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.LargeGeneticEngineeringTurnsRequired)
                TurnsToInvest = user.StrafezResearch.LargeGeneticEngineeringTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.LargeGeneticEngineeringTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Large Genetic Engineering.";
                return Page();
            }
            user.StrafezResearch.LargeGeneticEngineeringTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.LargeGeneticEngineeringTurnsRequired <= 0)
            {
                user.StrafezResearch.LargeGeneticEngineeringTurnsRequired = 0;
                user.StrafezResearch.LargeGeneticEngineering = true;
                ResearchMessage = "You have successfully researched Large Genetic Engineering!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Large Genetic Engineering. {user.StrafezResearch.LargeGeneticEngineeringTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchSmallStrafezRunnerFodderAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.TerranResearch)
                .Include(u => u.AMinerResearch)
                .Include(u => u.MarauderResearch)
                .Include(u => u.ViralResearch)
                .Include(u => u.ViralSpecificResearch)
                .Include(u => u.CollectiveResearch)
                .Include(u => u.CollectiveSpecificResearch)
                .Include(u => u.GuardianResearch)
                .Include(u => u.ProjectsResearch)
                .Include(u => u.StrafezResearch)
                .Include(u => u.FClassResearch)
                .Include(u => u.CyrilClassResearch)
                .Include(u => u.EClassResearch)
                .Include(u => u.ClusterResearch).Include(u => u.Turns)
                .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired)
                TurnsToInvest = user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Small Strafez Runner Fodder.";
                return Page();
            }
            user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired <= 0)
            {
                user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired = 0;
                user.StrafezResearch.SmallStrafezRunnerFodder = true;
                ResearchMessage = "You have successfully researched Small Strafez Runner Fodder!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Small Strafez Runner Fodder. {user.StrafezResearch.SmallStrafezRunnerFodderTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchLargeStrafezRunnerFodderAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired)
                TurnsToInvest = user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Large Strafez Runner Fodder.";
                return Page();
            }
            user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired <= 0)
            {
                user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired = 0;
                user.StrafezResearch.LargeStrafezRunnerFodder = true;
                ResearchMessage = "You have successfully researched Large Strafez Runner Fodder!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Large Strafez Runner Fodder. {user.StrafezResearch.LargeStrafezRunnerFodderTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchStrafezQueenKingAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.StrafezResearch.StrafezQueenKingTurnsRequired)
                TurnsToInvest = user.StrafezResearch.StrafezQueenKingTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.StrafezResearch.StrafezQueenKingTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Strafez Queen King.";
                return Page();
            }
            user.StrafezResearch.StrafezQueenKingTurnsRequired -= TurnsToInvest;
            if (user.StrafezResearch.StrafezQueenKingTurnsRequired <= 0)
            {
                user.StrafezResearch.StrafezQueenKingTurnsRequired = 0;
                user.StrafezResearch.StrafezQueenKing = true;
                ResearchMessage = "You have successfully researched Strafez Queen King!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Strafez Queen King. {user.StrafezResearch.StrafezQueenKingTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // projects research
        public async Task<IActionResult> OnPostResearchCapsuleLaboratoryAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ProjectsResearch.CapsuleLaboratoryTurnsRequired)
                TurnsToInvest = user.ProjectsResearch.CapsuleLaboratoryTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ProjectsResearch.CapsuleLaboratoryTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Capsule Laboratory.";
                return Page();
            }
            user.ProjectsResearch.CapsuleLaboratoryTurnsRequired -= TurnsToInvest;
            if (user.ProjectsResearch.CapsuleLaboratoryTurnsRequired <= 0)
            {
                user.ProjectsResearch.CapsuleLaboratoryTurnsRequired = 0;
                user.ProjectsResearch.CapsuleLaboratory = true;
                ResearchMessage = "You have successfully researched Capsule Laboratory!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Capsule Laboratory. {user.ProjectsResearch.CapsuleLaboratoryTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchKalZulOvermindAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ProjectsResearch.KalZulOvermindTurnsRequired)
                TurnsToInvest = user.ProjectsResearch.KalZulOvermindTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ProjectsResearch.KalZulOvermindTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Kal Zul Overmind.";
                return Page();
            }
            user.ProjectsResearch.KalZulOvermindTurnsRequired -= TurnsToInvest;
            if (user.ProjectsResearch.KalZulOvermindTurnsRequired <= 0)
            {
                user.ProjectsResearch.KalZulOvermindTurnsRequired = 0;
                user.ProjectsResearch.KalZulOvermind = true;
                ResearchMessage = "You have successfully researched Kal Zul Overmind!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Kal Zul Overmind. {user.ProjectsResearch.KalZulOvermindTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchKalZulArtifactAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ProjectsResearch.KalZulArtifactTurnsRequired)
                TurnsToInvest = user.ProjectsResearch.KalZulArtifactTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ProjectsResearch.KalZulArtifactTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Kal Zul Artifact.";
                return Page();
            }
            user.ProjectsResearch.KalZulArtifactTurnsRequired -= TurnsToInvest;
            if (user.ProjectsResearch.KalZulArtifactTurnsRequired <= 0)
            {
                user.ProjectsResearch.KalZulArtifactTurnsRequired = 0;
                user.ProjectsResearch.KalZulArtifact = true;
                ResearchMessage = "You have successfully researched Kal Zul Artifact!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Kal Zul Artifact. {user.ProjectsResearch.KalZulArtifactTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchITechLaboratoryAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ProjectsResearch.ITechLaboratoryTurnsRequired)
                TurnsToInvest = user.ProjectsResearch.ITechLaboratoryTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ProjectsResearch.ITechLaboratoryTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched ITech Laboratory.";
                return Page();
            }
            user.ProjectsResearch.ITechLaboratoryTurnsRequired -= TurnsToInvest;
            if (user.ProjectsResearch.ITechLaboratoryTurnsRequired <= 0)
            {
                user.ProjectsResearch.ITechLaboratoryTurnsRequired = 0;
                user.ProjectsResearch.ITechLaboratory = true;
                ResearchMessage = "You have successfully researched ITech Laboratory!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching ITech Laboratory. {user.ProjectsResearch.ITechLaboratoryTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchAdvancedExplorationPAsync()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ProjectsResearch.AdvancedExplorationTurnsRequired)
                TurnsToInvest = user.ProjectsResearch.AdvancedExplorationTurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ProjectsResearch.AdvancedExplorationTurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Advanced Exploration.";
                return Page();
            }
            user.ProjectsResearch.AdvancedExplorationTurnsRequired -= TurnsToInvest;
            if (user.ProjectsResearch.AdvancedExplorationTurnsRequired <= 0)
            {
                user.ProjectsResearch.AdvancedExplorationTurnsRequired = 0;
                user.ProjectsResearch.AdvancedExploration = true;
                ResearchMessage = "You have successfully researched Advanced Exploration!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Advanced Exploration . {user.ProjectsResearch.AdvancedExplorationTurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }

        // cluster research
        public async Task<IActionResult> OnPostResearchColonyClusterLevel1Async()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ClusterResearch.ColonyClusterLevel1TurnsRequired)
                TurnsToInvest = user.ClusterResearch.ColonyClusterLevel1TurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ClusterResearch.ColonyClusterLevel1TurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Colony Cluster Level 1.";
                return Page();
            }
            user.ClusterResearch.ColonyClusterLevel1TurnsRequired -= TurnsToInvest;
            if (user.ClusterResearch.ColonyClusterLevel1TurnsRequired <= 0)
            {
                user.ClusterResearch.ColonyClusterLevel1TurnsRequired = 0;
                user.ClusterResearch.ColonyClusterLevel1 = true;
                ResearchMessage = "You have successfully researched Colony Cluster Level 1!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Colony Cluster Level 1 . {user.ClusterResearch.ColonyClusterLevel1TurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchColonyClusterLevel2Async()
        {
            var user2 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user2.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ClusterResearch.ColonyClusterLevel2TurnsRequired)
                TurnsToInvest = user.ClusterResearch.ColonyClusterLevel2TurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ClusterResearch.ColonyClusterLevel2TurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Colony Cluster Level 2.";
                return Page();
            }
            user.ClusterResearch.ColonyClusterLevel2TurnsRequired -= TurnsToInvest;
            if (user.ClusterResearch.ColonyClusterLevel2TurnsRequired <= 0)
            {
                user.ClusterResearch.ColonyClusterLevel2TurnsRequired = 0;
                user.ClusterResearch.ColonyClusterLevel2 = true;
                ResearchMessage = "You have successfully researched Colony Cluster Level 2!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Colony Cluster Level 2 . {user.ClusterResearch.ColonyClusterLevel2TurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostResearchColonyClusterLevel3Async()
        {
            var user1 = await _userManager.GetUserAsync(User);
            user = await _context.Users
               .Include(u => u.TerranResearch)
               .Include(u => u.AMinerResearch)
               .Include(u => u.MarauderResearch)
               .Include(u => u.ViralResearch)
               .Include(u => u.ViralSpecificResearch)
               .Include(u => u.CollectiveResearch)
               .Include(u => u.CollectiveSpecificResearch)
               .Include(u => u.GuardianResearch)
               .Include(u => u.ProjectsResearch)
               .Include(u => u.StrafezResearch)
               .Include(u => u.FClassResearch)
               .Include(u => u.CyrilClassResearch)
               .Include(u => u.EClassResearch)
               .Include(u => u.ClusterResearch).Include(u => u.Turns)
               .FirstOrDefaultAsync(u => u.Id == user1.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (TurnsToInvest > user.Turns.CurrentTurns)
                TurnsToInvest = user.Turns.CurrentTurns;
            if (TurnsToInvest > user.ClusterResearch.ColonyClusterLevel3TurnsRequired)
                TurnsToInvest = user.ClusterResearch.ColonyClusterLevel3TurnsRequired;
            if (TurnsToInvest <= 0)
            {
                StatusMessage = "Please enter a valid number of turns to invest.";
                return Page();
            }
            if (user.ClusterResearch.ColonyClusterLevel3TurnsRequired <= 0)
            {
                StatusMessage = "You have already researched Colony Cluster Level 3.";
                return Page();
            }
            user.ClusterResearch.ColonyClusterLevel3TurnsRequired -= TurnsToInvest;
            if (user.ClusterResearch.ColonyClusterLevel3TurnsRequired <= 0)
            {
                user.ClusterResearch.ColonyClusterLevel3TurnsRequired = 0;
                user.ClusterResearch.ColonyClusterLevel3 = true;
                ResearchMessage = "You have successfully researched Colony Cluster Level 3!";
            }
            else
            {
                ResearchMessage = $"You have invested {TurnsToInvest} turns into researching Colony Cluster Level 3 . {user.ClusterResearch.ColonyClusterLevel3TurnsRequired} turns remaining.";
            }
            var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, TurnsToInvest);
            StatusMessage = $"{turnsMessage.Message}";
            await _context.SaveChangesAsync();
            return Page();
        }
    }
}

