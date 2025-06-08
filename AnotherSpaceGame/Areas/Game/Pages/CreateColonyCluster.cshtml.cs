using AnotherSpaceGame.Data;
using AnotherSpaceGame.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;

namespace AnotherSpaceGame.Areas.Game.Pages
{
    public class CreateColonyClusterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CreateColonyClusterModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public List<Planets> SelectedPlanets { get; set; } = new();
        public List<Planets> userPlanets { get; set; } = new(); 
        public List<SelectListItem> MineralTypeSelectList { get; set; } = new();
        public string? SelectedMineralType { get; set; }
        [BindProperty]
        public List<int> SelectedPlanetIds { get; set; } = new();
        public Planets? NewPlanet { get; set; } = new();
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToPage("/Account/Login");

            switch (id)
            {
                case 1:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.Icy | p.Type == PlanetType.Rocky | p.Type == PlanetType.Oceanic | p.Type == PlanetType.Balanced | p.Type == PlanetType.Barren | p.Type == PlanetType.Gas | p.Type == PlanetType.Forest | p.Type == PlanetType.Desert | p.Type == PlanetType.Marshy)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(5)
                        .ToList();
                    break;
                case 2:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.ClusterLevel1)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(5)
                        .ToList();
                    break;
                case 3:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.ClusterLevel2)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(5)
                        .ToList();
                    break;
                case 4:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.TaintedC1)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 5:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.TaintedC2)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 6:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.TaintedC3)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 7:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.InfectedC1)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(5)
                        .ToList();
                    break;
                case 8:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.InfectedC2)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(5)
                        .ToList();
                    break;
                case 9:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.SimilareC1)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 10:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.SimilareC2)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 11:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.SimilareC3)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 12:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.AssimilatedC1)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                case 13:
                    SelectedPlanets = _context.Planets
                        .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                        .Where(p => p.Type == PlanetType.AssimilatedC2)
                        .OrderBy(p => p.DateTimeAcquired)
                        .Take(4)
                        .ToList();
                    break;
                    case 14:
                    SelectedPlanets = _context.Planets
                    .Where(p => p.ApplicationUserId == user.Id && !p.Name.Contains("H."))
                    .Where(p => p.Type == PlanetType.SimilareC4)
                    .OrderBy(p => p.DateTimeAcquired)
                    .Take(4)
                    .ToList(); 
                    break;
                default:
                    break;
            }
            SelectedPlanetIds = SelectedPlanets.Select(p => p.Id).ToList();
            PopulateMineralTypeSelectList();

            return Page();
        }

        private void PopulateMineralTypeSelectList()
        {
            MineralTypeSelectList = Enum.GetValues(typeof(MineralType))
                            .Cast<MineralType>()
                            .Select(mt => new SelectListItem
                            {
                                Value = mt.ToString(),
                                Text = mt.ToString()
                            })
                            .ToList();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            PopulateMineralTypeSelectList();

            if (string.IsNullOrEmpty(SelectedMineralType))
            {
                ModelState.AddModelError("SelectedMineralType", "Please select a mineral type.");
                return Page();
            }

            // Retrieve the selected planets from the database
            var selectedPlanets = _context.Planets
                .Where(p => SelectedPlanetIds.Contains(p.Id))
                .Where(p => p.ApplicationUserId == user.Id)
                .OrderBy(p => p.DateTimeAcquired)
                .ToList();

            // ... your logic using selectedPlanets and SelectedMineralType ...
            switch (id)
            {
                case 1:
                    // Handle Cluster Level 1 creation logic
                    var newCluster = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.ClusterLevel1,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C1." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 5,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) + selectedPlanets.Sum(p => p.Housing) + selectedPlanets.Sum(p => p.Commercial) + selectedPlanets.Sum(p => p.Agriculture) + selectedPlanets.Sum(p => p.Industry) + selectedPlanets.Sum(p => p.Mining) + (5 * 1000),
                        PopulationModifier = 1.1m,
                        AgricultureModifier = 1.15m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster);
                    NewPlanet = newCluster;
                    break;

                case 2:
                    // Handle Cluster Level 2 creation logic
                    var newCluster2 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.ClusterLevel2,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C2." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 25,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) + 
                        selectedPlanets.Sum(p => p.Housing) + 
                        selectedPlanets.Sum(p => p.Commercial) + 
                        selectedPlanets.Sum(p => p.Agriculture) + 
                        selectedPlanets.Sum(p => p.Industry) + 
                        selectedPlanets.Sum(p => p.Mining) + (25 * 1000),
                        PopulationModifier = 1.2m,
                        AgricultureModifier = 1.3m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster2);
                    NewPlanet = newCluster2;
                    break;
                case 3:
                    // Handle Cluster Level 3 creation logic
                    var newCluster3 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.ClusterLevel3,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C3." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 125,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (125 * 1000),
                        PopulationModifier = 1.3m,
                        AgricultureModifier = 1.45m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster3);
                    NewPlanet = newCluster3;
                    break;
                case 4:
                    // Handle Tainted C.2 creation logic
                    var newCluster4 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.TaintedC2,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.2." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 4,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (4 * 1000),
                        PopulationModifier = 0.7m,
                        AgricultureModifier = 0.75m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster4);
                    NewPlanet = newCluster4;
                    break;
                case 5:
                    // Handle Tainted C.3 creation logic
                    var newCluster5 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.TaintedC3,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.3." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 16,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (16 * 1000),
                        PopulationModifier = 0.6m,
                        AgricultureModifier = 0.7m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster5);
                    NewPlanet = newCluster5;
                    break;
                case 6:
                    // Handle Tainted C.4 creation logic
                    var newCluster6 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.TaintedC4,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.4." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 64,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (64 * 1000),
                        PopulationModifier = 0.5m,
                        AgricultureModifier = 0.65m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster6);
                    NewPlanet = newCluster6;
                    break;
                case 7:
                    // Handle Infected C.2 creation logic
                    var newCluster7 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.InfectedC2,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.2." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 25,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (25 * 1000),
                        PopulationModifier = 0.65m,
                        AgricultureModifier = 1m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster7);
                    NewPlanet = newCluster7;
                    break;
                case 8:
                    // Handle Infected C.3 creation logic
                    var newCluster8 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.InfectedC3,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.3." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 125,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (125 * 1000),
                        PopulationModifier = 0.6m,
                        AgricultureModifier = 1.1m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster8);
                    NewPlanet = newCluster8;
                    break;
                case 9:
                    // Handle Similare C.2 creation logic
                    var newCluster9 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.SimilareC2,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.2." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 4,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (4 * 1000),
                        PopulationModifier = 1.2m,
                        AgricultureModifier = 0.35m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster9);
                    NewPlanet = newCluster9;
                    break;
                case 10:
                    // Handle Similare C.3 creation logic
                    var newCluster10 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.SimilareC3,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.3." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 16,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (15 * 1000),
                        PopulationModifier = 1.2m,
                        AgricultureModifier = 0.4m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster10);
                    NewPlanet = newCluster10;
                    break;
                case 11:
                    // Handle Similare C.4 creation logic
                    var newCluster11 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.SimilareC4,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.4." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 64,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (64 * 1000),
                        PopulationModifier = 1.2m,
                        AgricultureModifier = 0.45m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster11);
                    NewPlanet = newCluster11;
                    break;
                case 12:
                    // Handle Assimilated C.2 creation logic
                    var newCluster12 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.AssimilatedC2,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C2." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 25,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (25 * 1000),
                        PopulationModifier = 1m,
                        AgricultureModifier = 0.9m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster12);
                    NewPlanet = newCluster12;
                    break;
                case 13:
                    // Handle Assimilated C.3 creation logic
                    var newCluster13 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.AssimilatedC3,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C3." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 125,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (125 * 1000),
                        PopulationModifier = 1m,
                        AgricultureModifier = 1m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster13);
                    NewPlanet = newCluster13;
                    break;
                    case 14:
                    // Handle Assimilated C.5 creation logic
                    var newCluster14 = new Planets
                    {
                        ApplicationUserId = user.Id,
                        Type = PlanetType.SimilareC5,
                        MineralProduced = Enum.Parse<MineralType>(SelectedMineralType),
                        DateTimeAcquired = selectedPlanets.FirstOrDefault()?.DateTimeAcquired ?? DateTime.UtcNow,
                        Housing = selectedPlanets.Sum(p => p.Housing),
                        Commercial = selectedPlanets.Sum(p => p.Commercial),
                        Agriculture = selectedPlanets.Sum(p => p.Agriculture),
                        Industry = selectedPlanets.Sum(p => p.Industry),
                        Mining = selectedPlanets.Sum(p => p.Mining),
                        MaxPopulation = selectedPlanets.Sum(p => p.MaxPopulation),
                        CurrentPopulation = selectedPlanets.Sum(p => p.CurrentPopulation),
                        Name = "C.5." + Random.Shared.Next(1000, 9999).ToString(),
                        AvailableLabour = selectedPlanets.Sum(p => p.AvailableLabour),
                        AvailableOre = selectedPlanets.Sum(p => p.AvailableOre),
                        FoodRequired = selectedPlanets.Sum(p => p.FoodRequired),
                        GoodsRequired = selectedPlanets.Sum(p => p.GoodsRequired),
                        LandAvailable = selectedPlanets.Sum(p => p.LandAvailable),
                        TotalLand = selectedPlanets.Sum(p => p.TotalLand),
                        TotalPlanets = 256,
                        Loyalty = selectedPlanets.Sum(p => p.Loyalty) / 5,
                        PowerRating = (selectedPlanets.Sum(p => p.MaxPopulation) / 10) +
                        selectedPlanets.Sum(p => p.Housing) +
                        selectedPlanets.Sum(p => p.Commercial) +
                        selectedPlanets.Sum(p => p.Agriculture) +
                        selectedPlanets.Sum(p => p.Industry) +
                        selectedPlanets.Sum(p => p.Mining) + (256 * 1000),
                        PopulationModifier = 1.2m,
                        AgricultureModifier = 0.48m,
                        ArtifactModifier = 0.01m,
                        OreModifier = 0.02m,
                        ApplicationUser = user
                    };
                    _context.Planets.RemoveRange(selectedPlanets);
                    _context.Planets.Add(newCluster14);
                    NewPlanet = newCluster14;
                    break;
                default:
                    ModelState.AddModelError("", "Invalid cluster type selected.");
                    return Page();
            }
            
            return RedirectToPage();
        }
    }
}