using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class ManagePlanetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TurnService _turnService;

        public ManagePlanetModel(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            TurnService turnService)
        {
            _userManager = userManager;
            _context = context;
            _turnService = turnService;
        }

        public Planets Planet { get; set; }
        public long RawMaterial { get; set; }
        public long Goods { get; set; }
        public long Ore { get; set; }
        public int Turns { get; set; }
        public string TurnMessage { get; set; }
        public Faction Faction { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            TurnMessage = TempData["TurnMessage"] as string;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            Faction = user.Faction;
            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == user.Id);
            

            var commodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            RawMaterial = commodities?.RawMaterial ?? 0;
            Goods = commodities?.ConsumerGoods ?? 0;
            Ore = commodities?.Ore ?? 0;
            Turns = await _turnService.GetTurnsAsync(user.Id);
            // Check if the Planet exists and belongs to the current user
            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                // Redirect to ManageColonies if not authorized
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];
            int id = int.Parse(Request.Form["id"]);
            int housing = int.TryParse(Request.Form["housing"], out var h) ? h : 0;
            int commercial = int.TryParse(Request.Form["commercial"], out var c) ? c : 0;
            int agriculture = int.TryParse(Request.Form["agriculture"], out var a) ? a : 0;
            int industry = int.TryParse(Request.Form["industry"], out var i) ? i : 0;
            int mining = int.TryParse(Request.Form["mining"], out var m) ? m : 0;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            Faction = user.Faction;
            Planet = await _context.Planets.FirstOrDefaultAsync(p => p.Id == id && p.ApplicationUserId == user.Id);
            var Infra = await _context.Infrastructers.FirstOrDefaultAsync(i => i.ApplicationUserId == user.Id);

            // Check if the Planet exists and belongs to the current user
            if (Planet == null || Planet.ApplicationUserId != user.Id)
            {
                // Redirect to ManageColonies if not authorized
                return RedirectToPage("/ManageColonies", new { area = "Game" });
            }

            var commodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            RawMaterial = commodities?.RawMaterial ?? 0;
            Goods = commodities?.ConsumerGoods ?? 0;
            Ore = commodities?.Ore ?? 0;
            Turns = await _turnService.GetTurnsAsync(user.Id);

            // Resource requirements/returns per unit (adjust as needed)
            int landPerHousing = 1, landPerCommercial = 1, landPerAgriculture = 1, landPerIndustry = 1, landPerMining = 1;
            int laborPerHousing = 1, laborPerCommercial = 1, laborPerAgriculture = 1, laborPerIndustry = 1, laborPerMining = 1;
            int orePerHousing = 1, orePerCommercial = 1, orePerAgriculture = 1, orePerIndustry = 1, orePerMining = 1;

            if (action == "build")
            {
                if (housing + commercial + agriculture + industry + mining == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please enter at least one value to build.");
                    return await OnGetAsync(id);
                }
                if (Turns == 0)
                {
                    ModelState.AddModelError(string.Empty, "You need atleast 1 turn to build.");
                    return await OnGetAsync(id);
                }


                int totalLandRequired =
                    housing * landPerHousing +
                    commercial * landPerCommercial +
                    agriculture * landPerAgriculture +
                    industry * landPerIndustry +
                    mining * landPerMining;

                int totalLaborRequired =
                    housing * laborPerHousing +
                    commercial * laborPerCommercial +
                    agriculture * laborPerAgriculture +
                    industry * laborPerIndustry +
                    mining * laborPerMining;

                int totalOreRequired =
                    housing * orePerHousing +
                    commercial * orePerCommercial +
                    agriculture * orePerAgriculture +
                    industry * orePerIndustry +
                    mining * orePerMining;

                if (Planet.LandAvailable < totalLandRequired)
                {
                    ModelState.AddModelError(string.Empty, "Not enough land available for the requested construction.");
                    return await OnGetAsync(id);
                }
                if (Planet.AvailableLabour < totalLaborRequired)
                {
                    ModelState.AddModelError(string.Empty, "Not enough labor available for the requested construction.");
                    return await OnGetAsync(id);
                }
                if (Ore < totalOreRequired)
                {
                    ModelState.AddModelError(string.Empty, "Not enough ore available for the requested construction.");
                    return await OnGetAsync(id);
                }

                if (housing > 0) Planet.Housing += housing;
                if (commercial > 0) Planet.Commercial += commercial;
                if (agriculture > 0) Planet.Agriculture += agriculture;
                if (industry > 0) Planet.Industry += industry;
                if (mining > 0) Planet.Mining += mining;

                Planet.LandAvailable -= totalLandRequired;
                Planet.AvailableLabour -= totalLaborRequired;
                Ore -= totalOreRequired;
                var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 1);
                TempData["TurnMessage"] = turnsMessage.Message;
                TurnMessage = $"Build successful! 1 turn used.<hr>{turnsMessage.Message}";
                await _context.SaveChangesAsync();
            }
            else if (action == "demolish")
            {
                if (housing + commercial + agriculture + industry + mining == 0)
                {
                    ModelState.AddModelError(string.Empty, "Please enter at least one value to demolish.");
                    return await OnGetAsync(id);
                }
                if (Turns == 0)
                {
                    ModelState.AddModelError(string.Empty, "You need atleast 1 turn to demolish.");
                    return await OnGetAsync(id);
                }

                if (housing > Planet.Housing)
                {
                    ModelState.AddModelError(string.Empty, "You cannot demolish more Housing than you have.");
                    return await OnGetAsync(id);
                }
                if (Planet.Housing - housing < 1)
                {
                    ModelState.AddModelError(string.Empty, "You must have at least 1 Housing remaining on the Planet.");
                    return await OnGetAsync(id);
                }
                if (commercial > Planet.Commercial)
                {
                    ModelState.AddModelError(string.Empty, "You cannot demolish more Commercial than you have.");
                    return await OnGetAsync(id);
                }
                if (agriculture > Planet.Agriculture)
                {
                    ModelState.AddModelError(string.Empty, "You cannot demolish more Agriculture than you have.");
                    return await OnGetAsync(id);
                }
                if (industry > Planet.Industry)
                {
                    ModelState.AddModelError(string.Empty, "You cannot demolish more Industry than you have.");
                    return await OnGetAsync(id);
                }
                if (mining > Planet.Mining)
                {
                    ModelState.AddModelError(string.Empty, "You cannot demolish more Mining than you have.");
                    return await OnGetAsync(id);
                }

                int totalLandReturned =
                    housing * landPerHousing +
                    commercial * landPerCommercial +
                    agriculture * landPerAgriculture +
                    industry * landPerIndustry +
                    mining * landPerMining;

                int totalLaborReturned =
                    housing * laborPerHousing +
                    commercial * laborPerCommercial +
                    agriculture * laborPerAgriculture +
                    industry * laborPerIndustry +
                    mining * laborPerMining;

                int totalOreReturned =
                    housing * orePerHousing +
                    commercial * orePerCommercial +
                    agriculture * orePerAgriculture +
                    industry * orePerIndustry +
                    mining * orePerMining;

                if (housing > 0) Planet.Housing -= housing;
                if (commercial > 0) Planet.Commercial -= commercial;
                if (agriculture > 0) Planet.Agriculture -= agriculture;
                if (industry > 0) Planet.Industry -= industry;
                if (mining > 0) Planet.Mining -= mining;
                Planet.MaxPopulation = (int)Math.Ceiling((double)(Planet.Housing * 10 + (Planet.Housing * Infra.Housing)));
                if (user.Faction == Faction.Collective)
                {
                    Planet.MaxPopulation = (int)Math.Ceiling((double)(Planet.Housing * 10 + (Planet.Housing * Infra.Housing)) * 2);
                }
                
                if (Planet.CurrentPopulation > Planet.MaxPopulation)
                {
                    Planet.CurrentPopulation = Planet.MaxPopulation;
                }
                
                Planet.LandAvailable += totalLandReturned;
                Planet.AvailableLabour = (int)Math.Floor((double)(Planet.CurrentPopulation - (Planet.Housing + Planet.Commercial + Planet.Industry + Planet.Agriculture + Planet.Mining))); ;
                var turnsMessage = await _turnService.TryUseTurnsAsync(user.Id, 1);
                TempData["TurnMessage"] = turnsMessage.Message;
                TurnMessage = $"Demolish successful! 1 turn used.<hr> {turnsMessage.Message}";
                await _context.SaveChangesAsync();

            }            
            return RedirectToPage(new { id = Planet.Id });
        }
    }
}
