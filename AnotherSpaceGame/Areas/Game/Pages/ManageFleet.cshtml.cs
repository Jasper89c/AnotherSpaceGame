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

        public List<Fleet> UserFleets { get; set; } = new();
        public List<ManageFleet> manageFleets { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> RemoveShips { get; set; } = new();
        public Faction Faction { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
            Faction = user.Faction; // Assuming Faction is a property of ApplicationUser
            UserFleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == currentUser.Id)
                .OrderByDescending(f => f.TotalPowerRating)
                .ToListAsync();

            manageFleets = new List<ManageFleet>();

            foreach (var fleet in UserFleets)
            {
                manageFleets.Add(new ManageFleet
                {
                    Id = fleet.Id,
                    ApplicationUserId = fleet.ApplicationUserId,
                    ShipId = fleet.ShipId,
                    TotalShips = fleet.TotalShips,
                    TotalPowerRating = fleet.TotalPowerRating,
                    TotalUpkeep = fleet.TotalUpkeep,
                    Ship = await _context.Ships.FindAsync(fleet.ShipId)
                });
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
            Faction = user.Faction; // Assuming Faction is a property of ApplicationUser
            var fleets = await _context.Fleets
                .Where(f => f.ApplicationUserId == currentUser.Id)
                .ToListAsync();

            manageFleets = new List<ManageFleet>();

            foreach (var fleet in fleets)
            {
                manageFleets.Add(new ManageFleet
                {
                    Id = fleet.Id,
                    ApplicationUserId = fleet.ApplicationUserId,
                    ShipId = fleet.ShipId,
                    TotalShips = fleet.TotalShips,
                    TotalPowerRating = fleet.TotalPowerRating,
                    TotalUpkeep = fleet.TotalUpkeep,
                    Ship = await _context.Ships.FindAsync(fleet.ShipId)
                });
            }

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

    public class ManageFleet
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ShipId { get; set; }
        public int TotalShips { get; set; }
        public int TotalPowerRating { get; set; }
        public int TotalUpkeep { get; set; }
        public Ships Ship { get; set; } // Navigation property to Ships model
    }
}