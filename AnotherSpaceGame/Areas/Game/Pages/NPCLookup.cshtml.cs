using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class NPCLookUpModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public NPCLookUpModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (id == null)
                return NotFound();

            var NPC = await _context.NPCs
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (NPC == null)
                return NotFound();

            Username = NPC.UserName;
            DamageProtection = NPC.DamageProtection;
            Faction = NPC.Faction;
            TotalColonies = 1;
            TotalPlanets = 1;
            PowerRating = NPC.PowerRating;
            Federation = NPC.Federation?.FederationName ?? "None";
            PlayingSince = NPC.PlayingSince;
            EmpireAge = (DateTime.UtcNow - user.PlayingSince).Days;
            BattlesWon = NPC.BattlesWon;
            BattlesLost = NPC.BattlesLost;
            ColoniesWon = NPC.ColoniesWon;
            ColoniesLost = NPC.ColoniesLost;
            ColoniesExplored = NPC.ColoniesExplored;
            PlanetsPlundered = NPC.PlanetsPlundered;

            return Page();
        }
    }
}
