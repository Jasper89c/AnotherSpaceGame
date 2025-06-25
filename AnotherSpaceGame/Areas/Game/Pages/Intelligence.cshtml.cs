using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class IntelligenceModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IntelligenceModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public List<ApplicationUser> SearchResults { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            SearchResults = new List<ApplicationUser>();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                // Search by username (case-insensitive, contains)
                SearchResults = await _userManager.Users
                    .Where(u => u.UserName.Contains(SearchTerm))
                    .OrderBy(u => u.UserName)
                    .Take(50)
                    .ToListAsync();
            }

            return Page();
        }
    }
}
