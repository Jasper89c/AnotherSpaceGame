using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class FederationBattleLogsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FederationBattleLogsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<BattleLogs> BattleLogs { get; set; } = new List<BattleLogs>();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            PageNumber = pageNumber ?? 1;

            // Get current user and their federation
            user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user?.FederationId == null)
            {
                BattleLogs = new List<BattleLogs>();
                TotalPages = 1;
                return Page();
            }

            // Get all user IDs in the same federation
            var federationUserIds = await _context.Users
                .Where(u => u.FederationId == user.FederationId)
                .Select(u => u.Id)
                .ToListAsync();

            // Get total count for pagination
            var totalLogs = await _context.Battlelogs
                .Where(b => federationUserIds.Contains(b.ApplicationUserId))
                .CountAsync();

            TotalPages = (int)System.Math.Ceiling(totalLogs / (double)PageSize);

            // Get logs for current page
            BattleLogs = await _context.Battlelogs
                .Where(b => federationUserIds.Contains(b.ApplicationUserId))
                .OrderByDescending(b => b.DateAndTime)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Include(b => b.ApplicationUser)
                .ToListAsync();

            return Page();
        }
    }
}
