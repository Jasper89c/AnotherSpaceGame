using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationLookUpModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationLookUpModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Federations? Federation { get; set; }
        public string? FeedbackMessage { get; set; }

        public FederationWar? OutgoingWar { get; set; }
        public List<FederationWar> IncomingWars { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Id = id;
            Federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .Include(f => f.FederationMembers)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Federation != null)
            {
                // Outgoing war (this federation as attacker)
                OutgoingWar = await _context.FederationWars
                    .Include(w => w.DefenderFederation)
                    .FirstOrDefaultAsync(w => w.AttackerFederationId == id);

                // Incoming wars (this federation as defender)
                IncomingWars = await _context.FederationWars
                    .Include(w => w.AttackerFederation)
                    .Where(w => w.DefenderFederationId == id)
                    .ToListAsync();
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostApplyAsync()
        {
            // Load federation
            Federation = await _context.Federations
                .Where(f => f.Id == Id)
                .Select(f => new Federations
                {
                    Id = f.Id,
                    FederationName = f.FederationName,
                    FederationLeader = f.FederationLeader,
                    TotalPowerating = f.TotalPowerating,
                    TotalPlanets = f.TotalPlanets,
                    TotalMembers = f.TotalMembers,
                    CreatedAt = f.CreatedAt,
                    FederationDescription = f.FederationDescription,
                    FederationMembers = f.FederationMembers
                })
                .FirstOrDefaultAsync();

            if (Federation == null)
            {
                FeedbackMessage = "Federation not found.";
                return Page();
            }

            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                FeedbackMessage = "You must be logged in to apply.";
                return Page();
            }

            // Get user
            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                FeedbackMessage = "User not found.";
                return Page();
            }

            // Check if user is already a member of any federation
            if (user.FederationId != null)
            {
                FeedbackMessage = "You are already a member of a federation and cannot apply to another.";
                return Page();
            }

            // Check for existing application and overwrite if exists
            var existingApplication = await _context.FederationApplications
                .FirstOrDefaultAsync(a => a.ApplicationUserId == user.Id);

            if (existingApplication != null)
            {
                existingApplication.FederationId = Id;
                existingApplication.AppliedAt = DateTime.UtcNow;
                _context.FederationApplications.Update(existingApplication);
                FeedbackMessage = "Your previous application has been updated to this federation!";
            }
            else
            {
                var application = new FederationApplication
                {
                    FederationId = Id,
                    ApplicationUserId = user.Id,
                    AppliedAt = DateTime.UtcNow
                };
                _context.FederationApplications.Add(application);
                FeedbackMessage = "Your application has been submitted!";
            }

            // Create an important event for the user
            var eventMessage = $"You applied to join the federation '{Federation.FederationName}'.";
            var importantEvent = new ImportantEvents
            {
                ApplicationUserId = user.Id,
                ImportantEventTypes = ImportantEventTypes.Misc
            };
            if (importantEvent.GetType().GetProperty("Message") != null)
                importantEvent.GetType().GetProperty("Message")?.SetValue(importantEvent, eventMessage);
            if (importantEvent.GetType().GetProperty("Description") != null)
                importantEvent.GetType().GetProperty("Description")?.SetValue(importantEvent, eventMessage);

            _context.ImportantEvents.Add(importantEvent);

            await _context.SaveChangesAsync();
            return Page();
        }
    }
}
