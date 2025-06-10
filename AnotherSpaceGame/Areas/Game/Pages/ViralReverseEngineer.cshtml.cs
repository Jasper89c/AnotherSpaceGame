using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ViralReverseEngineerModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ViralReverseEngineerModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int ShipId { get; set; }

        public string Message { get; set; }

        public void OnGet(int shipId)
        {
            ShipId = shipId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if(user.Faction != Faction.Viral)
            {
                Message = "You must be a Viral to reverse engineer ships.";
                return Page();
            }
            // Check if ShipId is in the allowed ranges
            bool validShipId =
                (ShipId >= 219 && ShipId <= 229) ||
                (ShipId >= 237 && ShipId <= 249) ||
                (ShipId >= 275 && ShipId <= 284);

            if (!validShipId)
            {
                Message = "This ship cannot be reverse engineered.";
                return Page();
            }

            bool hasFleet = _context.Fleets.Any(f => f.ApplicationUserId == user.Id && f.ShipId == ShipId && f.TotalShips > 0);

            if (hasFleet == false)
            {
                Message = "You do not have any of this ship in your fleet. Capture more to reverse engineer.";
                return Page();
            }
            else
            {
                // Logic for reverse engineering the ship
                var viralReversedShips = await _context.ViralReversedShips.FirstOrDefaultAsync(v => v.ApplicationUserId == user.Id);
                if (viralReversedShips == null)
                {
                    viralReversedShips = new ViralReversedShips { ApplicationUserId = user.Id };
                    _context.ViralReversedShips.Add(viralReversedShips);
                }
                // Increment the count for the specific ship type
                if (ShipId >= 219 && ShipId <= 229)
                {
                    if (viralReversedShips.TerranShip1Id == 0)
                    {
                        viralReversedShips.TerranShip1Id = ShipId;
                        viralReversedShips.TerranReversedShipsCount++;
                    }
                    else if (viralReversedShips.TerranShip2Id == 0)
                    {
                        viralReversedShips.TerranShip2Id = ShipId;
                        viralReversedShips.TerranReversedShipsCount++;
                    }
                    else if (viralReversedShips.TerranShip3Id == 0)
                    {
                        viralReversedShips.TerranShip3Id = ShipId;
                        viralReversedShips.TerranReversedShipsCount++;
                    }
                    else if (viralReversedShips.TerranShip1Id != 0 && viralReversedShips.TerranShip2Id != 0 && viralReversedShips.TerranShip3Id != 0 &&
                    viralReversedShips.TerranReversedShipsCount == 3)
                    {
                        Message = "You have already reverse engineered all Terran ships.";
                    }
                }
                else if (ShipId >= 237 && ShipId <= 249)
                {
                    if (viralReversedShips.AminerShip1Id == 0)
                    {
                        viralReversedShips.AminerShip1Id = ShipId;
                        viralReversedShips.AMinerReversedShipsCount++;
                    }
                    else if (viralReversedShips.AminerShip2Id == 0)
                    {
                        viralReversedShips.AminerShip2Id = ShipId;
                        viralReversedShips.AMinerReversedShipsCount++;
                    }
                    else if (viralReversedShips.AminerShip3Id == 0)
                    {
                        viralReversedShips.AminerShip3Id = ShipId;
                        viralReversedShips.AMinerReversedShipsCount++;
                    }
                    else if (viralReversedShips.AminerShip1Id != 0 && viralReversedShips.AminerShip2Id != 0 && viralReversedShips.AminerShip3Id != 0 && viralReversedShips.AMinerReversedShipsCount == 3)
                    {
                        Message = "You have already reverse engineered all A-Miner ships.";
                    }
                }
                else if (ShipId >= 275 && ShipId <= 284)
                {
                    if (viralReversedShips.MarauderShip1Id == 0)
                    {
                        viralReversedShips.MarauderShip1Id = ShipId;
                        viralReversedShips.MarauderReversedShipsCount++;
                    }
                    else if (viralReversedShips.MarauderShip2Id == 0)
                    {
                        viralReversedShips.MarauderShip2Id = ShipId;
                        viralReversedShips.MarauderReversedShipsCount++;
                    }
                    else if (viralReversedShips.MarauderShip3Id == 0)
                    {
                        viralReversedShips.MarauderShip3Id = ShipId;
                        viralReversedShips.MarauderReversedShipsCount++;
                    }
                    else if (viralReversedShips.MarauderShip1Id != 0 && viralReversedShips.MarauderShip2Id != 0 && viralReversedShips.MarauderShip3Id != 0 && viralReversedShips.MarauderReversedShipsCount == 3)
                    {
                        Message = "You have already reverse engineered all Marauder ships.";
                    }
                    
                }
                await _context.SaveChangesAsync();
                Message = "Ship successfully reverse engineered!";
            }

            return Page();
        }
    }
}
