using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Threading.Tasks;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationResignModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationResignModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string? FeedbackMessage { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                FeedbackMessage = "You must be logged in to resign from a federation.";
                return Page();
            }

            var user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                FeedbackMessage = "User not found.";
                return Page();
            }

            if (user.FederationId == null)
            {
                FeedbackMessage = "You are not a member of any federation.";
                return Page();
            }

            var federationName = user.Federation?.FederationName ?? "your federation";

            // Remove user from federation
            user.FederationId = null;
            _context.Users.Update(user);

            // Optionally, remove from FederationMembers navigation property
            if (user.Federation != null && user.Federation.FederationMembers.Contains(user))
            {
                user.Federation.FederationMembers.Remove(user);
            }

            // Create an important event
            _context.ImportantEvents.Add(new ImportantEvents
            {
                ApplicationUserId = user.Id,
                DateAndTime = DateTime.UtcNow,
                Text = $"You have resigned from {federationName}.",
                ImportantEventTypes = ImportantEventTypes.Misc
            });

            await _context.SaveChangesAsync();

            FeedbackMessage = $"You have successfully resigned from {federationName}.";
            return Page();
        }
    }
}
