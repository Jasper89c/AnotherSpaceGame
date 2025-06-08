using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Threading.Tasks;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationCreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FederationName { get; set; }

        [BindProperty]
        public string FederationDescription { get; set; }

        public string? FeedbackMessage { get; set; }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users
                .Include(u => u.Commodities)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                FeedbackMessage = "User not found.";
                return Page();
            }

            if (user.FederationId != null)
            {
                FeedbackMessage = "You are already a member of a federation.";
                return Page();
            }

            if (user.PowerRating < 250000)
            {
                FeedbackMessage = "You need at least 250,000 Power Rating to create a federation.";
                return Page();
            }

            if (user.Commodities == null || user.Commodities.Credits < 100_000_000)
            {
                FeedbackMessage = "You need at least 100,000,000 credits to create a federation.";
                return Page();
            }

            // Deduct credits
            user.Commodities.Credits -= 100_000_000;

            // Create federation
            var federation = new Federations
            {
                FederationName = FederationName,
                FederationDescription = FederationDescription,
                FederationLeader = user,
                FederationLeaderId = user.Id,
                FederationMembers = new System.Collections.Generic.List<ApplicationUser> { user },
                CreatedAt = DateTime.UtcNow,
                TotalMembers = 1,
                MaximumMembers = 21,
                TotalPowerating = user.PowerRating,
                TotalPlanets = user.TotalPlanets
            };

            _context.Federations.Add(federation);
            await _context.SaveChangesAsync();

            // Set user's federation
            user.FederationId = federation.Id;
            _context.Users.Update(user);

            // Create important event
            var importantEvent = new ImportantEvents
            {
                ApplicationUserId = user.Id,
                DateAndTime = DateTime.UtcNow,
                ImportantEventTypes = ImportantEventTypes.Misc,
                Text = $"You have created the federation '{FederationName}'."
            };
            _context.ImportantEvents.Add(importantEvent);

            await _context.SaveChangesAsync();

            FeedbackMessage = "Federation created successfully!";
            return Page();
        }
    }
}
