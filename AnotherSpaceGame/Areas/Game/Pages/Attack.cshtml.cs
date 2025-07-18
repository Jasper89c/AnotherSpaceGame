using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class AttackModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AttackModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<Fleet> TopFleets { get; set; } = new();
        public List<UserShipFleet> UserShipFleets { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public string? UserName { get; set; }
        [BindProperty(SupportsGet = true)]
        public AttackType SelectedAttackType { get; set; } = AttackType.Normal;
        public List<SelectListItem> AttackTypeList { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            AttackTypeList = Enum.GetValues(typeof(AttackType))
                .Cast<AttackType>()
                .Select(a => new SelectListItem
                {
                    Value = a.ToString(),
                    Text = a.ToString()
                }).ToList();

            UserShipFleets = await (from fleet in _context.Fleets.AsNoTracking()
                                    join ship in _context.Ships.AsNoTracking() on fleet.ShipId equals ship.Id
                                    where fleet.ApplicationUserId == user.Id
                                          && ship.ShipType != ShipType.Starbase
                                          && ship.ShipType != ShipType.Scout
                                    orderby fleet.TotalPowerRating descending
                                    select new UserShipFleet
                                    {
                                        TotalShips = fleet.TotalShips,
                                        TotalPowerRating = fleet.TotalPowerRating,
                                        TotalUpkeep = fleet.TotalUpkeep,
                                        ShipName = ship.ShipName,
                                        ShipId = fleet.ShipId,
                                        FleetId = fleet.Id,
                                        ShipType = ship.ShipType
                                    })
                                    .Take(10)
                                    .ToListAsync();

            return Page();
        }
    }
}
