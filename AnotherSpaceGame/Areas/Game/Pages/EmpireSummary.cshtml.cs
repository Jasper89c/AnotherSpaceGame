using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class EmpireSummaryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public EmpireSummaryModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public string Username { get; set; }
        public DateTime DamageProtection { get; set; }
        public Faction Faction { get; set; }
        public int TotalColonies { get; set; }
        public int TotalPlanets { get; set; }
        public int PowerRating { get; set; }
        public string Federation { get; set; }
        public DateTime PlayingSince { get; set; }
        public int EmpireAge { get; set; }
        public int BattlesWon { get; set; }
        public int BattlesLost { get; set; }
        public int ColoniesWon { get; set; }
        public int ColoniesLost { get; set; }
        public int ColoniesExplored { get; set; }
        public int PlanetsPlundered { get; set; }
        public DateTime LastActivity { get; set; }
        public int? FederationId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _dbContext.Users
                .Include(u => u.Federation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            if (user != null)
            {
                Username = user.UserName;
                DamageProtection = user.DamageProtection;
                Faction = user.Faction;
                TotalColonies = user.TotalColonies;
                TotalPlanets = user.TotalPlanets;
                PowerRating = user.PowerRating;
                Federation = user.Federation?.FederationName ?? "None";
                EmpireAge = (DateTime.Now - user.PlayingSince).Days;
                PlayingSince = user.PlayingSince;
                BattlesWon = user.BattlesWon;
                BattlesLost = user.BattlesLost;
                ColoniesWon = user.ColoniesWon;
                ColoniesLost = user.ColoniesLost;
                ColoniesExplored = user.ColoniesExplored;
                PlanetsPlundered = user.PlanetsPlundered;
                LastActivity = user.LastAction; 
                FederationId = user.FederationId;
            }
            
            return Page();
        }
    }
}
