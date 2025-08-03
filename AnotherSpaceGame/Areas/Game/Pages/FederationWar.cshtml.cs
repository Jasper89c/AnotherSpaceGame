using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationWarModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationWarModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public int TargetFederationId { get; set; }

        public string? FeedbackMessage { get; set; }

        public List<Federations> AvailableFederations { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user?.Federation != null)
            {
                // Get all federations ordered by TotalPowerating descending
                var federations = await _context.Federations
                    .OrderByDescending(f => f.TotalPlanets)
                    .ToListAsync();

                // Find the user's federation rank (0-based)
                int userFederationIndex = federations.FindIndex(f => f.Id == user.Federation.Id);

                // Only show federations within 5 ranks above or below (excluding self)
                AvailableFederations = federations
                    .Select((f, idx) => new { Federation = f, Rank = idx })
                    .Where(x => x.Federation.Id != user.Federation.Id &&
                                Math.Abs(x.Rank - userFederationIndex) <= 5)
                    .Select(x => x.Federation)
                    .ToList();
            }
            else
            {
                AvailableFederations = await _context.Federations.ToListAsync();
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeclareWarAsync(int targetFederationId)
        {
            // Get current user's federation
            var user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user?.Federation == null)
            {
                FeedbackMessage = "You are not in a federation.";
                return await ReloadFederationsAndReturnPage(user);
            }

            // Only leader can declare war
            if (user.Federation.FederationLeaderId != user.Id)
            {
                FeedbackMessage = "Only the federation leader can declare war.";
                return await ReloadFederationsAndReturnPage(user);
            }

            // Prevent declaring war on self
            if (user.Federation.Id == targetFederationId)
            {
                FeedbackMessage = "You cannot declare war on your own federation.";
                return await ReloadFederationsAndReturnPage(user);
            }

            // Check for existing outgoing war (regardless of target)
            var existingWar = await _context.FederationWars
                .FirstOrDefaultAsync(w => w.AttackerFederationId == user.Federation.Id);

            if (existingWar != null)
            {
                // Override the old war
                existingWar.DefenderFederationId = targetFederationId;
                existingWar.DeclaredAt = DateTime.Now;
                FeedbackMessage = "Previous war overridden. New war declared successfully!";
            }
            else
            {
                // Create new war
                var war = new FederationWar
                {
                    AttackerFederationId = user.Federation.Id,
                    DefenderFederationId = targetFederationId,
                    DeclaredAt = DateTime.Now
                };
                _context.FederationWars.Add(war);
                FeedbackMessage = "War declared successfully!";
            }

            await _context.SaveChangesAsync();
            return await ReloadFederationsAndReturnPage(user);
        }

        // Helper to reload federations for the form and return Page()
        private async Task<IActionResult> ReloadFederationsAndReturnPage(ApplicationUser? user)
        {
            if (user?.Federation != null)
            {
                var federations = await _context.Federations
                    .OrderByDescending(f => f.TotalPlanets)
                    .ToListAsync();

                int userFederationIndex = federations.FindIndex(f => f.Id == user.Federation.Id);

                AvailableFederations = federations
                    .Select((f, idx) => new { Federation = f, Rank = idx })
                    .Where(x => x.Federation.Id != user.Federation.Id &&
                                Math.Abs(x.Rank - userFederationIndex) <= 5)
                    .Select(x => x.Federation)
                    .ToList();
            }
            else
            {
                AvailableFederations = await _context.Federations.ToListAsync();
            }
            return Page();
        }
    }
}
