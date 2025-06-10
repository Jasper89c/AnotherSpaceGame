using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class UnreverseEngineeringModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UnreverseEngineeringModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var userProjects = _context.UserProjects.FirstOrDefault(up => up.ApplicationUserId == user.Id);
            if (userProjects == null)
            {
                StatusMessage = "User projects not found.";
                return RedirectToPage();
            }
            if (user.Faction != Faction.Viral)
            {
                StatusMessage = "You must be a Viral to Unreverse Engineering.";
                return RedirectToPage();
            }

            // Check the required conditions before resetting
            if (userProjects.UnreverseEngineering == true &&
                userProjects.UnreverseEngineeringTurnsRequired == 0 &&
                userProjects.UnreverseEngineeringCreditsRequired == 0)
            {
                // Reset ViralReversedShips to default (assuming parameterless constructor is default)
                if (user.ViralReversedShips != null)
                {
                    user.ViralReversedShips = new ViralReversedShips();
                    _context.Update(user);
                }

                // Reset UserProjects properties to default
                userProjects.UnreverseEngineering = false;
                userProjects.UnreverseEngineeringTurnsRequired = 100;
                userProjects.UnreverseEngineeringCreditsRequired = 100000000;
                _context.Update(userProjects);

                await _context.SaveChangesAsync();
                StatusMessage = "Unreverse Engineering has been reset to default.";
            }
            else
            {
                StatusMessage = "You cannot Unreverse Engineering without completing the project first.";
            }

            return RedirectToPage();
        }
    }
}
