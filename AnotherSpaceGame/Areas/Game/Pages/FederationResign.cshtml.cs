using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationResignModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationResignModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string? FeedbackMessage { get; set; }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var userName = user.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                FeedbackMessage = "You must be logged in to resign from a federation.";
                return Page();
            }

                user = await _context.Users
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
                if(user.Federation.FederationMembers.Count == 0)
                {
                    // If no members left, remove the federation
                    _context.Federations.Remove(user.Federation);
                }
                else
                {
                    var federation = await _context.Federations
                        .Include(f => f.FederationMembers)
                        .FirstOrDefaultAsync(f => f.Id == user.FederationId);
                    var federationMembers = federation?.FederationMembers ?? new List<ApplicationUser>();
                    var federationMembersOrdered = federationMembers.OrderByDescending(m => m.PowerRating).ToList();
                    // Update the federation with the remaining members
                    user.Federation.TotalMembers = federationMembersOrdered.Count;
                    user.Federation.TotalPlanets = federationMembersOrdered.Sum(m => m.Planets.Count);
                    user.Federation.TotalPowerating = federationMembersOrdered.Sum(m => m.PowerRating);
                    _context.Federations.Update(user.Federation);
                }
                if(user.Federation.FederationLeaderId == user.Id)
                {
                    // If the resigning user is the leader, set a new leader if available
                    var newLeader = user.Federation.FederationMembers.OrderByDescending(m => m.PowerRating).FirstOrDefault();
                    if (newLeader != null)
                    {
                        user.Federation.FederationLeaderId = newLeader.Id;
                        user.Federation.FederationLeader = newLeader;
                        _context.Federations.Update(user.Federation);
                    }
                    else
                    {
                        // If no members left, remove the federation
                        _context.Federations.Remove(user.Federation);
                    }
                }
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
