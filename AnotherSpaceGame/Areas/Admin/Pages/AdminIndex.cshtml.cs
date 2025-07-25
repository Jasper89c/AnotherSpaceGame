using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AnotherSpaceGame.Areas.Admin.Pages
{
    public class AdminIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminIndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string SelectedUserId { get; set; }
        [BindProperty]
        public PlanetType SelectedPlanetType { get; set; }
        [BindProperty]
        public string PlanetName { get; set; }
        [BindProperty]
        public int FoodRequired { get; set; } = 1;
        [BindProperty]
        public int GoodsRequired { get; set; } = 1;
        [BindProperty]
        public int CurrentPopulation { get; set; } = 10;
        [BindProperty]
        public int MaxPopulation { get; set; } = 10;
        [BindProperty]
        public int Loyalty { get; set; } = 2500;
        [BindProperty]
        public int AvailableLabour { get; set; } = 8;
        [BindProperty]
        public int Housing { get; set; } = 1;
        [BindProperty]
        public int Commercial { get; set; } = 0;
        [BindProperty]
        public int Industry { get; set; } = 0;
        [BindProperty]
        public int Agriculture { get; set; } = 0;
        [BindProperty]
        public int Mining { get; set; } = 1;
        [BindProperty]
        public int PowerRating { get; set; } = 0;
        [BindProperty]
        public int LandAvailable { get; set; } = 100;
        [BindProperty]
        public int TotalLand { get; set; } = 100;

        // Commodity edit properties
        [BindProperty]
        public string CommodityUserId { get; set; }
        [BindProperty]
        public int Food { get; set; }
        [BindProperty]
        public int Goods { get; set; }
        [BindProperty]
        public int Ore { get; set; }
        [BindProperty]
        public int Credits { get; set; }

        public List<ApplicationUser> Users { get; set; }
        public List<PlanetType> PlanetTypes { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.OrderBy(u => u.UserName).ToListAsync();
            PlanetTypes = System.Enum.GetValues(typeof(PlanetType)).Cast<PlanetType>().ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Users = await _context.Users.OrderBy(u => u.UserName).ToListAsync();
            PlanetTypes = System.Enum.GetValues(typeof(PlanetType)).Cast<PlanetType>().ToList();

            if (string.IsNullOrEmpty(SelectedUserId) || string.IsNullOrEmpty(PlanetName))
            {
                ModelState.AddModelError(string.Empty, "User and planet name are required.");
                return Page();
            }

            var user = await _context.Users.FindAsync(SelectedUserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }

            var newPlanet = new Planets
            {
                ApplicationUserId = user.Id,
                Name = PlanetName,
                Type = SelectedPlanetType,
                FoodRequired = FoodRequired,
                GoodsRequired = GoodsRequired,
                CurrentPopulation = CurrentPopulation,
                MaxPopulation = MaxPopulation,
                Loyalty = Loyalty,
                AvailableLabour = AvailableLabour,
                Housing = Housing,
                Commercial = Commercial,
                Industry = Industry,
                Agriculture = Agriculture,
                Mining = Mining,
                MineralProduced = MineralType.Rutile,
                PowerRating = PowerRating,
                LandAvailable = LandAvailable,
                TotalLand = TotalLand
            };

            _context.Planets.Add(newPlanet);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Planet added successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditCommoditiesAsync()
        {
            Users = await _context.Users.OrderBy(u => u.UserName).ToListAsync();

            if (string.IsNullOrEmpty(CommodityUserId))
            {
                ModelState.AddModelError(string.Empty, "User is required.");
                return Page();
            }

            var user = await _context.Users.FindAsync(CommodityUserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }
            var commodities = await _context.Commodities.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            // Update commodity values (adjust property names as needed)
            commodities.Food = Food;
            commodities.ConsumerGoods = Goods;
            commodities.Ore = Ore;
            commodities.Credits = Credits;

            await _context.SaveChangesAsync();

            TempData["Success"] = "User commodities updated successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateArtifactsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var currentuser = _context.Users
                .FirstOrDefault(x => x.Id == user.Id);
            TempData["Success"] = "Created:";
            for (int i = 1; i <= 64; i++)
            {
                var artifact = new Artifacts(i,10,currentuser.Id);
                _context.Artifacts.Add(artifact);
                TempData["Success"] += $"/{i}";
            }
            await _context.SaveChangesAsync();
            
            return RedirectToPage();
        }
    }
}
