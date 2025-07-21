using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService; // Add this field for the turn service

        public ProjectsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public UserProjects UserProjects { get; set; }
        public ProjectsResearch ProjectsResearch { get; set; }
        public ClusterResearch ClusterResearch { get; set; }
        public ViralSpecificResearch ViralSpecificResearch { get; set; }
        public CollectiveSpecificResearch CollectiveSpecificResearch { get; set; }
        [BindProperty]
        public int TurnsToInvest { get; set; }
        [BindProperty]
        public long CreditsToInvest { get; set; }
        public string StatusMessage { get; set; }
        public Faction Faction { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .Include(u => u.Turns) // Assuming Turns is a navigation property of ApplicationUser
                .Include(u => u.Commodities) // Assuming Commodities is a navigation property of ApplicationUser
                .FirstOrDefault(u => u.Id == user.Id);
            UserProjects = _context.UserProjects.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ClusterResearch = _context.ClusterResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ProjectsResearch = _context.ProjectsResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            if (currentUser.Faction == Faction.Viral)
            {
                ViralSpecificResearch = _context.ViralSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                ViralSpecificResearch = null; // or handle accordingly if not Viral
            }
            if(user.Faction == Faction.Collective)
            {
                CollectiveSpecificResearch = _context.CollectiveSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                CollectiveSpecificResearch = null; // or handle accordingly if not Collective
            }

            Faction = currentUser.Faction; // Assuming Faction is a property of ApplicationUser

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInvestCapsuleLabAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .Include(u => u.Turns) // Assuming Turns is a navigation property of ApplicationUser
                .Include(u => u.Commodities) // Assuming Commodities is a navigation property of ApplicationUser
                .FirstOrDefault(u => u.Id == user.Id);
            Faction = currentUser.Faction; // Assuming Faction is a property of ApplicationUser
            UserProjects = _context.UserProjects.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ClusterResearch = _context.ClusterResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ProjectsResearch = _context.ProjectsResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            if (currentUser.Faction == Faction.Viral)
            {
                ViralSpecificResearch = _context.ViralSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                ViralSpecificResearch = null; // or handle accordingly if not Viral
            }
            if (currentUser.Faction == Faction.Collective)
            {
                CollectiveSpecificResearch = _context.CollectiveSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                CollectiveSpecificResearch = null; // or handle accordingly if not Collective
            }
            if (UserProjects == null)
            {
                StatusMessage = "Project data not found.";
                return Page();
            }
            if(TurnsToInvest > UserProjects.CapsuleLabTurnsRequired)
            {
                TurnsToInvest = UserProjects.CapsuleLabTurnsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest > currentUser.Turns.CurrentTurns)
            {
                TurnsToInvest = currentUser.Turns.CurrentTurns; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > currentUser.Commodities.Credits)
            {
                CreditsToInvest = currentUser.Commodities.Credits; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > UserProjects.CapsuleLabCreditsRequired)
            {
                CreditsToInvest = UserProjects.CapsuleLabCreditsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest <= 0 && CreditsToInvest <= 0)
            {
                StatusMessage = "No investment made. Please enter valid amounts.";
                return Page();
            }
            // Assume ApplicationUser has properties for available turns and credits
            int availableTurns = currentUser.Turns.CurrentTurns; // Replace with your actual property
            long availableCredits = currentUser.Commodities.Credits; // Replace with your actual property

            if (TurnsToInvest > availableTurns || CreditsToInvest > availableCredits)
            {
                StatusMessage = "Not enough turns or credits.";
                return Page();
            }

            // Deduct from required, but not below zero
            UserProjects.CapsuleLabTurnsRequired =
                System.Math.Max(0, UserProjects.CapsuleLabTurnsRequired - TurnsToInvest);
            UserProjects.CapsuleLabCreditsRequired =
                (int)System.Math.Max(0, UserProjects.CapsuleLabCreditsRequired - CreditsToInvest);

            // Deduct from user
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);
            currentUser.Commodities.Credits -= CreditsToInvest;
            // If both requirements are met, unlock the lab
            if (UserProjects.CapsuleLabTurnsRequired == 0 && UserProjects.CapsuleLabCreditsRequired == 0)
            {
                UserProjects.CapsuleLab = true;
                StatusMessage = $"Capsule Lab unlocked! <hr /> {turnMessage.Message}";
            }
            else
            {
                StatusMessage = $"Investment applied. <hr /> {turnMessage.Message}";
            }

            await _context.SaveChangesAsync();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInvestITechAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");
            var currentUser = _context.Users
                .Include(u => u.Turns) // Assuming Turns is a navigation property of ApplicationUser
                .Include(u => u.Commodities) // Assuming Commodities is a navigation property of ApplicationUser
                .FirstOrDefault(u => u.Id == user.Id);
            Faction = currentUser.Faction; // Assuming Faction is a property of ApplicationUser
            UserProjects = _context.UserProjects.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ClusterResearch = _context.ClusterResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ProjectsResearch = _context.ProjectsResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            if (currentUser.Faction == Faction.Viral)
            {
                ViralSpecificResearch = _context.ViralSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                ViralSpecificResearch = null; // or handle accordingly if not Viral
            }
            if (user.Faction == Faction.Collective)
            {
                CollectiveSpecificResearch = _context.CollectiveSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                CollectiveSpecificResearch = null; // or handle accordingly if not Collective
            }
            if (UserProjects == null)
            {
                StatusMessage = "Project data not found.";
                return Page();
            }
            if (TurnsToInvest > UserProjects.ItechTurnsRequired)
            {
                TurnsToInvest = UserProjects.ItechTurnsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest > currentUser.Turns.CurrentTurns)
            {
                TurnsToInvest = currentUser.Turns.CurrentTurns; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > currentUser.Commodities.Credits)
            {
                CreditsToInvest = currentUser.Commodities.Credits; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > UserProjects.ItechCreditsRequired)
            {
                CreditsToInvest = UserProjects.ItechCreditsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest <= 0 && CreditsToInvest <= 0)
            {
                StatusMessage = "No investment made. Please enter valid amounts.";
                return Page();
            }
            // Assume ApplicationUser has properties for available turns and credits
            int availableTurns = currentUser.Turns.CurrentTurns; // Replace with your actual property
            long availableCredits = currentUser.Commodities.Credits; // Replace with your actual property

            if (TurnsToInvest > availableTurns || CreditsToInvest > availableCredits)
            {
                StatusMessage = "Not enough turns or credits.";
                return Page();
            }

            // Deduct from required, but not below zero
            UserProjects.ItechTurnsRequired =
                System.Math.Max(0, UserProjects.ItechTurnsRequired - TurnsToInvest);
            UserProjects.ItechCreditsRequired =
                (int)System.Math.Max(0, UserProjects.ItechCreditsRequired - CreditsToInvest);

            // Deduct from user
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);

            // If both requirements are met, unlock the lab
            if (UserProjects.ItechTurnsRequired == 0 && UserProjects.ItechCreditsRequired == 0)
            {
                UserProjects.Itech = true;
                StatusMessage = $"Itech unlocked! <hr> {turnMessage.Message}";
            }
            else
            {
                StatusMessage = $"Investment applied. <hr> {turnMessage.Message}";
            }

            await _context.SaveChangesAsync();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInvestUnreverseEngineeringAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");
            var currentUser = _context.Users
                .Include(u => u.Turns) // Assuming Turns is a navigation property of ApplicationUser
                .Include(u => u.Commodities) // Assuming Commodities is a navigation property of ApplicationUser
                .FirstOrDefault(u => u.Id == user.Id);
            Faction = currentUser.Faction; // Assuming Faction is a property of ApplicationUser
            UserProjects = _context.UserProjects.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ClusterResearch = _context.ClusterResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            ProjectsResearch = _context.ProjectsResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            if (user.Faction == Faction.Viral)
            {
                ViralSpecificResearch = _context.ViralSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                ViralSpecificResearch = null; // or handle accordingly if not Viral
            }
            if (currentUser.Faction == Faction.Collective)
            {
                CollectiveSpecificResearch = _context.CollectiveSpecificResearches.FirstOrDefault(p => p.ApplicationUserId == currentUser.Id);
            }
            else
            {
                CollectiveSpecificResearch = null; // or handle accordingly if not Collective
            }
            if (UserProjects == null)
            {
                StatusMessage = "Project data not found.";
                return Page();
            }
            if (TurnsToInvest > UserProjects.UnreverseEngineeringTurnsRequired)
            {
                TurnsToInvest = UserProjects.UnreverseEngineeringTurnsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest > currentUser.Turns.CurrentTurns)
            {
                TurnsToInvest = currentUser.Turns.CurrentTurns; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > currentUser.Commodities.Credits)
            {
                CreditsToInvest = currentUser.Commodities.Credits; // Ensure we don't invest more than available
            }
            if (CreditsToInvest > UserProjects.UnreverseEngineeringCreditsRequired)
            {
                CreditsToInvest = UserProjects.UnreverseEngineeringCreditsRequired; // Ensure we don't invest more than required
            }
            if (TurnsToInvest <= 0 && CreditsToInvest <= 0)
            {
                StatusMessage = "No investment made. Please enter valid amounts.";
                return Page();
            }
            // Assume ApplicationUser has properties for available turns and credits
            int availableTurns = currentUser.Turns.CurrentTurns; // Replace with your actual property
            long availableCredits = currentUser.Commodities.Credits; // Replace with your actual property

            if (TurnsToInvest > availableTurns || CreditsToInvest > availableCredits)
            {
                StatusMessage = "Not enough turns or credits.";
                return Page();
            }

            // Deduct from required, but not below zero
            UserProjects.UnreverseEngineeringTurnsRequired =
                System.Math.Max(0, UserProjects.UnreverseEngineeringTurnsRequired - TurnsToInvest);
            UserProjects.UnreverseEngineeringCreditsRequired =
                (int)System.Math.Max(0, UserProjects.UnreverseEngineeringCreditsRequired - CreditsToInvest);

            // Deduct from user
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);

            // If both requirements are met, unlock the lab
            if (UserProjects.UnreverseEngineeringTurnsRequired == 0 && UserProjects.UnreverseEngineeringCreditsRequired == 0)
            {
                UserProjects.UnreverseEngineering = true;
                StatusMessage = $"Unreverse Engineering unlocked! <hr> {turnMessage.Message}";
            }
            else
            {
                StatusMessage = $"Investment applied. <hr> {turnMessage.Message}";
            }

            await _context.SaveChangesAsync();
            return Page();
        }
    }
}