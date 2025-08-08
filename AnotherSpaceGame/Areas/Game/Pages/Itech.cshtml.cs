using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ItechModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TurnService _turnService;
        [TempData]
        public string StatusMessage { get; set; }
        public int TotalLevels { get; set; }
        public int TurnsRequired { get; set; }
        public bool CanRedistribute { get; set; }
        public Infrastructer Infrastructer { get; set; }
        public DateTime CooldownTimer { get; set; }
        public UserProjects UserProjects { get; set; }

        public ItechModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, TurnService turnService)
        {
            _context = context;
            _userManager = userManager;
            _turnService = turnService;
        }

        

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Infrastructer = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            UserProjects = _context.UserProjects.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            if(UserProjects.Itech != true)
            {
                return RedirectToPage("/Projects");
            }
            if (Infrastructer == null)
            {
                StatusMessage = "Infrastructure not found.";
                return Page();
            }
            ServerStats serverStats = _context.ServerStats.FirstOrDefault();
            if(serverStats.UWEnabled == true)
            {
                return RedirectToPage("/Projects");
            }
            TotalLevels = Infrastructer.TotalLevels; // Assume you have a property or method to get total levels
            TurnsRequired = TotalLevels * 200;
            CanRedistribute = user.ITechCooldown < DateTime.Now;
            CooldownTimer = user.ITechCooldown;

            return Page();
        }

        public async Task<IActionResult> OnPostInvestmentAsync(int investment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Infrastructer = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            var userTurns = _context.Turns.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            CanRedistribute = user.ITechCooldown < DateTime.Now;
            CooldownTimer = user.ITechCooldown;
            if (Infrastructer == null)
            {
                StatusMessage = "Infrastructure not found.";
                return Page();
            }
            if (!CanRedistribute)
            {
                StatusMessage = "You cannot invest in ITech yet. Please wait for the cooldown period to end.";
                return Page();
            }
            if (investment < 0)
            {
                StatusMessage = "Investment cannot be negative.";
                return Page();
            }
            if (investment > userTurns.CurrentTurns)
            {
                StatusMessage = $"You do not have enough turns. You have {userTurns.CurrentTurns} turns remaining.";
                return Page();
            }
            if (investment > (Infrastructer.ITechInvestmentTurnsRequired - Infrastructer.ITechInvestmentTurns))
            {
                investment = Infrastructer.ITechInvestmentTurnsRequired - Infrastructer.ITechInvestmentTurns;
            }
            // Update ITech investment turns
            Infrastructer.ITechInvestmentTurns += investment;
            var turnResult = await _turnService.TryUseTurnsAsync(user.Id, investment);

            if (Infrastructer.ITechInvestmentTurns >= Infrastructer.ITechInvestmentTurnsRequired)
            {
                StatusMessage = $"ITech investment updated. Current investment turns: {Infrastructer.ITechInvestmentTurns}";
            }
            else
            {
                StatusMessage = $"Turns invested, {turnResult.Message}";
            }
                return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRedistributeAsync(int housing, int commercial, int industry, int agriculture, int mining)
        {
            
            
            // ... your existing logic to update infrastructure levels

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            CanRedistribute = user.ITechCooldown < DateTime.Now;
            CooldownTimer = user.ITechCooldown;
            Infrastructer = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == user.Id);
            int total = housing + commercial + industry + agriculture + mining;
            TotalLevels = Infrastructer.TotalLevels;
            if (total > TotalLevels)
            {
                StatusMessage = $"The sum of all levels cannot exceed {TotalLevels}. Your total: {total}.";
                return Page();
            }
            if (Infrastructer == null)
            {
                StatusMessage = "Infrastructure not found.";
                return Page();
            }
            if (!CanRedistribute)
            {
                StatusMessage = "You cannot redistribute infrastructure yet. Please wait for the cooldown period to end.";
                return Page();
            }
            // Update infrastructure levels
            Infrastructer.Housing = housing;
            Infrastructer.Commercial = commercial;
            Infrastructer.Industry = industry;
            Infrastructer.Agriculture = agriculture;
            Infrastructer.Mining = mining;
            Infrastructer.UnusedLevels = 0;
            Infrastructer.ITechInvestmentTurns = 0; // Reset ITech investment turns
            user.ITechCooldown = DateTime.Now.AddHours(48); // Set cooldown to 48 hours
            _context.Infrastructers.Update(Infrastructer);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            StatusMessage = "Infrastructure redistributed successfully!";
            return RedirectToPage();
        }

    }
}
