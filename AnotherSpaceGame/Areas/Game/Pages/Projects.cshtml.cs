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
        public bool ServerUWEnabled { get; set; } // This should be set based on your server settings

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
            ServerStats serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            ServerUWEnabled = serverStats.UWEnabled;
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
            ServerStats serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            ServerUWEnabled = serverStats.UWEnabled;
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
            ServerStats serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            ServerUWEnabled = serverStats.UWEnabled;
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
            ServerStats serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            ServerUWEnabled = serverStats.UWEnabled;
            await _context.SaveChangesAsync();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInvestKalZulHektarAsync()
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
            UserProjects.KalZulHektarTurnsRequired =
                System.Math.Max(0, UserProjects.KalZulHektarTurnsRequired - TurnsToInvest);
            UserProjects.KalZulHektarCreditsRequired =
                (int)System.Math.Max(0, UserProjects.KalZulHektarCreditsRequired - CreditsToInvest);

            // Deduct from user
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);

            // If both requirements are met, unlock the lab
            if (UserProjects.KalZulHektarTurnsRequired == 0 && UserProjects.KalZulHektarCreditsRequired == 0)
            {
                UserProjects.KalZulHektar = true;
                UserProjects.KalZulHektarUnlockTimer = DateTime.Now.AddDays(1); // Set the unlock timer to 24 hours from now
                StatusMessage = $"KalZul Loktar unlocked! <hr> {turnMessage.Message}";
            }
            else
            {
                StatusMessage = $"Investment applied. <hr> {turnMessage.Message}";
            }
            ServerStats serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            ServerUWEnabled = serverStats.UWEnabled;
            _context.SaveChanges();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDestroyGalaxyAsync()
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

            // DESTROY GALAXY LOGIC
            var serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            serverStats.UWCompleted = true; 
            serverStats.UWEnabled = false; // Disable UW after completion
            // Alter users data
            var Users = _context.Users.Include(x => x.Commodities).ToList();
            foreach (var u in Users)
            {
                u.Commodities.Credits = (long)Math.Ceiling(u.Commodities.Credits * 0.01); // Reset credits
                u.Commodities.Food = (long)Math.Ceiling(u.Commodities.Food * 0.01); // Reset credits
                u.Commodities.Ore = (long)Math.Ceiling(u.Commodities.Ore * 0.01); // Reset credits
                u.Commodities.RawMaterial = (long)Math.Ceiling(u.Commodities.RawMaterial * 0.01); // Reset credits
                u.Commodities.ConsumerGoods = (long)Math.Ceiling(u.Commodities.ConsumerGoods * 0.01); // Reset credits
                u.Commodities.TerranMetal = (long)Math.Ceiling(u.Commodities.TerranMetal * 0.01); // Reset credits
                u.Commodities.RedCrystal = (long)Math.Ceiling(u.Commodities.RedCrystal * 0.01); // Reset credits
                u.Commodities.WhiteCrystal = (long)Math.Ceiling(u.Commodities.WhiteCrystal * 0.01); // Reset credits
                u.Commodities.Composite = (long)Math.Ceiling(u.Commodities.Composite * 0.01); // Reset credits
                u.Commodities.Rutile = (long)Math.Ceiling(u.Commodities.Rutile * 0.01); // Reset credits
                u.Commodities.StrafezOrganism = (long)Math.Ceiling(u.Commodities.StrafezOrganism * 0.01); // Reset credits
                u.PowerRating = 1500;
                u.DamageProtection = DateTime.Now.AddDays(1); // Reset damage protection
            }
            ServerUWEnabled = serverStats.UWEnabled;
            GalaxyEnd galaxyEnd = new GalaxyEnd
            {
                Name = currentUser.UserName,
                ApplicationUserId = currentUser.Id,
                Description = "",
                PowerRating = currentUser.PowerRating.ToString("N0"),
                CreatedAt = DateTime.Now
            };
            _context.GalaxyEnd.Add(galaxyEnd); // Add the galaxy end record
            _context.SaveChanges();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostInvestKalZulLoktarAsync()
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
            var UserProjectsList = _context.UserProjects.Where(x => x.ApplicationUserId != currentUser.Id).ToList();
            var UserResearchProjectsList = _context.ProjectsResearches.Where(x => x.ApplicationUserId != currentUser.Id).ToList();
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
            UserProjects.KalZulLoktarTurnsRequired =
                System.Math.Max(0, UserProjects.KalZulLoktarTurnsRequired - TurnsToInvest);
            UserProjects.KalZulLoktarCreditsRequired =
                (int)System.Math.Max(0, UserProjects.KalZulLoktarCreditsRequired - CreditsToInvest);

            // Deduct from user
            var turnMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToInvest);

            var serverStats = await _context.ServerStats.FirstOrDefaultAsync();
            // If both requirements are met, unlock the lab
            if (UserProjects.KalZulLoktarTurnsRequired == 0 && UserProjects.KalZulLoktarCreditsRequired == 0)
            {
                UserProjects.KalZulLoktar = true;
                UserProjects.KalZulLoktarUnlockTimer = DateTime.Now.AddDays(2); // Set the unlock timer to 24 hours from now

                // DESTROY GALAXY LOGIC
                serverStats.UWEnabled = true;
                serverStats.UWHolderId = currentUser.Id;
                serverStats.UWHolderName = currentUser.UserName;
                user.DamageProtection = DateTime.Now;
                var artifacts = _context.Artifacts.Where(a => a.ApplicationUserId == currentUser.Id).ToList();
                var bTC = artifacts.FirstOrDefault(a => a.ArtifactName == ArtifactName.BigTimeCapsule);
                if (bTC != null)
                {
                    bTC.Total = 2;
                }
                foreach (var u in UserProjectsList)
                {
                    u.KalZulHektar = false;
                    u.KalZulLoktar = false;
                    u.KalZulHektarCreditsRequired = 800000000; // Reset requirements for other users
                    u.KalZulHektarTurnsRequired = 400; // Reset requirements for other users
                    u.KalZulLoktarCreditsRequired = 400000000; // Reset requirements for other users
                    u.KalZulLoktarTurnsRequired = 200; // Reset requirements for other users
                    u.KalZulLoktarUnlockTimer = DateTime.MinValue; // Reset unlock timer for other users
                    u.KalZulHektarUnlockTimer = DateTime.MinValue; // Reset unlock timer for other users
                    _context.UserProjects.Update(u); // Update other users' projects
                }
                foreach (var ur in UserResearchProjectsList)
                {
                    ur.KalZulArtifact = false;
                    ur.KalZulArtifactTurnsRequired = 90;
                    _context.ProjectsResearches.Update(ur); // Update other users' research projects
                }

                StatusMessage = $"KalZul Loktar timer started! <hr> {turnMessage.Message}";
            }
            else
            {
                StatusMessage = $"Investment applied. <hr> {turnMessage.Message}";
            }
            ServerUWEnabled = serverStats.UWEnabled;
            _context.SaveChanges();
            return Page();
        }
    }
}