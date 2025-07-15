using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Models;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class EmpireSummaryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpireSummaryModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
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
                EmpireAge = (DateTime.UtcNow - user.PlayingSince).Days;
                PlayingSince = user.PlayingSince;
                BattlesWon = user.BattlesWon;
                BattlesLost = user.BattlesLost;
                ColoniesWon = user.ColoniesWon;
                ColoniesLost = user.ColoniesLost;
                ColoniesExplored = user.ColoniesExplored;
                PlanetsPlundered = user.PlanetsPlundered;
                LastActivity = user.LastAction; 
            }
            return Page();
        }
    }
}
