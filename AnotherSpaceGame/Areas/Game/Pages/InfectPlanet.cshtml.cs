using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class InfectPlanetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public InfectPlanetModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        [BindProperty]
        public Planets? Planet { get; set; }

        [BindProperty]
        public int PlanetId { get; set; }

        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            Planet = _context.Planets.FirstOrDefault(p => p.Id == id && p.ApplicationUserId == user.Id);
            if (Planet == null)
                return RedirectToPage("/Game/ManageColonies");

            PlanetId = Planet.Id;
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            Planet = _context.Planets.FirstOrDefault(p => p.Id == PlanetId && p.ApplicationUserId == user.Id);
            if (Planet == null)
                return RedirectToPage("/Game/ManageColonies");

            // Check if user has enough turns
            var userTurns = _context.Turns.FirstOrDefault(x => x.ApplicationUserId == user.Id);
            int availableTurns = userTurns.CurrentTurns;
            if (availableTurns < 5)
            {
                StatusMessage = "You do not have enough turns to infect this planet (5 required).";
                return Page();
            }

            // Infect the planet (set a property, e.g., IsInfected = true)
            // You may need to add this property to your Planets model if it doesn't exist
            // Planet.IsInfected = true;
            if(user.Faction == Faction.Viral)
            {
                switch (Planet.Type)
                {
                    case PlanetType.Barren:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Icy:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Marshy:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Forest:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Oceanic:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Rocky:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Desert:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Balanced:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.Gas:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.TaintedC1;
                        Planet.PopulationModifier = 0.8m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.ClusterLevel1:
                        Planet.Name = "C1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.InfectedC1;
                        Planet.PopulationModifier = 0.7m;
                        Planet.AgricultureModifier = 0.9m;
                        break;
                    case PlanetType.ClusterLevel2:
                        Planet.Name = "C2." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.InfectedC2;
                        Planet.PopulationModifier = 0.65m;
                        Planet.AgricultureModifier = 0.1m;
                        break;
                    case PlanetType.ClusterLevel3:
                        Planet.Name = "C3." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.InfectedC3;
                        Planet.PopulationModifier = 0.7m;
                        Planet.AgricultureModifier = 1.1m;
                        break;
                    default:
                        StatusMessage = "You cannot infect this planet type.";
                        return Page();
                        break;
                }
            }
            if (user.Faction == Faction.Collective)
            {
                
                switch (Planet.Type)
                {
                    case PlanetType.Barren:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Icy:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Marshy:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Forest:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Oceanic:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Rocky:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Desert:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Balanced:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.Gas:
                        Planet.Name = "C.1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.SimilareC1;
                        Planet.PopulationModifier = 1.2m;
                        Planet.AgricultureModifier = 0.3m;
                        break;
                    case PlanetType.ClusterLevel1:
                        Planet.Name = "C1." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.AssimilatedC1;
                        Planet.PopulationModifier = 1m;
                        Planet.AgricultureModifier = 0.8m;
                        break;
                    case PlanetType.ClusterLevel2:
                        Planet.Name = "C2." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.AssimilatedC2;
                        Planet.PopulationModifier = 1m;
                        Planet.AgricultureModifier = 0.9m;
                        break;
                    case PlanetType.ClusterLevel3:
                        Planet.Name = "C3." + Random.Shared.RandomString(4);
                        Planet.Type = PlanetType.AssimilatedC3;
                        Planet.PopulationModifier = 1m;
                        Planet.AgricultureModifier = 1m;
                        break;
                    default:
                        StatusMessage = "You cannot infect this planet type.";
                        return Page();
                        break;
                }
            }            

            // Deduct 5 turns
            var turnMessage = await _turnService.TryUseTurnsAsync(user.Id, 5);
            await _context.SaveChangesAsync();

            StatusMessage = $"Planet successfully infected! <hr> {turnMessage.Message}";
            return Page();
        }
    }
}