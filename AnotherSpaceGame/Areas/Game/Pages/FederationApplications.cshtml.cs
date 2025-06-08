using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationApplicationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FederationApplicationsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Federations Federation { get; set; }
        public List<FederationApplication> Applicants { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int FederationId { get; set; }

        [BindProperty]
        public string FeedbackMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int federationId)
        {
            FederationId = federationId;
            Federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .Include(f => f.FederationApplicants)
                    .ThenInclude(a => a.ApplicationUser)
                .FirstOrDefaultAsync(f => f.Id == FederationId);

            if (Federation == null)
                return NotFound();

            // Only leader can view
            if (Federation.FederationLeaderId != User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
                return Forbid();

            Applicants = Federation.FederationApplicants.ToList();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostApproveAsync(int applicationId)
        {
            var application = await _context.FederationApplications
                .Include(a => a.ApplicationUser)
                .Include(a => a.Federation)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return NotFound();

            // Only leader can approve
            if (application.Federation.FederationLeaderId != Federation.FederationLeaderId)
                return Forbid();

            // Add user to federation
            var user = application.ApplicationUser;
            user.FederationId = application.FederationId;
            user.Federation = application.Federation;
            _context.Users.Update(user);

            // Optionally add to FederationMembers collection
            application.Federation.FederationMembers.Add(user);

            // Create important event
            _context.ImportantEvents.Add(new ImportantEvents
            {
                ApplicationUserId = user.Id,
                DateAndTime = DateTime.UtcNow,
                Text = $"Your application to join '{application.Federation.FederationName}' was approved.",
                ImportantEventTypes = ImportantEventTypes.Misc
            });

            // Remove application
            _context.FederationApplications.Remove(application);

            await _context.SaveChangesAsync();
            FeedbackMessage = "Application approved and user added to federation.";
            return RedirectToPage(new { federationId = application.FederationId });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDenyAsync(int applicationId)
        {
            var application = await _context.FederationApplications
                .Include(a => a.ApplicationUser)
                .Include(a => a.Federation)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return NotFound();

            // Only leader can deny
            if (application.Federation.FederationLeaderId != Federation.FederationLeaderId)
                return Forbid();

            // Create important event
            _context.ImportantEvents.Add(new ImportantEvents
            {
                ApplicationUserId = application.ApplicationUserId,
                DateAndTime = DateTime.UtcNow,
                Text = $"Your application to join '{application.Federation.FederationName}' was denied.",
                ImportantEventTypes = ImportantEventTypes.Misc
            });

            // Remove application
            _context.FederationApplications.Remove(application);

            await _context.SaveChangesAsync();
            FeedbackMessage = "Application denied and removed.";
            return RedirectToPage(new { federationId = application.FederationId });
        }
    }
}
