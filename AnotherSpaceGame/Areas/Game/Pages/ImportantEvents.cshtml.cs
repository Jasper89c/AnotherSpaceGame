using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    [Area("Game")]
    [Route("game/important-events")]
    [Route("game/important-events/{*slug}")]
    [Authorize]
    public class ImportantEventsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ImportantEventsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<ImportantEvents> Events { get; set; } = new List<ImportantEvents>();

        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (user != null)
            {
                var query = _context.ImportantEvents
                    .Where(e => e.ApplicationUserId == user.Id)
                    .OrderByDescending(e => e.DateAndTime);

                int totalEvents = await Task.Run(() => query.Count());
                TotalPages = (int)System.Math.Ceiling(totalEvents / (double)PageSize);

                Events = await Task.Run(() =>
                    query
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToList()
                );
            }
            else
            {
                Events = new List<ImportantEvents>();
                TotalPages = 1;
            }
            return Page();
        }
    }
}
