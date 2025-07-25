using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using AnotherSpaceGame.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ResearchModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;
        private readonly ILogger<ResearchModel> _logger;

        public ResearchModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, TurnService turnService, ILogger<ResearchModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
            _logger = logger;
        }

        public int CurrentLevel { get; set; }
        public int MaxLevel { get; } = 255;
        public int UnusedLevels { get; set; }
        public int AvailableTurns { get; set; }
        public int TurnsRemaining { get; set; }
        public string StatusMessage { get; set; }

        [BindProperty]
        [Range(1, 45, ErrorMessage = "Turns must be between 1 and 45")]
        public int TurnsToUse { get; set; }
        [BindProperty]
        public string? AssignCategory { get; set; }
        [BindProperty]
        [Range(1, 255, ErrorMessage = "Points must be at least 1.")]
        public int? PointsToAssign { get; set; }

        public Infrastructer Infrastructure { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
            Infrastructure = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            Turns turns = _context.Turns.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            if (Infrastructure == null)
            {
                Infrastructure = new Infrastructer { ApplicationUserId = user.Id };
                _context.Infrastructers.Add(Infrastructure);
                await _context.SaveChangesAsync();
            }
            CurrentLevel = Infrastructure.TotalLevels;
            UnusedLevels = Infrastructure.UnusedLevels;
            AvailableTurns = turns.CurrentTurns;
            TurnsRemaining = Infrastructure.TurnsRemaining;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostGainLevelsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
            Infrastructure = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            Turns turns = _context.Turns.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var modelErrors = ModelState[key].Errors;
                    foreach (var error in modelErrors)
                    {
                        // Log or display error.ErrorMessage
                        _logger.LogError(error.ErrorMessage);
                    }
                }
                CurrentLevel = Infrastructure.TotalLevels;
                UnusedLevels = Infrastructure.UnusedLevels;
                AvailableTurns = turns.CurrentTurns;
                TurnsRemaining = Infrastructure.TurnsRemaining;
                StatusMessage = "Invalid input.";
                return Page();
            }
            AvailableTurns = turns.CurrentTurns;
            if (TurnsToUse < 1)
            {
                CurrentLevel = Infrastructure.TotalLevels;
                UnusedLevels = Infrastructure.UnusedLevels;
                AvailableTurns = turns.CurrentTurns;
                TurnsRemaining = Infrastructure.TurnsRemaining;
                StatusMessage = "You must use atleast 1 turn.";
                return Page();
            }
            if (TurnsToUse > AvailableTurns)
            {
                TurnsToUse = AvailableTurns;
            }
                if (TurnsToUse > Infrastructure.TurnsRemaining)
                {
                    TurnsToUse = Infrastructure.TurnsRemaining;
                }
            if ( TurnsToUse <= AvailableTurns)
            {
                Infrastructure.TurnsRemaining -= TurnsToUse;                
            }
            if (Infrastructure.TurnsRemaining <= 0)
            {
                Infrastructure.TotalLevels += 1;
                Infrastructure.UnusedLevels += 1;
                Infrastructure.ITechInvestmentTurnsRequired += 200;
                Infrastructure.TurnsRemaining = GetTurnsRequiredForLevel(Infrastructure.TotalLevels);
                turns.CurrentTurns -= TurnsToUse;
                var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToUse);
                StatusMessage = $"You gained 1 infrastructure level(s) </br > {turnsMessage.Message}";
                await _context.SaveChangesAsync();
                CurrentLevel = Infrastructure.TotalLevels;
                UnusedLevels = Infrastructure.UnusedLevels;
                AvailableTurns = turns.CurrentTurns;
                TurnsRemaining = Infrastructure.TurnsRemaining;
                return Page();
            }
            else
            {
                var turnsMessage = await _turnService.TryUseTurnsAsync(currentUser.Id, TurnsToUse);
                StatusMessage = $"{turnsMessage.Message}";
                await _context.SaveChangesAsync();
                CurrentLevel = Infrastructure.TotalLevels;
                UnusedLevels = Infrastructure.UnusedLevels;
                AvailableTurns = turns.CurrentTurns;
                TurnsRemaining = Infrastructure.TurnsRemaining;
                return Page();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAssignPointsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            var currentUser = _context.Users
                .FirstOrDefault(u => u.Id == user.Id);
            Infrastructure = _context.Infrastructers.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            Turns turns = _context.Turns.FirstOrDefault(i => i.ApplicationUserId == currentUser.Id);
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                StatusMessage = "Invalid input.";
                return Page();
            }

            if (PointsToAssign < 1 || PointsToAssign > Infrastructure.UnusedLevels)
            {
                await OnGetAsync();
                StatusMessage = "You do not have enough unused levels.";
                return Page();
            }
            if (string.IsNullOrEmpty(AssignCategory))
            {
                await OnGetAsync();
                StatusMessage = "Please select a category to assign points.";
                return Page();
            }
            if (PointsToAssign > Infrastructure.UnusedLevels)
            {
                await OnGetAsync();
                StatusMessage = "You cannot assign more points than you have unused levels.";
                return Page();
            }

            // Assign points to the selected category, only if not previously assigned
            switch (AssignCategory)
            {
                case "Housing":
                    Infrastructure.Housing += (int)PointsToAssign;
                    break;
                case "Commercial":
                    Infrastructure.Commercial += (int)PointsToAssign;
                    break;
                case "Agriculture":
                    Infrastructure.Agriculture += (int)PointsToAssign;
                    break;
                case "Industry":
                    Infrastructure.Industry += (int)PointsToAssign;
                    break;
                case "Mining":
                    Infrastructure.Mining += (int)PointsToAssign;
                    break;
                default:
                    await OnGetAsync();
                    StatusMessage = "Invalid category.";
                    return Page();
            }

            Infrastructure.UnusedLevels -= (int)PointsToAssign;
            await _context.SaveChangesAsync();

            StatusMessage = $"Assigned {PointsToAssign} point(s) to {AssignCategory}.";
            await OnGetAsync();
            return Page();
        }

        // Level cost logic
        public static int GetTurnsRequiredForLevel(int level)
        {
            int[] costs = new int[]
            {
                2,3,4,5,6,7,8,9,10,12,14,16,19,22,26,31,37,44,52,62,74,88,105,126,151,181,217,260,312,374,448,537,644,750
            };
            if (level < 1) return 0;
            if (level <= 34) return costs[level - 1];
            if (level <= 100) return 750;
            if (level <= 150) return 1250;
            if (level <= 200) return 1750;
            if (level <= 255) return 2250;
            return int.MaxValue;
        }
    }
}