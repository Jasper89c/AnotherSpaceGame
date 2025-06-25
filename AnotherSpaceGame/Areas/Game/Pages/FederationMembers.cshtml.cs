using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationMembersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationMembersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Federations? Federation { get; set; }

        [BindProperty]
        public string? FeedbackMessage { get; set; }
        public FederationWar? OutgoingWar { get; set; }
        public List<FederationWar> IncomingWars { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            Id = id;
            Federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .Include(f => f.FederationMembers)
                .FirstOrDefaultAsync(f => f.Id == id);

            // Load outgoing war (this federation as attacker)
            OutgoingWar = await _context.FederationWars
                .Include(w => w.DefenderFederation)
                .FirstOrDefaultAsync(w => w.AttackerFederationId == id);

            // Load incoming wars (this federation as defender)
            IncomingWars = await _context.FederationWars
                .Include(w => w.AttackerFederation)
                .Where(w => w.DefenderFederationId == id)
                .ToListAsync();

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostKickAsync(int id, string memberId)
        {
            var federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .Include(f => f.FederationMembers)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (federation == null)
            {
                return NotFound();
            }

            // Only the leader can kick
            if (User.Identity?.Name != federation.FederationLeader?.UserName)
            {
                return Forbid();
            }

            var member = federation.FederationMembers.FirstOrDefault(m => m.UserName == memberId);

            // Prevent leader from kicking themselves
            if (member == null || member.UserName == federation.FederationLeader?.UserName)
            {
                return RedirectToPage(new { id });
            }
           
            federation.FederationMembers.Remove(member);
            member.FederationId = null; // Clear the federation ID from the member
            _context.Users.Update(member); // Corrected to use the `Users` DbSet for ApplicationUser
            _context.Federations.Update(federation);

            // Create an ImportantEvent for the kicked user
            var importantEvent = new ImportantEvents
            {
                ApplicationUserId = member.Id,
                DateAndTime = DateTime.UtcNow,
                ImportantEventTypes = ImportantEventTypes.Misc,
                Text = $"{federation.FederationLeader?.UserName} has kicked you from the federation {federation.FederationName}."
            };
            _context.ImportantEvents.Add(importantEvent);

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}