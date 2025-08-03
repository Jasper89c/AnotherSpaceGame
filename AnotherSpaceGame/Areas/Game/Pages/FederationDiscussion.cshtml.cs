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
    public class FederationDiscussionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationDiscussionModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Federations? Federation { get; set; }
        public List<FederationMessages> Messages { get; set; } = new();

        [BindProperty]
        public string NewMessage { get; set; }

        public string? FeedbackMessage { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            PageNumber = pageNumber ?? 1;

            var user = await _userManager.GetUserAsync(User);
            user = await _context.Users
                .Include(u => u.Commodities)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            if (user?.FederationId == null)
            {
                FeedbackMessage = "You are not a member of any federation.";
                return Page();
            }

            Federation = await _context.Federations
                .Include(f => f.FederationLeader)
                .FirstOrDefaultAsync(f => f.Id == user.FederationId);

            // Get total message count for pagination
            var totalMessages = await _context.FederationMessages
                .Where(m => m.FederationId == user.FederationId)
                .CountAsync();

            TotalPages = (int)Math.Ceiling(totalMessages / (double)PageSize);

            Messages = await _context.FederationMessages
                .Where(m => m.FederationId == user.FederationId)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.SentAt)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user?.FederationId == null)
            {
                FeedbackMessage = "You are not a member of any federation.";
                return await OnGetAsync(PageNumber); // Pass the current PageNumber to fix CS7036
            }

            if (string.IsNullOrWhiteSpace(NewMessage))
            {
                FeedbackMessage = "Message cannot be empty.";
                return await OnGetAsync(PageNumber); // Pass the current PageNumber to fix CS7036
            }

            var message = new FederationMessages
            {
                SenderId = user.Id,
                MessageText = NewMessage.Trim(),
                SentAt = DateTime.Now,
                FederationId = user.FederationId.Value // Set the federation ID
            };

            _context.FederationMessages.Add(message);
            await _context.SaveChangesAsync();

            NewMessage = string.Empty;

            // Redirect to first page after posting
            return RedirectToPage(new { pageNumber = 1 });
        }
    }
}
