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

            // Optionally, remove from FederationMembers navigation property
            if (user.Federation != null && user.Federation.FederationMembers.Contains(user))
            {
                var federation = await _context.Federations
                        .Include(f => f.FederationMembers)
                        .FirstOrDefaultAsync(f => f.Id == user.FederationId);
                federation.FederationMembers.Remove(user);
                if(federation.FederationMembers.Count == 0)
                {
                    // If no members left, remove the federation
                    _context.Federations.Remove(user.Federation);
                }
                else
                {                    
                    var federationMembers = federation?.FederationMembers ?? new List<ApplicationUser>();
                    var federationMembersOrdered = federationMembers.OrderByDescending(m => m.PowerRating).ToList();
                    // Update the federation with the remaining members
                    federation.TotalMembers -= 1;
                    federation.TotalPlanets = federationMembersOrdered.Sum(m => m.TotalPlanets);
                    federation.TotalPowerating = federationMembersOrdered.Sum(m => m.PowerRating);
                    _context.Federations.Update(user.Federation);
                    
                }
                if(federation.FederationLeaderId == user.Id)
                {
                    // If the resigning user is the leader, set a new leader if available
                    var newLeader = federation.FederationMembers.OrderByDescending(m => m.PowerRating).FirstOrDefault();
                    if (newLeader != null)
                    {
                        user.Federation.FederationLeaderId = newLeader.Id;
                        user.Federation.FederationLeader = newLeader;
                        _context.Federations.Update(user.Federation);
                    }
                    else
                    {
                        // Remove related FederationWars where this federation is a defender
                        var warsAsDefender = _context.FederationWars
                            .Where(w => w.DefenderFederationId == federation.Id);
                        _context.FederationWars.RemoveRange(warsAsDefender);

                        // (Optional) Remove related FederationWars where this federation is an attacker
                        var warsAsAttacker = _context.FederationWars
                            .Where(w => w.AttackerFederationId == federation.Id);
                        _context.FederationWars.RemoveRange(warsAsAttacker);

                        // Remove related FederationApplications where this federation is referenced
                        var applications = _context.FederationApplications
                            .Where(a => a.FederationId == federation.Id);
                        _context.FederationApplications.RemoveRange(applications);

                        // Remove related FederationMessages where this federation is referenced
                        var messages = _context.FederationMessages
                            .Where(m => m.FederationId == federation.Id);
                        _context.FederationMessages.RemoveRange(messages);

                        // If no members left, remove the federation
                        _context.Federations.Remove(federation);
                    }
                }
            }
            // Remove user from federation
            user.FederationId = null;
            user.Federation = null;
            user.FederationApplication = null; // Clear the federation application if exists

            _context.Users.Update(user);
            // Create an important event
            _context.ImportantEvents.Add(new ImportantEvents
            {
                ApplicationUserId = user.Id,
                DateAndTime = DateTime.Now,
                Text = $"You have resigned from {federationName}.",
                ImportantEventTypes = ImportantEventTypes.Misc
            });

            await _context.SaveChangesAsync();

            FeedbackMessage = $"You have successfully resigned from {federationName}.";
            return Page();
        }
    }
}
