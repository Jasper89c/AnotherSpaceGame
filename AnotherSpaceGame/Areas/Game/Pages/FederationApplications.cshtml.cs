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
    public class FederationApplicationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationApplicationsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Federations Federation { get; set; }
        public List<FederationApplication> Applicants { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string FeedbackMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .Include(f => f.FederationApplicants)
                    .ThenInclude(a => a.ApplicationUser)
                .FirstOrDefaultAsync(f => f.Id == id);

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

            Federation = await _context.Federations.Include(s => s.FederationMembers)
                .FirstOrDefaultAsync(f => f.Id == application.FederationId);

            if (Federation == null)
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
            Federation.FederationMembers.Add(user);

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
