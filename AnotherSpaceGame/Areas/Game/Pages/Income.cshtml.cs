using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class IncomeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IncomeModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public decimal CreditsPerTurn { get; set; }
        public decimal CommercialPerTurn { get; set; }
        public decimal AgriculturePerTurn { get; set; }
        public decimal IndustryPerTurn { get; set; }
        public decimal MiningPerTurn { get; set; }
        public decimal InfrastructureCostPerTurn { get; set; }
        public decimal FleetCostPerTurn { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            // Example: Replace these with your actual calculation logic
            //CreditsPerTurn = CalculateCreditsPerTurn(user);
            //CommercialPerTurn = CalculateCommercialPerTurn(user);
            //AgriculturePerTurn = CalculateAgriculturePerTurn(user);
            //IndustryPerTurn = CalculateIndustryPerTurn(user);
            //MiningPerTurn = CalculateMiningPerTurn(user);
            //InfrastructureCostPerTurn = CalculateInfrastructureCostPerTurn(user);
            //FleetCostPerTurn = CalculateFleetCostPerTurn(user);

            return Page();
        }

        //        // Example calculation methods (replace with your actual logic)
        //        private decimal CalculateCreditsPerTurn(ApplicationUser user)
        //        {
        //            // Example: base credits + commercial + industry + mining + agriculture - costs
        //            return 1000 + CommercialPerTurn + IndustryPerTurn + MiningPerTurn + AgriculturePerTurn - InfrastructureCostPerTurn - FleetCostPerTurn;
        //        }
        //        private decimal CalculateCommercialPerTurn(ApplicationUser user) => 200; // Replace with actual logic
        //        private decimal CalculateAgriculturePerTurn(ApplicationUser user) => 150; // Replace with actual logic
        //        private decimal CalculateIndustryPerTurn(ApplicationUser user) => 300; // Replace with actual logic
        //        private decimal CalculateMiningPerTurn(ApplicationUser user) => Math.Floor((planet.TotalPlanets* ((0.13m * infra.Mining) + 1)) * mods.FactionMiningModifier); // Replace with actual logic
        //        private decimal CalculateInfrastructureCostPerTurn(ApplicationUser user) => 100; // Replace with actual logic
        //        private decimal CalculateFleetCostPerTurn(ApplicationUser user) => 80; // Replace with actual logic
    }
}
