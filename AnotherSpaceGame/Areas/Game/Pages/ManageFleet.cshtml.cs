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
    public class ManageFleetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ManageFleetModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Fleet> UserFleets { get; set; }

        [BindProperty]
        public Dictionary<int, int> RemoveShips { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            UserFleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == user.Id)
                .OrderByDescending(f => f.TotalPowerRating)
                .ToListAsync();

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var fleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == user.Id)
                .ToListAsync();

            foreach (var fleet in fleets)
            {
                if (RemoveShips.TryGetValue(fleet.Id, out int toRemove) && toRemove > 0)
                {
                    fleet.TotalShips = (fleet.TotalShips - toRemove) < 0 ? 0 : fleet.TotalShips - toRemove;
                    // Optionally, remove fleet if ships reach 0
                    if (fleet.TotalShips <= 0) _context.Fleets.Remove(fleet);
                }
                var ship = await _context.Ships.FindAsync(fleet.ShipId);
                if (ship != null)
                {
                    fleet.TotalPowerRating = fleet.TotalShips * ship.PowerRating;
                    fleet.TotalUpkeep = fleet.TotalShips * ship.Upkeep;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
