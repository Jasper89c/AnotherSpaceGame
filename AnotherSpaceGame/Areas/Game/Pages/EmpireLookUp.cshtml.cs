using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class EmpireLookUpModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmpireLookUpModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string Username { get; set; }
        public DateTime DamageProtection { get; set; }
        public Faction Faction { get; set; }
        public int TotalColonies { get; set; }
        public int TotalPlanets { get; set; }
        public int PowerRating { get; set; }
        public string Federation { get; set; }
        public DateTime PlayingSince { get; set; }
        public double EmpireAge { get; set; }
        public int BattlesWon { get; set; }
        public int BattlesLost { get; set; }
        public int ColoniesWon { get; set; }
        public int ColoniesLost { get; set; }
        public int ColoniesExplored { get; set; }
        public int PlanetsPlundered { get; set; }
        public DateTime LastActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _context.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.UserName == id);

            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Username = user.UserName;
            DamageProtection = user.DamageProtection;
            Faction = user.Faction;
            TotalColonies = user.TotalColonies;
            TotalPlanets = user.TotalPlanets;
            PowerRating = user.PowerRating;
            Federation = user.Federation?.FederationName ?? "None";
            PlayingSince = user.PlayingSince;
            EmpireAge = (DateTime.UtcNow - user.PlayingSince).Days;
            BattlesWon = user.BattlesWon;
            BattlesLost = user.BattlesLost;
            ColoniesWon = user.ColoniesWon;
            ColoniesLost = user.ColoniesLost;
            ColoniesExplored = user.ColoniesExplored;
            PlanetsPlundered = user.PlanetsPlundered;
            LastActivity = user.LastAction;

            return Page();
        }
    }
}
